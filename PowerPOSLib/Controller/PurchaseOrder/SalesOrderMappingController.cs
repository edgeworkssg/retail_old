using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Transactions;
using System.Collections;
using PowerPOS.Container;
using SubSonic;
using System.ComponentModel;
using System.Configuration;
namespace PowerPOS
{
    public partial class SalesOrderMappingController
    {
        public static bool AddSalesOrderMapping(string orderDetid, string PurchaseOrderDetRefNo, decimal Quantity)
        {
            try
            {
                int salesID = 0;
                if (isExist(orderDetid, PurchaseOrderDetRefNo, out salesID))
                {
                    SalesOrderMapping s = new SalesOrderMapping(salesID);
                    s.OrderDetID = orderDetid;
                    s.PurchaseOrderDetRefNo = PurchaseOrderDetRefNo;
                    s.Qty = Quantity;
                    s.Deleted = false;
                    s.Save(UserInfo.username);
                }
                else
                {
                    SalesOrderMapping s = new SalesOrderMapping();
                    s.OrderDetID = orderDetid;
                    s.PurchaseOrderDetRefNo = PurchaseOrderDetRefNo;
                    s.Qty = Quantity;
                    s.QtyApproved = 0;
                    s.Deleted = false;
                    s.Save(UserInfo.username);
                }
                return true;
            }
            catch (Exception ex) { Logger.writeLog(ex.Message); return false; }
        }

        public static QueryCommandCollection AddSalesOrderMappingToQueryCommand(string orderDetid, string PurchaseOrderDetRefNo, decimal Quantity)
        {
            QueryCommandCollection tmp = new QueryCommandCollection();
            try
            {
                
                int salesID = 0;
                if (isExist(orderDetid, PurchaseOrderDetRefNo, out salesID))
                {
                    SalesOrderMapping s = new SalesOrderMapping(salesID);
                    s.OrderDetID = orderDetid;
                    s.PurchaseOrderDetRefNo = PurchaseOrderDetRefNo;
                    s.Qty = Quantity;
                    s.Deleted = false;
                    tmp.Add(s.GetUpdateCommand(UserInfo.username));
                }
                else
                {
                    SalesOrderMapping s = new SalesOrderMapping();
                    s.OrderDetID = orderDetid;
                    s.PurchaseOrderDetRefNo = PurchaseOrderDetRefNo;
                    s.Qty = Quantity;
                    s.QtyApproved = 0;
                    s.Deleted = false;
                    tmp.Add(s.GetInsertCommand(UserInfo.username));
                }
                return tmp;
            }
            catch (Exception ex) { Logger.writeLog(ex.Message); return tmp; }
        }

        public static bool ApproveAllSalesOrderMapping(string PurchaseOrderDetRefNo)
        {
            try
            {
                SalesOrderMappingCollection somCol = new SalesOrderMappingCollection();
                somCol.Where(SalesOrderMapping.Columns.PurchaseOrderDetRefNo, PurchaseOrderDetRefNo);
                somCol.Load();
                foreach (SalesOrderMapping som in somCol)
                {
                    som.QtyApproved = som.Qty;
                    som.Save(UserInfo.username);
                }
                return true;
            }
            catch (Exception ex) { Logger.writeLog(ex.Message); return false; }
        }

        public static bool ApproveAllSalesOrderMappingToQueryCommand(string PurchaseOrderDetRefNo, out QueryCommandCollection qmc)
        {
            qmc = new QueryCommandCollection();
            try
            {
                
                SalesOrderMappingCollection somCol = new SalesOrderMappingCollection();
                somCol.Where(SalesOrderMapping.Columns.PurchaseOrderDetRefNo, PurchaseOrderDetRefNo);
                somCol.Load();
                foreach (SalesOrderMapping som in somCol)
                {
                    som.QtyApproved = som.Qty;
                    qmc.Add(som.GetUpdateCommand(UserInfo.username));
                    //som.Save(UserInfo.username);
                }
                return true;
            }
            catch (Exception ex) { Logger.writeLog(ex.Message); return false; }
        }

        public static bool UpdateApproved(string orderDetid, string PurchaseOrderDetRefNo, decimal Quantity)
        {
            try
            {
                int salesID = 0;
                if (isExist(orderDetid, PurchaseOrderDetRefNo, out salesID))
                {
                    SalesOrderMapping s = new SalesOrderMapping(salesID);
                    s.OrderDetID = orderDetid;
                    s.PurchaseOrderDetRefNo = PurchaseOrderDetRefNo;
                    s.QtyApproved = Quantity;
                    s.Deleted = false;
                    s.Save(UserInfo.username);
                }
                else
                { return false; }

                return true;
            }
            catch (Exception ex) { Logger.writeLog(ex.Message); return false; }
        }

