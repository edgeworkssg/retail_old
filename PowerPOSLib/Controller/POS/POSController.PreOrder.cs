using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PowerPOS;
using PowerPOS.Container;
using SubSonic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Collections;

namespace PowerPOS
{
    public partial class POSController
    {
        public bool CreateDeliveryPreOrderSingleOrderDet(string orderdetid, int qty, int personnelID, bool fromWeb)
        {
            return CreateDeliveryPreOrderSingleOrderDet(orderdetid, qty, personnelID, null, fromWeb);
        }

        public bool CreateDeliveryPreOrderSingleOrderDet(string orderdetid, int qty, int personnelID, Bitmap signature)
        {
            return CreateDeliveryPreOrderSingleOrderDet(orderdetid, qty, personnelID, signature, false);
        }

        public bool CreateDeliveryPreOrderSingleOrderDet(string orderdetid, int qty, int personnelID, Bitmap signature, bool fromWeb)
        {
            try
            {
                OrderDet od = new OrderDet(orderdetid);

                OrderHdr oh = new OrderHdr(od.OrderHdrID);
                Membership member = new Membership(oh.MembershipNo);

                DeliveryController dc = new DeliveryController(oh.PointOfSaleID);  
                DeliveryOrder doHdr = new DeliveryOrder();

                doHdr.CopyFrom(dc.myDeliveryOrderHdr);
                if (fromWeb)
                {
                    // Append "W" to the OrderNumber so it won't conflict with DO created from POS
                    doHdr.OrderNumber += "W";
                    doHdr.PurchaseOrderRefNo += "W";
                }
                doHdr.MembershipNo = member.MembershipNo;
                doHdr.DeliveryAddress = string.Format("{0} {1} {2} {3} {4}", member.StreetName, member.StreetName2, member.UnitNo, member.City, member.Country);
                doHdr.DeliveryDate = DateTime.Now;
                doHdr.RecipientName = member.NameToAppear;
                doHdr.MobileNo = member.Mobile;
                doHdr.HomeNo = member.Home;
                doHdr.SalesOrderRefNo = oh.OrderHdrID;
                doHdr.PersonAssigned = personnelID;
                doHdr.Deleted = false;
                if (signature != null)
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        signature.Save(stream, ImageFormat.Jpeg);
                        //doHdr.Signature = stream.ToArray();
                    }
                }
                
                DeliveryOrderDetailCollection doDetCol = new DeliveryOrderDetailCollection();
                DeliveryOrderDetail doDet = new DeliveryOrderDetail();
                doDet.Dohdrid = doHdr.OrderNumber.ToString();                
                doDet.ItemNo = od.ItemNo;
                doDet.Quantity = qty;
                doDet.DetailsID = doDet.Dohdrid + ".0";
                doDet.OrderDetID = od.OrderDetID;
                doDet.Deleted = false;

                doDetCol.Add(doDet);

