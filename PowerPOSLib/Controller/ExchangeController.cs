using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using PowerPOS.Container;
using PowerPOS;
using SubSonic;

namespace PowerPOS
{
    public class ExchangeController
    {
        OrderHdr oHdr;
        OrderDetCollection oDet;
        POSController pos;
        decimal amountPayable;
        private ExchangeLog log;

        public OrderDet FetchItemByItemNo(string itemno)
        {
            for (int i = 0; i < oDet.Count; i++)
            {
                if (itemno == oDet[i].ItemNo)
                {
                    return oDet[i];
                }
            }
            return null;
        } 
        public OrderDet FetchItemByItemName(string itemname)
        {
            for (int i = 0; i < oDet.Count; i++)
            {
                if (itemname == oDet[i].Item.ItemName)
                {
                    return oDet[i];
                }
            }
            return null;
        }
        public OrderDet FetchItemByBarcode(string barcode)
        {
            for (int i = 0; i < oDet.Count; i++)
            {
                if (barcode == oDet[i].Item.Barcode)
                {
                    return oDet[i];
                }
            }
            return null;
        }

        public bool PerformExchange
            (string ReturnedItemNo, int ReturnedQty, 
            string ExchangeItemNo, int ExchangeQty, 
            bool DoStockIn, out string status)
        {
            status = "";
            try
            {
                Item ReturnedItem = new Item("ItemName",ReturnedItemNo);
                Item ExchangeItem = new Item("ItemName",ExchangeItemNo);                                
                
                //Find item in collection and set to be void and allow exchange....
                /*for (int i = 0; i < oDet.Count; i++)
                {
                    if (oDet[i].ItemNo == ReturnedItem.ItemNo  
                        & oDet[i].Quantity >= ReturnedQty
                        & !oDet[i].IsPromo
                        & (!oDet[i].IsFreeOfCharge)
                        & (!oDet[i].IsPromoFreeOfCharge)
                        )
                    {
                        /*oDet[i].Quantity -= ReturnedQty;
                        oDet[i].Save();
                    }
                }*/


                //Perform sales... by creating new orderhdr and orderdetail
                //Create POSController, add items and sell                                
                bool AllowPromo = false;
                status = "";
                pos.SetHeaderRemark("Exchange for Order:" + oHdr.OrderHdrID + ". ItemNo = " + ReturnedItemNo);
                if (!pos.AddItemToOrder(ExchangeItem, ExchangeQty, 0, AllowPromo, out status))
                {
                    Logger.writeLog(status);
                    return false;
                }
                if (!pos.AddItemToOrder(ReturnedItem, -1 * ReturnedQty, 0, AllowPromo, out status))
                {
                    Logger.writeLog(status);
                    return false;
                }
                
                amountPayable = pos.CalculateTotalAmount(out status); 
                status = "";

                log = new ExchangeLog();
                log.CashierID = UserInfo.username;
                log.ExchangeDateTime = DateTime.Now;
                log.ItemNo = ReturnedItem.ItemNo;                
                log.Qty = ReturnedQty;
                log.NewItemNo = ExchangeItem.ItemNo;                
                log.NewQty = ExchangeQty;
                log.OrderHdrID = oHdr.OrderHdrID;
                log.OutletName = PointOfSaleInfo.OutletName;
                log.PointOfSaleID = PointOfSaleInfo.PointOfSaleID;
                log.UndoStockOut = DoStockIn;
                log.UniqueID = Guid.NewGuid();
                log.IsInventoryUpdated = false;
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }

        public string GetOrderHdrID()
        {
            if (oHdr != null)
                return oHdr.OrderHdrID;
            return "";
        }

        public DateTime GetOrderDate()
        {
            if (oHdr != null)
                return oHdr.OrderDate;
            return DateTime.MinValue;
        }

        public POSController GetPOS()
        {
            return pos;
        }
        
        public DataTable GetExchangeDetail()
        {
           
            try
            {
                //create and return a datatable.....
                DataTable dTable = new DataTable();
                DataRow dr;

                dTable.Columns.Add("ItemNo");
                dTable.Columns.Add("ItemName");
                dTable.Columns.Add("itemdesc");
                dTable.Columns.Add("CategoryName");
                dTable.Columns.Add("Quantity");
                dTable.Columns.Add("Disc(%)");
                dTable.Columns.Add("Price");
                dTable.Columns.Add("Amount");
                dTable.Columns.Add("IsVoided");
                dTable.Columns.Add("IsSpecial");
                dTable.Columns.Add("ID");
                dTable.Columns.Add("IsFreeOfCharge");
                dTable.Columns.Add("IsPromo");

                Item myItem;
                decimal qty, unitprice;


                for (int i = oDet.Count - 1; i >= 0; i--)
                {
                    dr = dTable.NewRow();
                    myItem = new Item(oDet[i].ItemNo);
                    dr["ItemNo"] = myItem.ItemNo;
                    dr["ItemName"] = myItem.ItemName;
                    dr["CategoryName"] = myItem.CategoryName;
                    dr["ItemDesc"] = myItem.ItemDesc;
                    qty = oDet[i].Quantity.GetValueOrDefault(0);
                    unitprice = oDet[i].UnitPrice;
                    dr["Quantity"] = qty.ToString("N0");

                    dr["Price"] = unitprice.ToString("N2");

                    dr["IsVoided"] = CommonUILib.GetYesOrNoFromBool(oDet[i].IsVoided);
                    dr["IsSpecial"] = CommonUILib.GetYesOrNoFromBool((bool)oDet[i].IsSpecial);
                    dr["ID"] = oDet[i].OrderDetID;
                    

                    dr["IsPromo"] = CommonUILib.GetYesOrNoFromBool(oDet[i].IsPromo);
                    if (oDet[i].IsPromo)
                    {

                            dr["Amount"] = ((decimal)oDet[i].PromoAmount).ToString("N2");
                            dr["Disc(%)"] = ((decimal)oDet[i].PromoDiscount).ToString("N2");
                       
                

                        dr["IsFreeOfCharge"] = CommonUILib.GetYesOrNoFromBool(oDet[i].IsPromoFreeOfCharge);

                    }
                    else
                    {
                        dr["Amount"] = oDet[i].Amount.ToString("N2");
                        dr["IsFreeOfCharge"] = CommonUILib.GetYesOrNoFromBool(oDet[i].IsFreeOfCharge);
                        dr["Disc(%)"] = oDet[i].Discount.ToString("N2");

                    }

                    dTable.Rows.Add(dr);
                }
                return dTable;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }
        
        public ExchangeController(string OrderHdrRefNo) 
        {            
            try
            {                
                if (OrderHdrRefNo != null && OrderHdrRefNo.ToString() != "")
                {                                        
                    pos = new POSController();
                    //Get the order
                    oHdr = new OrderHdr(OrderHdr.Columns.OrderRefNo, OrderHdrRefNo);                    
                    if (!oHdr.IsNew) 
                        oDet = oHdr.OrderDetRecords();                    

                }                
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);                
            }
        }

        public ArrayList GetItemList()
        {
            ArrayList ar = new ArrayList();            
            for (int i = 0; i < oDet.Count; i++)
            {                
                ar.Add(oDet[i].Item.ItemName);
            }
            ar.Insert(0, "");
            return ar;
        }

        public bool IsLoaded()
        {
            if (oDet == null | oHdr == null)
            {
                return false;
            }
            if (oHdr.IsNew)
                return false;

            return true;
        }

        public decimal GetTotalAmountPayable()
        {
            //throw new Exception("The method or operation is not implemented.");
            return amountPayable;
        }

        public bool SaveExchangeLog()
        {
            log.NewOrderHdrID = pos.GetSavedRefNo().Substring(2);
            log.Save();
            return true;
        }

        public string getMembershipNo()
        {
            return oHdr.MembershipNo;
        }
    }
}