        public static bool isExist(string orderDetid, string PurchaseOrderDetRefNo, out int salesID)
        {
            salesID = 0;
            SalesOrderMappingCollection col = new SalesOrderMappingCollection();
            col.Where(SalesOrderMapping.Columns.OrderDetID, orderDetid);
            col.Where(SalesOrderMapping.Columns.PurchaseOrderDetRefNo, PurchaseOrderDetRefNo);
            col.Where(SalesOrderMapping.Columns.Deleted, false);
            col.Load();
            if (col.Count > 0)
            {
                salesID = col[0].SalesOrderMappingID;
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool DistributeBackOrderSalesMapping(string PurchaseOrderDetRefNo, string backOrderRefNo, decimal Quantity)
        {
            try
            {
                SalesOrderMappingCollection col = new SalesOrderMappingCollection();
                col.Where(SalesOrderMapping.Columns.PurchaseOrderDetRefNo, PurchaseOrderDetRefNo);
                col.Where(SalesOrderMapping.Columns.Deleted, false);
                col.OrderByAsc(SalesOrderMapping.Columns.SalesOrderMappingID);
                col.Load();


                PurchaseOrderDetail pod = new PurchaseOrderDetail(PurchaseOrderDetRefNo);
                decimal quantity = pod.Quantity ?? 0;
                decimal ApprovedQuantity = (pod.Quantity ?? 0) + pod.RejectQty - Quantity;
                foreach (SalesOrderMapping som in col)
                {
                    OrderDet od = new OrderDet(som.OrderDetID);
                    if (som.Qty <= ApprovedQuantity)
                    {
                        ApprovedQuantity -= (int)som.Qty;
                        UpdateApproved(od.OrderDetID, PurchaseOrderDetRefNo, (int)som.Qty);
                    }
                    else
                    {
                        if (ApprovedQuantity >= 0)
                        {
                            UpdateApproved(od.OrderDetID, PurchaseOrderDetRefNo, ApprovedQuantity);
                        }
                        AddSalesOrderMapping(od.OrderDetID, backOrderRefNo, (decimal) som.Qty - ApprovedQuantity);
                        ApprovedQuantity = 0;
                    }

                }
                return true;
            }
            catch (Exception ex) { Logger.writeLog(ex.Message); return false; }
        }

        public static QueryCommandCollection DistributeBackOrderSalesMappingToQueryCommand(string PurchaseOrderDetRefNo, string backOrderRefNo, decimal Quantity)
        {
            QueryCommandCollection tmp = new QueryCommandCollection();
            try
            {
                SalesOrderMappingCollection col = new SalesOrderMappingCollection();
                col.Where(SalesOrderMapping.Columns.PurchaseOrderDetRefNo, PurchaseOrderDetRefNo);
                col.Where(SalesOrderMapping.Columns.Deleted, false);
                col.OrderByAsc(SalesOrderMapping.Columns.SalesOrderMappingID);
                col.Load();


                

                PurchaseOrderDetail pod = new PurchaseOrderDetail(PurchaseOrderDetRefNo);
                decimal quantity = pod.Quantity ?? 0;
                decimal ApprovedQuantity = (pod.Quantity ?? 0) + pod.RejectQty - Quantity;
                int salesID = 0;
                foreach (SalesOrderMapping som in col)
                {
                    OrderDet od = new OrderDet(som.OrderDetID);
                    if (som.Qty <= ApprovedQuantity)
                    {
                        
                        ApprovedQuantity -= (int)som.Qty;
                        if (isExist(od.OrderDetID, PurchaseOrderDetRefNo, out salesID))
                        {
                            SalesOrderMapping s = new SalesOrderMapping(salesID);
                            s.OrderDetID = od.OrderDetID;
                            s.PurchaseOrderDetRefNo = PurchaseOrderDetRefNo;
                            s.QtyApproved = (int)som.Qty;
                            s.Deleted = false;
                            tmp.Add(s.GetUpdateCommand(UserInfo.username));
                        }
                        //UpdateApproved(od.OrderDetID, PurchaseOrderDetRefNo, (int)som.Qty);
                    }
                    else
                    {
                        if (ApprovedQuantity >= 0)
                        {
                            if (isExist(od.OrderDetID, PurchaseOrderDetRefNo, out salesID))
                            {
                                SalesOrderMapping s = new SalesOrderMapping(salesID);
                                s.OrderDetID = od.OrderDetID;
                                s.PurchaseOrderDetRefNo = PurchaseOrderDetRefNo;
                                s.QtyApproved = ApprovedQuantity;
                                s.Deleted = false;
                                tmp.Add(s.GetUpdateCommand(UserInfo.username));
                            }
                            //UpdateApproved(od.OrderDetID, PurchaseOrderDetRefNo, ApprovedQuantity);
                        }
                        tmp.AddRange(AddSalesOrderMappingToQueryCommand(od.OrderDetID, backOrderRefNo, (decimal)som.Qty - ApprovedQuantity));
                        ApprovedQuantity = 0;
                    }

                }
                return tmp;
            }
            catch (Exception ex) { Logger.writeLog(ex.Message); return tmp; }
        }

        public static bool ClearPurchaseOrderReference(string PurchaseOrderDetRefNo)
        {
            try
            {
                SalesOrderMappingCollection col = new SalesOrderMappingCollection();
                col.Where(SalesOrderMapping.Columns.PurchaseOrderDetRefNo, PurchaseOrderDetRefNo);
                col.Load();
                foreach (SalesOrderMapping som in col)
                {
                    som.Deleted = true;
                    som.Save(UserInfo.username);
                }
                return true;

            }
            catch (Exception ex) { Logger.writeLog(ex.Message); return false; }
        }

        public static bool ClearPurchaseOrderReferenceHeader(string PurchaseOrderHeaderRefNo)
        {
            try
            {
                PurchaseOrderDetailCollection podColl = new PurchaseOrderDetailCollection();
                podColl.Where(PurchaseOrderDetail.Columns.PurchaseOrderHeaderRefNo, PurchaseOrderHeaderRefNo);
                podColl.Load();

                foreach (PurchaseOrderDetail pod in podColl)
                {
                    SalesOrderMappingCollection col = new SalesOrderMappingCollection();
                    col.Where(SalesOrderMapping.Columns.PurchaseOrderDetRefNo, pod.PurchaseOrderDetailRefNo);
                    col.Load();
                    foreach (SalesOrderMapping som in col)
                    {
                        som.Deleted = true;
                        som.Save(UserInfo.username);
                    }
                }


                return true;

            }
            catch (Exception ex) { Logger.writeLog(ex.Message); return false; }
        }
    }
}