                DeliveryOrderController.SaveOrder(doHdr, doDetCol);

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error create delivery order : " + ex.Message);
                return false;
            }
        }

        public bool CreateDeliveryPreOrderSingleOrderDet(string orderdetid, int qty, int personnelID, int pointOfSaleID, Bitmap signature, bool fromWeb)
        {
            try
            {
                OrderDet od = new OrderDet(orderdetid);

                OrderHdr oh = new OrderHdr(od.OrderHdrID);
                Membership member = new Membership(oh.MembershipNo);

                DeliveryController dc = new DeliveryController(pointOfSaleID);
                DeliveryOrder doHdr = new DeliveryOrder();

                doHdr.CopyFrom(dc.myDeliveryOrderHdr);
                if (fromWeb)
                {
                    // Append "W" to the OrderNumber so it won't conflict with DO created from POS
                    doHdr.OrderNumber += "W";
                    doHdr.PurchaseOrderRefNo += "W";
                }
                doHdr.MembershipNo = member.MembershipNo;
                doHdr.DeliveryAddress = string.Format("{0} {1} {2} {3} {4}", member.StreetName, member.StreetName2, member.UnitNo, member.City, member.Country);
                doHdr.DeliveryDate = DateTime.Now;
                doHdr.RecipientName = member.NameToAppear;
                doHdr.MobileNo = member.Mobile;
                doHdr.HomeNo = member.Home;
                doHdr.SalesOrderRefNo = oh.OrderHdrID;
                doHdr.PersonAssigned = personnelID;
                doHdr.Deleted = false;
                if (signature != null)
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        signature.Save(stream, ImageFormat.Jpeg);
                        //doHdr.Signature = stream.ToArray();
                    }
                }

                DeliveryOrderDetailCollection doDetCol = new DeliveryOrderDetailCollection();
                DeliveryOrderDetail doDet = new DeliveryOrderDetail();
                doDet.Dohdrid = doHdr.OrderNumber.ToString();
                doDet.ItemNo = od.ItemNo;
                doDet.Quantity = qty;
                doDet.DetailsID = doDet.Dohdrid + ".0";
                doDet.OrderDetID = od.OrderDetID;
                doDet.Deleted = false;

                doDetCol.Add(doDet);

                DeliveryOrderController.SaveOrder(doHdr, doDetCol);

                od.PreOrderPointOfSalesID = pointOfSaleID;
                od.Save();

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error create delivery order : " + ex.Message);
                return false;
            }
        }

        public bool CreateDeliveryPreOrderSingleOrderDet(string orderdetid, string orderhdrid, string membershipno, int pointOfSaleID, string itemNo,int qty, int personnelID, Bitmap signature, bool fromWeb)
        {
            try
            {
                Membership member = new Membership(membershipno);

                DeliveryController dc = new DeliveryController(pointOfSaleID);
                DeliveryOrder doHdr = new DeliveryOrder();

                doHdr.CopyFrom(dc.myDeliveryOrderHdr);
                if (fromWeb)
                {
                    // Append "W" to the OrderNumber so it won't conflict with DO created from POS
                    doHdr.OrderNumber += "W";
                    doHdr.PurchaseOrderRefNo += "W";
                }
                doHdr.MembershipNo = member.MembershipNo;
                doHdr.DeliveryAddress = string.Format("{0} {1} {2} {3} {4}", member.StreetName, member.StreetName2, member.UnitNo, member.City, member.Country);
                doHdr.DeliveryDate = DateTime.Now;
                doHdr.RecipientName = member.NameToAppear;
                doHdr.MobileNo = member.Mobile;
                doHdr.HomeNo = member.Home;
                doHdr.SalesOrderRefNo = orderhdrid;
                doHdr.PersonAssigned = personnelID;
                doHdr.Deleted = false;
                if (signature != null)
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        signature.Save(stream, ImageFormat.Jpeg);
                        //doHdr.Signature = stream.ToArray();
                    }
                }

                DeliveryOrderDetailCollection doDetCol = new DeliveryOrderDetailCollection();
                DeliveryOrderDetail doDet = new DeliveryOrderDetail();
                doDet.Dohdrid = doHdr.OrderNumber.ToString();
                doDet.ItemNo = itemNo;
                doDet.Quantity = qty;
                doDet.DetailsID = doDet.Dohdrid + ".0";
                doDet.OrderDetID = orderdetid;
                doDet.Deleted = false;

                doDetCol.Add(doDet);

                DeliveryOrderController.SaveOrder(doHdr, doDetCol);

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error create delivery order : " + ex.Message);
                return false;
            }
        }


        public bool DoStockOutPreOrderSingle(string orderdetid, decimal qty, out string status) 
        {
            status = "";
            bool objreturn = false;
            try
            {
                #region *) Validation: Don't do anything if IntegrateWithInventory is false
                /* If not IntegrateWithInventory, Stock Out will be done at server */
                if (!PointOfSaleInfo.IntegrateWithInventory)
                {
                    return true;
                }
                #endregion

                #region *) Validation: No movement is allowed when there are un-adjusted stock take in place
                /* Return true because we don't want to disturb the cashier with warning */
                if (StockTakeController.IsThereUnAdjustedStockTake())
                {
                    Logger.writeLog("There are Stock Take in progress, no stock movement is allowed.");
                    return true;
                }
                #endregion

                #region *) Validation: Make sure POS is attached to InventoryLocation
                if (PointOfSaleInfo.InventoryLocationID == -1)
                {
                    status = "POS is not attached to any Inventory Location.\nPlease contact your administrator";
                    Logger.writeLog("WARNING: POS is not attached to any InventoryLocation.");
                    return false;
                }
                #endregion

                OrderDet od = new OrderDet(orderdetid);

                //if (od.InventoryHdrRefNo != null && od.InventoryHdrRefNo != "")
                //    return false;

                
                QueryCommandCollection Result = new QueryCommandCollection();

                int InventoryLocationID;
                #region *) Validation: Make sure POS is attached to InventoryLocation
                InventoryLocationID = od.OrderHdr.PointOfSale.Outlet.InventoryLocationID.GetValueOrDefault(-1);
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
                    return false;
                }
                #endregion

                if (od.OrderHdr.IsVoided || !od.Item.IsInInventory)
                {
                    #region *) Script: Mark OrderDet as NONINVENTORY
                    //if non inventory
                    Query qr = OrderDet.CreateQuery();
                    qr.AddWhere(OrderDet.Columns.OrderDetID, orderdetid);
                    qr.AddUpdateSetting(OrderDet.Columns.InventoryHdrRefNo, "NONINVENTORY");
                    qr.AddUpdateSetting(OrderDet.Columns.Userflag1, true);
                    Result.Add(qr.BuildCommand());
                    #endregion
                }
                else
                {
                    //Find the last stock take date for the location
                    DateTime lastStockTakeDate;
                    #region *) Fetch: Get Last Stock Take Date
                    lastStockTakeDate = StockTakeController.FetchLastStockTakeDateByInventoryLocationByItemNo(InventoryLocationID, od.ItemNo);
                    #endregion

                    decimal balQty;
                    balQty = InventoryController.GetStockBalanceQtyByItemByDate(od.ItemNo, InventoryLocationID, DateTime.Now, out status);

                    if (od.OrderDetDate < lastStockTakeDate)
                    {
                        #region *) Script: Update the OrderDet.InventoryHdrRefNo
                        Query qr = new Query("OrderDet");
                        qr.QueryType = QueryType.Update;
                        qr.AddWhere(OrderDet.Columns.OrderDetID, od.OrderDetID);
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
                        int newIndex = GetIndexInventoryHDRRefNo(od.OrderDetID);
                        invCtr.SetInventoryHdrRefNo(string.Format("{0}{1}",od.OrderDetID.Replace(".", ""), newIndex.ToString("00")));
                        invCtr.setInvoiceNo(od.OrderDetID);
                        if (UseOrderDate)
                            invCtr.SetInventoryDate(od.OrderDetDate);
                        else
                            invCtr.SetInventoryDate(DateTime.Now);
                        if (qty >= 0)
                        {
                            invCtr.AddItemIntoInventory(od.ItemNo, qty, out status);

                            /// TODO: Handle the negative qty sales
                            if (!invCtr.CreateStockOutQueryCommand("SYSTEM", 0, InventoryLocationID, false, true, out status, out cmd))
                                throw new Exception(status);
                        }
                        else
                        {
                            invCtr.SetInventoryHeaderInfo("", "", "Stock in for order " + od.OrderDetID, 0.0M, 1, 0.0M);

                            invCtr.AddItemIntoInventory(od.ItemNo, -qty
                                , InventoryController.FetchAverageCostOfGoodsLeftByItemNo(balQty, od.ItemNo, InventoryLocationID)
                                , out status);

                            cmd = invCtr.CreateStockInQueryCommand("SYSTEM", InventoryLocationID, true, true);
                        }

                        Result.AddRange(cmd);
                        #endregion

                        #region *) Script: Update the OrderDet.InventoryHdrRefNo
                        Query qr = new Query("OrderDet");
                        qr.QueryType = QueryType.Update;
                        qr.AddWhere(OrderDet.Columns.OrderDetID,od.OrderDetID);
                        qr.AddUpdateSetting(OrderDet.Columns.InventoryHdrRefNo, invCtr.GetInvHdrRefNo());
                        Result.Add(qr.BuildUpdateCommand());
                        #endregion
                    }
                }

                for (int Counter = Result.Count - 1; Counter >= 0; Counter--)
                    if (Result[Counter] == null) Result.RemoveAt(Counter);

                DataService.ExecuteTransaction(Result);

                objreturn = true;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error create delivery order : " + ex.Message);
            }

            return objreturn;
        }

        public int GetIndexInventoryHDRRefNo(string orderdetid)
        {
            int objreturn = 0;
            string sql = "select convert(int, isnull(max(right(inventoryhdrrefno,2)),0)) + 1 as NewIndexInvRefNo " +
                            "from inventoryhdr " +
                            "where InvoiceNo = '" + orderdetid + "'";

            DataSet ds = DataService.GetDataSet(new QueryCommand(sql));

            if (ds != null && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0][0] != null) 
            {
                objreturn = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
            }

            return objreturn;
        }

        public static string GetDataOrderDetForRefund(string orderhdrid, string orderdetid, int pointOfSaleID, bool AllowRefundForSameOutlet)
        {
            string status = "";
            OrderDet odReturn = new OrderDet();
            bool AllowRefundForOtherOutlet = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Refund.RefundReceiptFromOtherOutlet), false);
            try
            {
                OrderHdr oh = new OrderHdr(orderhdrid);
                PointOfSale p = new PointOfSale(pointOfSaleID);
                
                if (!oh.IsNew)
                {   
                    if (!AllowRefundForOtherOutlet && !AllowRefundForSameOutlet)
                    {
                        if (oh.PointOfSaleID != pointOfSaleID)
                        {
                            throw new Exception( "Order is valid, but can only be refunded in " + oh.PointOfSale.PointOfSaleName);                            
                        }
                    }
                    if (!AllowRefundForOtherOutlet && AllowRefundForSameOutlet)
                    {
                        if (oh.PointOfSale.OutletName != p.OutletName)
                        {
                            status = "Receipt " + oh.OrderRefNo + " is valid, but refund can be done at " + oh.PointOfSale.OutletName + " outlet";                            
                        }
                    }

                    if (oh.IsVoided)
                    {
                        throw new Exception("Receipt " + oh.OrderRefNo + " is voided.");
                    }

                }
                else
                {
                    throw new Exception("Order " + oh.OrderRefNo + " not found.");
                }

                POSController pos = new POSController(oh.OrderHdrID);
                if (pos != null && pos.myOrderHdr != null && pos.myOrderHdr.OrderHdrID != "")
                {
                    #region validation is Refunded
                    
                    foreach (OrderDet od in pos.myOrderDet)
                    {
                        if (od.OrderDetID != orderdetid)
                            continue;

                        decimal qtyRefunded = 0;
                        if (POSController.isRefunded(od.OrderDetID, out qtyRefunded) || (od.Quantity < 0 && od.ItemNo != "LINE_DISCOUNT"))
                        {
                            if (qtyRefunded < od.Quantity)
                            {
                                od.Quantity = (od.Quantity - qtyRefunded) * -1;
                                od.ReturnedReceiptNo = pos.myOrderHdr.Userfld5;
                                od.RefundOrderDetID = od.OrderDetID;
                                od.InventoryHdrRefNo = "";
                                OrderDet tempOD = od;
                                pos.CalculateLineAmount(ref tempOD);
                                status = "Warning. Some items has already refunded.";

                                odReturn = od.Clone();
                            }                            
                        }
                        else
                        {
                            if (!POSController.isRefunded(od.OrderDetID, out qtyRefunded))
                            {
                                od.Quantity = od.Quantity * -1;
                                od.ReturnedReceiptNo = pos.myOrderHdr.Userfld5;
                                od.RefundOrderDetID = od.OrderDetID;
                                od.InventoryHdrRefNo = "";
                                OrderDet tempOD = od;
                                pos.CalculateLineAmount(ref tempOD);

                                odReturn = od.Clone();
                            }
                        }
                    }
                    
                    #endregion

                    
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex.Message);
                status = "Error : " + ex.Message;
            }

            var result = new OrderDetReturnWithStatus () { status = status, orderDet = odReturn };

            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }

        public static string GetDataOrderDetForRefundStr(string orderhdrid, string orderdetid, int currentPosID, bool AllowRefundForSameOutlet, out string status)
        {
            status = "";
            bool AllowRefundForOtherOutlet = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Refund.RefundReceiptFromOtherOutlet), false);
            try
            {
                PointOfSale p = new PointOfSale(currentPosID);
                if (p == null || p.PointOfSaleID == null)
                    return null;

                OrderHdrCollection ohCol = new OrderHdrCollection();
                ohCol.Where(OrderHdr.Columns.OrderHdrID, orderhdrid);
                ohCol.Load();

                if (ohCol.Count == 0)
                {
                    ohCol.Where(OrderHdr.Columns.OrderHdrID, orderhdrid);
                    ohCol.Load();
                }

                if (ohCol.Count > 0)
                {

                    if (!AllowRefundForOtherOutlet && !AllowRefundForSameOutlet)
                    {
                        if (ohCol[0].PointOfSaleID != currentPosID)
                        {
                            status = "Order is valid, but can only be refunded in " + ohCol[0].PointOfSale.PointOfSaleName;
                            return null;
                        }
                    }
                    if (!AllowRefundForOtherOutlet && AllowRefundForSameOutlet)
                    {
                        if (ohCol[0].PointOfSale.OutletName != p.OutletName)
                        {
                            status = "Receipt " + ohCol[0].OrderRefNo + " is valid, but refund can be done at " + ohCol[0].PointOfSale.OutletName + " outlet";
                            return null;
                        }
                    }
                    if (ohCol[0].IsVoided)
                    {
                        status = "Receipt " + ohCol[0].OrderRefNo + " is voided.";
                        return null;
                    }

                }
                else
                {
                    status = "Order " + ohCol[0].OrderRefNo + " not found.";
                    return null;
                }
                POSController pos = new POSController(ohCol[0].OrderHdrID);
                if (pos != null && pos.myOrderHdr != null && pos.myOrderHdr.OrderHdrID != "")
                {
                    #region validation is Refunded
                    ArrayList tmpRemove = new ArrayList();
                    foreach (OrderDet od in pos.myOrderDet)
                    {
                        if (od.OrderDetID != orderdetid)
                        {
                            tmpRemove.Add(od.OrderDetID);
                            continue;
                        }

                        decimal qtyRefunded = 0;
                        if (POSController.isRefunded(od.OrderDetID, out qtyRefunded) || (od.Quantity < 0 && od.ItemNo != "LINE_DISCOUNT"))
                        {
                            if (qtyRefunded < od.Quantity)
                            {
                                od.Quantity = (od.Quantity - qtyRefunded) * -1;
                                od.ReturnedReceiptNo = ohCol[0].Userfld5;
                                od.RefundOrderDetID = od.OrderDetID;
                                od.InventoryHdrRefNo = "";
                                OrderDet tempOD = od;
                                pos.CalculateLineAmount(ref tempOD);
                                status = "Warning. Some items has already refunded.";
                            }
                            else
                            {
                                tmpRemove.Add(od.OrderDetID);
                            }
                        }
                        else
                        {
                            if (!POSController.isRefunded(od.OrderDetID, out qtyRefunded))
                            {
                                od.Quantity = od.Quantity * -1;
                                od.ReturnedReceiptNo = ohCol[0].Userfld5;
                                od.RefundOrderDetID = od.OrderDetID;
                                od.InventoryHdrRefNo = "";
                                OrderDet tempOD = od;
                                pos.CalculateLineAmount(ref tempOD);
                            }
                        }
                    }
                    //if (tmpRemove.Count == pos.myOrderDet.Count)
                    //{
                    //    status = "Order " + pos.myOrderHdr.OrderRefNo + " has been refunded previously.";
                    //    return null;
                    //}
                    //else
                    //{
                    if (tmpRemove.Count > 0)
                    {
                        for (int i = 0; i < tmpRemove.Count; i++)
                        {
                            OrderDet od = (OrderDet)pos.myOrderDet.Find(tmpRemove[i].ToString());
                            if (od != null && od.OrderDetID != "")
                                pos.myOrderDet.Remove(od);
                        }
                        //status = "Warning. Some items has already refunded.";
                    }
                    //}
                    #endregion

                    //res = Newtonsoft.Json.JsonConvert.SerializeObject(pos);
                    DataTable dtOrderHdr = ohCol.ToDataTable();
                    DataTable dtOrderDet = pos.myOrderDet.ToDataTable();
                    DataSet ds = new DataSet();
                    ds.Tables.Add(dtOrderHdr);
                    ds.Tables.Add(dtOrderDet);

                    return Newtonsoft.Json.JsonConvert.SerializeObject(ds);
                }
                return null;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex.Message);
                return null;
            }
        }

        public static byte[] GetDataOrderDetForRefund(string orderhdrid, string orderdetid, int currentPosID, bool AllowRefundForSameOutlet, out string status)
        {
            status = "";
            bool AllowRefundForOtherOutlet = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Refund.RefundReceiptFromOtherOutlet), false);
            try
            {
                PointOfSale p = new PointOfSale(currentPosID);
                if (p == null || p.PointOfSaleID == null)
                    return null;

                OrderHdrCollection ohCol = new OrderHdrCollection();
                ohCol.Where(OrderHdr.Columns.OrderHdrID, orderhdrid);
                ohCol.Load();

                if (ohCol.Count == 0)
                {
                    ohCol.Where(OrderHdr.Columns.OrderHdrID, orderhdrid);
                    ohCol.Load();
                }

                if (ohCol.Count > 0)
                {

                    if (!AllowRefundForOtherOutlet && !AllowRefundForSameOutlet)
                    {
                        if (ohCol[0].PointOfSaleID != currentPosID)
                        {
                            status = "Warning. Order is valid, but can only be refunded in " + ohCol[0].PointOfSale.PointOfSaleName;
                            return null;
                        }
                    }
                    if (!AllowRefundForOtherOutlet && AllowRefundForSameOutlet)
                    {
                        if (ohCol[0].PointOfSale.OutletName != p.OutletName)
                        {
                            status = "Warning. Receipt " + ohCol[0].OrderRefNo + " is valid, but refund can be done at " + ohCol[0].PointOfSale.OutletName + " outlet";
                            return null;
                        }
                    }
                    if (ohCol[0].IsVoided)
                    {
                        status = "Warning. Receipt " + ohCol[0].OrderRefNo + " is voided.";
                        return null;
                    }

                }
                else
                {
                    status = "Order " + ohCol[0].OrderRefNo + " not found.";
                    return null;
                }
                POSController pos = new POSController(ohCol[0].OrderHdrID);
                if (pos != null && pos.myOrderHdr != null && pos.myOrderHdr.OrderHdrID != "")
                {
                    #region validation is Refunded
                    ArrayList tmpRemove = new ArrayList();
                    foreach (OrderDet od in pos.myOrderDet)
                    {
                        if (od.OrderDetID != orderdetid)
                        {
                            tmpRemove.Add(od.OrderDetID);
                            continue;
                        }

                        decimal qtyRefunded = 0;
                        if (POSController.isRefunded(od.OrderDetID, out qtyRefunded) || (od.Quantity < 0 && od.ItemNo != "LINE_DISCOUNT"))
                        {
                            if (qtyRefunded < od.Quantity)
                            {
                                od.Quantity = (od.Quantity - qtyRefunded) * -1;
                                od.ReturnedReceiptNo = ohCol[0].Userfld5;
                                od.RefundOrderDetID = od.OrderDetID;
                                od.InventoryHdrRefNo = "";
                                OrderDet tempOD = od;
                                pos.CalculateLineAmount(ref tempOD);
                                status = "Warning. Some items has already refunded.";
                            }
                            else
                            {
                                tmpRemove.Add(od.OrderDetID);
                            }
                        }
                        else
                        {
                            if (!POSController.isRefunded(od.OrderDetID, out qtyRefunded))
                            {
                                od.Quantity = od.Quantity * -1;
                                od.ReturnedReceiptNo = ohCol[0].Userfld5;
                                od.RefundOrderDetID = od.OrderDetID;
                                od.InventoryHdrRefNo = "";
                                OrderDet tempOD = od;
                                pos.CalculateLineAmount(ref tempOD);
                            }
                        }
                    }
                    //if (tmpRemove.Count == pos.myOrderDet.Count)
                    //{
                    //    status = "Order " + pos.myOrderHdr.OrderRefNo + " has been refunded previously.";
                    //    return null;
                    //}
                    //else
                    //{
                        if (tmpRemove.Count > 0)
                        {
                            for (int i = 0; i < tmpRemove.Count; i++)
                            {
                                OrderDet od = (OrderDet)pos.myOrderDet.Find(tmpRemove[i].ToString());
                                if (od != null && od.OrderDetID != "")
                                    pos.myOrderDet.Remove(od);
                            }
                            //status = "Warning. Some items has already refunded.";
                        }
                    //}
                    #endregion

                    //res = Newtonsoft.Json.JsonConvert.SerializeObject(pos);
                    DataTable dtOrderHdr = ohCol.ToDataTable();
                    DataTable dtOrderDet = pos.myOrderDet.ToDataTable();
                    DataSet ds = new DataSet();
                    ds.Tables.Add(dtOrderHdr);
                    ds.Tables.Add(dtOrderDet);
                    byte[] data = SyncClientController.CompressDataSetToByteArray(ds);
                    return data;
                }
                return null;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex.Message);
                return null;
            }
        }
            
    }

    public class OrderDetReturnWithStatus
    {
        public string status { get; set; }
        public OrderDet orderDet { get; set; }
    }
}
