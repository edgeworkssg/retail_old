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
using System.Linq;
using System.Web.Script.Serialization;

namespace PowerPOS
{
    [System.ComponentModel.DataObject]
    [Serializable]
    public partial class PurchaseOrderController
    {
        #region "PurchaseOrder Movement Type Constant"
        public const string PurchaseOrderMovementType_StockIn = "Stock In";
        public const string PurchaseOrderMovementType_StockOut = "Stock Out";
        public const string PurchaseOrderMovementType_Sales = "Sales";
        public const string PurchaseOrderMovementType_TransferOut = "Transfer Out";
        public const string PurchaseOrderMovementType_TransferIn = "Transfer In";
        public const string PurchaseOrderMovementType_Adjustment = "Adjustment";
        #endregion
        public PurchaseOrderHeader InvHdr; //Save the header information of the Inventory Document
        PurchaseOrderDetailCollection InvDet; //Save the detail information (Item by Item) of the inventory document

        public const string ERR_CLINIC_FROZEN = "This Inventory Location IS frozen. No changes can be made to database.";
        public const string ERR_CLINIC_NOT_FROZEN = "This Inventory Location is NOT frozen. No stock take activity can be made.";
        public const string ERR_CLINIC_DELETED = "This Inventory Location IS deleted.";

        #region "Constructor"
        public PurchaseOrderController()
        {
            //CostingType -> Do not this variable anymore
            //AssignCostingType(CostingType);

            //Create a new inventory objects
            InvHdr = new PurchaseOrderHeader();
            //InvHdr.UniqueID = Guid.NewGuid(); //assign a uniqueid

            //Create the new default values
            InvHdr.PurchaseOrderDate = DateTime.Now; //date of the documents
            InvHdr.UserName = UserInfo.username; //the name of the user doing it
            InvHdr.Remark = ""; //remark
            //InvHdr.FreightCharge = 0; //freight
            //InvHdr.Deleted = false; //deleted
            InvDet = new PurchaseOrderDetailCollection();

        }

        //Load existing saved inventory reference number
        public PurchaseOrderController(string PurchaseOrderHeaderRefNo)
        {
            //AssignCostingType(CostingType);

            InvHdr = new PurchaseOrderHeader(PurchaseOrderHeaderRefNo);

            if (InvHdr.IsLoaded)
            {
                //create new inventory reference no....                                 

                InvHdr.UserName = UserInfo.username;
                InvDet = new PurchaseOrderDetailCollection();
                InvDet.Where(PurchaseOrderDetail.Columns.PurchaseOrderHeaderRefNo, PurchaseOrderHeaderRefNo)
                            .OrderByAsc("CreatedOn")
                            .Load();
            }
            else
            {
                //create new PurchaseOrder reference no.... 
                InvHdr.PurchaseOrderHeaderRefNo = PurchaseOrderHeaderRefNo;
                InvHdr.PurchaseOrderDate = DateTime.Now;
                InvHdr.UserName = UserInfo.username;
                InvHdr.Remark = "";

                InvDet = new PurchaseOrderDetailCollection();
            }

        }
        public bool IsPOExist(string PONO)
        {
            Where whr = new Where();
            whr.TableName = "PurchaseOrderHeaderRefNo";
            whr.ParameterName = "@PurchaseOrderHeaderRefNo";
            whr.ParameterValue = PONO;
            whr.ColumnName = PurchaseOrderHeader.Columns.PurchaseOrderHeaderRefNo;
            return (new Query("PurchaseOrderHeaderRefNo").GetCount(PurchaseOrderHeader.Columns.PurchaseOrderHeaderRefNo, whr) > 0);
        }
        //Create a new PurchaseOrder by copying PurchaseOrder
        public bool CopyItemsFrom(string PurchaseOrderRefNo, out string status)
        {
            status = "";
            try
            {
                PurchaseOrderDetailCollection tmppoDetail = new PurchaseOrderDetailCollection();
                tmppoDetail.Where(PurchaseOrderDetail.Columns.PurchaseOrderHeaderRefNo, PurchaseOrderRefNo).Load();

                if (tmppoDetail != null)
                {
                    tmppoDetail.CopyTo(InvDet);

                    //perform loop to change the PurchaseOrderHeaderRefNo and PurchaseOrderDetailRefNo
                    for (int i = 0; i < InvDet.Count; i++)
                    {
                        InvDet[i].PurchaseOrderHeaderRefNo = InvHdr.PurchaseOrderHeaderRefNo;
                        InvDet[i].PurchaseOrderDetailRefNo = InvHdr.PurchaseOrderHeaderRefNo + "." + i;
                    }

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
                status = ex.Message;
                return false;
            }
        }

        #endregion

        #region "Get ID"

        //Get the maximum Inventory Line ID
        private string GetPurchaseOrderDetailMaxID(out string status)
        {
            status = "";
            try
            {
                if (InvDet != null)
                {
                    if (InvDet.Count == 0) return "1";
                    return (int.Parse(InvDet[InvDet.Count - 1].PurchaseOrderDetailRefNo.Split('.')[1]) + 1).ToString();
                }
                status = "Inventory Line Detail has not been created.";
                return "";
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return "";
            }
        }

        //Get the inventory header ref no
        public string GetPurchaseOrderHeaderRefNo()
        {
            return InvHdr.PurchaseOrderHeaderRefNo;
        }

        public DateTime GetPurchaseOrderDate()
        {
            return InvHdr.PurchaseOrderDate;
        }

        public String GetRemark()
        {
            return InvHdr.Remark;
        }

        public String GetInvoiceNo()
        {
            return InvHdr.ShipVia;
        }

        public PurchaseOrderDetail GetPurchaseOrderDetail(string _PurchaseOrderNo)
        {
            PurchaseOrderDetail temp = new PurchaseOrderDetail();
            foreach (PurchaseOrderDetail tmp1 in InvDet)
            {
                if (tmp1.PurchaseOrderDetailRefNo == _PurchaseOrderNo)
                {
                    temp = tmp1;
                }
            }
            return temp;
        }

        #endregion

        #region "Fetch PurchaseOrder Detail Info"

        public bool LoadConfirmedPurchaseOrderController(string PurchaseOrderHeaderRefNo)
        {
            InvHdr = new PurchaseOrderHeader(PurchaseOrderHeaderRefNo);
            if (!InvHdr.IsNew && InvHdr.IsLoaded)
            {
                Query qry = PurchaseOrderDetail.CreateQuery();

                InvDet = new PurchaseOrderDetailCollection();
                InvDet.Load(qry.WHERE(PurchaseOrderDetail.Columns.PurchaseOrderHeaderRefNo, PurchaseOrderHeaderRefNo).ExecuteDataSet().Tables[0]);

                return true;
            }
            return false;
        }
        public DataTable FetchMergedPurchaseOrderItems(bool displayOnHand, bool displayCostPrice, out string status)
        {
            DataTable dt = FetchUnSavedInventoryItems(displayOnHand, displayCostPrice, out status);

            for (int k = 0; k < dt.Rows.Count; k++)
            {
                if (dt.Rows[k]["Quantity"].ToString() != "0")
                {
                    if (displayCostPrice)
                    {
                        dt.Rows[k]["FactoryPrice"] =
                                    decimal.Parse(dt.Rows[k]["CostOfGoods"].ToString()) *
                                    int.Parse(dt.Rows[k]["Quantity"].ToString());
                    }
                    else
                    {
                        //dt.Rows[k]["FactoryPrice"] = 0.0M;
                    }
                }
            }
            //pull out
            int i = dt.Rows.Count - 1;
            while (i > 0)
            {
                for (int k = 0; k < i; k++)
                {
                    if (dt.Rows[i]["ItemNo"].ToString() == dt.Rows[k]["ItemNo"].ToString())
                    {

                        dt.Rows[k]["Quantity"] =
                            int.Parse(dt.Rows[k]["Quantity"].ToString()) +
                            int.Parse(dt.Rows[i]["Quantity"].ToString());

                        if (displayCostPrice)
                            dt.Rows[k]["FactoryPrice"] =
                                decimal.Parse(dt.Rows[k]["FactoryPrice"].ToString()) + decimal.Parse(dt.Rows[i]["FactoryPrice"].ToString());

                        dt.Rows.RemoveAt(i);

                        break;
                    }
                }
                i--;
            }
            for (int k = 0; k < dt.Rows.Count; k++)
            {
                if (dt.Rows[k]["Quantity"].ToString() != "0")
                {
                    if (displayCostPrice)
                        dt.Rows[k]["FactoryPrice"] =
                                decimal.Parse(dt.Rows[k]["FactoryPrice"].ToString()) / int.Parse(dt.Rows[k]["Quantity"].ToString());
                }
            }
            return dt;
        }

        public DataTable FetchUnSavedInventoryItems(bool displayOnHand, bool displayCostPrice, out string status)
        {
            status = "";
            try
            {
                //create and return a datatable.....
                DataTable dTable = new DataTable();
                DataRow dr;

                dTable.Columns.Add("ItemNo");
                dTable.Columns.Add("ItemName");
                dTable.Columns.Add("Quantity", System.Type.GetType("System.Int32"));
                dTable.Columns.Add("SuggestedQty", System.Type.GetType("System.Int32"));
                dTable.Columns.Add("OnHand", System.Type.GetType("System.Int32"));
                dTable.Columns.Add("Remark");
                dTable.Columns.Add("PurchaseOrderDetailRefNo");
                dTable.Columns.Add("Deleted", System.Type.GetType("System.Boolean"));
                dTable.Columns.Add("RetailPrice", System.Type.GetType("System.Decimal"));
                if (displayCostPrice)
                {
                    dTable.Columns.Add("FactoryPrice", System.Type.GetType("System.Decimal"));
                    dTable.Columns.Add("CostOfGoods", System.Type.GetType("System.Decimal"));
                }
                Item myItem;
                //map OrderDet
                for (int i = InvDet.Count - 1; i > -1; i--)
                {
                    dr = dTable.NewRow();
                    myItem = new Item(InvDet[i].ItemNo);

                    //if (myItem.IsInInventory == true)
                    {
                        dr["ItemNo"] = InvDet[i].ItemNo;
                        dr["ItemName"] = myItem.ItemName;
                        dr["Quantity"] = InvDet[i].Quantity;
                        if (displayCostPrice)
                        {
                            dr["FactoryPrice"] = InvDet[i].FactoryPrice;
                            dr["CostOfGoods"] = InvDet[i].CostOfGoods;

                        }
                        //else
                        //{
                        //    dr["FactoryPrice"] = 0;
                        //    dr["CostOfGoods"] = 0;
                        //}
                        dr["RetailPrice"] = myItem.RetailPrice;
                        //dr["CostOfGoods"] = InvDet[i].CostOfGoods;
                        dr["Remark"] = InvDet[i].Remark;
                        dr["PurchaseOrderDetailRefNo"] = InvDet[i].PurchaseOrderDetailRefNo;
                        if (displayOnHand && InvHdr.InventoryLocationID.HasValue)
                        {
                            dr["OnHand"] =
                                    InventoryController.GetStockBalanceQtyByItemByDate
                                    (InvDet[i].ItemNo, InvHdr.InventoryLocationID.Value,
                                    InvHdr.PurchaseOrderDate,
                                    out status);
                        }
                        else
                        {
                            dr["OnHand"] = 0;
                        }

                        dr["SuggestedQty"] = 0;
                        ItemQuantityTrigger iq = new ItemQuantityTrigger("ItemNo", InvDet[i].ItemNo);
                        if (iq != null && iq.ItemNo != "")
                        {
                            dr["SuggestedQty"] = iq.TriggerQuantity - Convert.ToInt16(dr["OnHand"]);
                        }

                        /*if (InvDet[i].Deleted.HasValue)
                        {
                            dr["Deleted"] = InvDet[i].Deleted.Value;
                        }
                        else
                        {
                            dr["Deleted"] = false;
                        }*/
                        dTable.Rows.Add(dr);
                    }

                }
                return dTable;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return null;
            }
        }

        public DataTable FetchUnSavedPurchaseOrderItems(out string status)
        {
            status = "";
            try
            {
                //create and return a datatable.....
                DataTable dTable = new DataTable();
                DataRow dr;

                dTable.Columns.Add("ItemNo");
                dTable.Columns.Add("ItemName");
                dTable.Columns.Add("Quantity");
                dTable.Columns.Add("Price");
                dTable.Columns.Add("Remark");
                dTable.Columns.Add("PurchaseOrderDetailRefNo");

                Item myItem;
                //map OrderDetail
                for (int i = InvDet.Count - 1; i > -1; i--)
                {
                    dr = dTable.NewRow();
                    myItem = new Item(InvDet[i].ItemNo);

                    if (myItem.IsInInventory == true)
                    {
                        dr["ItemNo"] = InvDet[i].ItemNo;
                        dr["ItemName"] = myItem.ItemName;
                        dr["Quantity"] = InvDet[i].Quantity;
                        dr["Price"] = InvDet[i].FactoryPrice;
                        dr["Remark"] = InvDet[i].Remark;
                        dr["PurchaseOrderDetailRefNo"] = InvDet[i].PurchaseOrderDetailRefNo;
                        dTable.Rows.Add(dr);
                    }
                }
                return dTable;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return null;
            }
        }

        public PurchaseOrderDetailCollection GetPODetail()
        {
            try
            {
                return InvDet;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        #endregion

        #region "Add and Delete Item"

        //Add item into inventory line with Quantity

        public bool AddItemIntoInventory(string ItemID, decimal Qty, decimal COGS, out string status)
        {
            status = "";
            try
            {
                if (!(new Item(ItemID).IsLoaded))
                {
                    status = "Item with Item No " + ItemID + " does not exist in the system";
                    return false;
                }
                //Add into InventoryDet -- Check if already existance and non promo            
                if (InvDet != null)
                {
                    PurchaseOrderDetail tmp;
                    tmp = new PurchaseOrderDetail();
                    tmp.CostOfGoods = COGS;
                    tmp.Discount = 0;
                    tmp.Quantity = Qty;
                    tmp.PurchaseOrderHeaderRefNo = InvHdr.PurchaseOrderHeaderRefNo;
                    tmp.PurchaseOrderDetailRefNo = tmp.PurchaseOrderHeaderRefNo + "." + GetPurchaseOrderDetailMaxID(out status);

                    if (status != "")
                    {
                        return false;
                    }
                    tmp.ItemNo = ItemID;
                    //tmp.FactoryPrice = tmp.Item.FactoryPrice;
                    if (InvHdr.CurrencyID.HasValue)
                    {
                        tmp.FactoryPrice = ItemController.FetchCostPrice(InvHdr.CurrencyID.Value, ItemID);
                    }
                    else
                    {
                        tmp.FactoryPrice = tmp.Item.FactoryPrice;
                    }
                    tmp.Deleted = true;
                    //tmp.UniqueID = Guid.NewGuid();
                    InvDet.Add(tmp);
                    return true;
                }
                else
                {
                    status = "Inventory Detail has not been initialized.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }

        }

        //Add new item into PurchaseOrder - set the quantity to be 1
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public bool AddItemIntoPurchaseOrder(string ItemID, out string status)
        {
            status = "";
            try
            {
                if (!new Item(ItemID).IsLoaded)
                {
                    status = "Item with ItemNo:" + ItemID + " does not exist";
                    return false;
                }
                if (InvDet != null)
                {
                    PurchaseOrderDetail tmp;
                    tmp = new PurchaseOrderDetail();

                    tmp.Discount = 0;
                    tmp.Quantity = 1;
                    tmp.PurchaseOrderHeaderRefNo = InvHdr.PurchaseOrderHeaderRefNo;
                    tmp.PurchaseOrderDetailRefNo = tmp.PurchaseOrderHeaderRefNo + "." + GetPurchaseOrderDetailMaxID(out status);
                    if (status != "")
                    {
                        return false;
                    }
                    tmp.ItemNo = ItemID;
                    tmp.FactoryPrice = tmp.Item.FactoryPrice;
                    InvDet.Add(tmp);
                    InvHdr.Save();
                    InvDet.SaveAll();
                    return true;
                }
                else
                {
                    status = "PurchaseOrder Detail has not been created.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }

        }

        //Add item into inventory line with Quantity
        public bool AddItemIntoInventory(string ItemID, decimal Qty, out string status)
        {
            status = "";
            try
            {
                Item tmpItem = new Item(ItemID);
                if (!tmpItem.IsLoaded)
                {
                    status = "Item with Item No " + ItemID + " does not exist in the system";
                    return false;
                }
                if (!tmpItem.IsInInventory)
                {
                    status = "Item with Item No " + ItemID + " is not an inventory item";
                    return false;
                }
                //Add into InventoryDet -- Check if already existance and non promo            
                if (InvDet != null)
                {
                    int existinvdet = InvDet.Find("ItemNo", tmpItem.ItemNo);
                    if (existinvdet != -1)
                    {
                        InvDet[existinvdet].Quantity = InvDet[existinvdet].Quantity + Qty;

                    }
                    else
                    {
                        PurchaseOrderDetail tmp;
                        tmp = new PurchaseOrderDetail();
                        tmp.CostOfGoods = 0;
                        tmp.Discount = 0;
                        tmp.Quantity = Qty;
                        tmp.PurchaseOrderHeaderRefNo = InvHdr.PurchaseOrderHeaderRefNo;
                        tmp.PurchaseOrderDetailRefNo = tmp.PurchaseOrderHeaderRefNo + "." + GetPurchaseOrderDetailMaxID(out status);
                        if (status != "")
                        {
                            return false;
                        }
                        tmp.ItemNo = ItemID;
                        string currency = "";
                        if (InvHdr.CurrencyID.HasValue)
                        {
                            tmp.FactoryPrice = ItemController.FetchCostPrice(InvHdr.CurrencyID.Value, ItemID);
                        }
                        else
                        {
                            tmp.FactoryPrice = tmp.Item.FactoryPrice;
                        }
                        tmp.Deleted = false;
                        //tmp.UniqueID = Guid.NewGuid();
                        InvDet.Add(tmp);
                    }
                    status = "";
                    return true;
                }
                else
                {
                    status = "Inventory Detail has not been initialized.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }

        }

        //Add item into inventory line with Quantity
        public bool AddItemIntoInventorySupplier(string ItemID, int Qty, string SupplierName, out string status)
        {
            status = "";
            try
            {
                Item tmpItem = new Item(ItemID);
                if (!tmpItem.IsLoaded)
                {
                    status = "Item with Item No " + ItemID + " does not exist in the system";
                    return false;
                }
                if (!tmpItem.IsInInventory)
                {
                    status = "Item with Item No " + ItemID + " is not an inventory item";
                    return false;
                }

                Supplier supp = new Supplier("SupplierName", SupplierName);
                if (!supp.IsLoaded)
                {
                    status = "Wrong Supplier Code";
                    return false;
                }

                decimal costprice = tmpItem.FactoryPrice;

                ItemSupplierMapCollection coll = new ItemSupplierMapCollection();
                coll.Where(ItemSupplierMap.Columns.ItemNo, SubSonic.Comparison.Like, ItemID);
                coll.Where(ItemSupplierMap.Columns.SupplierID, supp.SupplierID);
                coll.Load();

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.SupplierIsCompulsoryPO), false))
                {
                    if (coll.Count < 1)
                    {
                        status = "Cannot Load Item Supplier Map";
                        return false;
                    }
                    else
                    {
                        costprice = coll[0].CostPrice;
                    }
                }


                //Add into InventoryDet -- Check if already existance and non promo            
                if (InvDet != null)
                {
                    int existinvdet = InvDet.Find("ItemNo", tmpItem.ItemNo);
                    if (existinvdet != -1)
                    {
                        InvDet[existinvdet].Quantity = InvDet[existinvdet].Quantity + Qty;

                    }
                    else
                    {
                        PurchaseOrderDetail tmp;
                        tmp = new PurchaseOrderDetail();
                        tmp.CostOfGoods = 0;
                        tmp.Discount = 0;
                        tmp.Quantity = Qty;
                        tmp.PurchaseOrderHeaderRefNo = InvHdr.PurchaseOrderHeaderRefNo;
                        tmp.PurchaseOrderDetailRefNo = tmp.PurchaseOrderHeaderRefNo + "." + GetPurchaseOrderDetailMaxID(out status);
                        if (status != "")
                        {
                            return false;
                        }
                        tmp.ItemNo = ItemID;
                        string currency = "";
                        if (InvHdr.CurrencyID.HasValue)
                        {
                            tmp.FactoryPrice = costprice;
                        }
                        else
                        {
                            tmp.FactoryPrice = costprice;
                        }
                        tmp.Deleted = true;
                        //tmp.UniqueID = Guid.NewGuid();
                        InvDet.Add(tmp);
                    }
                    status = "";
                    return true;
                }
                else
                {
                    status = "Inventory Detail has not been initialized.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }

        }

        //Add item into inventory line with Quantity
        public bool AddItemIntoInventory1(string ItemID, int Qty, decimal CPrice, out string status)
        {
            status = "";
            try
            {
                Item tmpItem = new Item(ItemID);
                if (!tmpItem.IsLoaded)
                {
                    status = "Item with Item No " + ItemID + " does not exist in the system";
                    return false;
                }
                if (!tmpItem.IsInInventory)
                {
                    status = "Item with Item No " + ItemID + " is not an inventory item";
                    return false;
                }
                //Add into InventoryDet -- Check if already existance and non promo            
                if (InvDet != null)
                {
                    int existinvdet = InvDet.Find("ItemNo", tmpItem.ItemNo);
                    if (existinvdet != -1)
                    {
                        InvDet[existinvdet].Quantity = InvDet[existinvdet].Quantity + Qty;

                    }
                    else
                    {
                        PurchaseOrderDetail tmp;
                        tmp = new PurchaseOrderDetail();
                        tmp.CostOfGoods = 0;
                        tmp.Discount = 0;
                        tmp.Quantity = Qty;
                        tmp.PurchaseOrderHeaderRefNo = InvHdr.PurchaseOrderHeaderRefNo;
                        tmp.PurchaseOrderDetailRefNo = tmp.PurchaseOrderHeaderRefNo + "." + GetPurchaseOrderDetailMaxID(out status);
                        if (status != "")
                        {
                            return false;
                        }
                        tmp.ItemNo = ItemID;
                        string currency = "";
                        if (InvHdr.CurrencyID.HasValue)
                        {
                            tmp.FactoryPrice = CPrice;
                        }
                        else
                        {
                            tmp.FactoryPrice = CPrice;
                        }
                        tmp.Deleted = true;
                        //tmp.UniqueID = Guid.NewGuid();
                        InvDet.Add(tmp);
                    }
                    status = "";
                    return true;
                }
                else
                {
                    status = "Inventory Detail has not been initialized.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }

        }

        /// <summary>
        /// Add item for Delivery Stock Out purpose. Will keep reference to OrderDet
        /// </summary>
        /// <param name="ItemID"></param>
        /// <param name="Qty"></param>
        /// <param name="OrderDetID"></param>
        /*public void AddItemIntoInventoryForDelivery(string ItemID, int Qty, string OrderDetID)
        {
            string status = "";
            try
            {
                if (InvDet == null)
                    throw new Exception("(error)Inventory Detail has not been initialized.");

                Item tmpItem = new Item(ItemID);
                if (!tmpItem.IsLoaded)
                    throw new Exception("(warning)Item with Item No " + ItemID + " does not exist in the system");

                if (!tmpItem.IsInInventory)
                    throw new Exception("(warning)Item with Item No " + ItemID + " is not an inventory item");

                //Add into InventoryDet -- Check if already existance and non promo            
                InventoryDet tmp;
                tmp = new InventoryDet();
                tmp.CostOfGoods = 0;
                tmp.Discount = 0;
                tmp.Quantity = Qty;
                tmp.InventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                tmp.InventoryDetRefNo = tmp.InventoryHdrRefNo + "." + GetInvDetMaxID(out status);
                tmp.DORefNo = OrderDetID;

                if (status != "") throw new Exception(status);

                tmp.ItemNo = ItemID;
                string currency = "";
                if (InvHdr.CurrencyID.HasValue)
                {
                    tmp.FactoryPrice = ItemController.FetchCostPrice(InvHdr.CurrencyID.Value, ItemID);
                }
                else
                {
                    tmp.FactoryPrice = tmp.Item.FactoryPrice;
                }
                tmp.UniqueID = Guid.NewGuid();
                InvDet.Add(tmp);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                throw ex;
            }

        }*/

        /*
        //Add new item into inventory by scanning barcode
        public bool AddItemIntoInventoryByBarcode(string Barcode, out string status)
        {
            status = "";
            try
            {
                if (!(new Item(Item.Columns.Barcode, Barcode).IsLoaded))
                {
                    status = "Item with Barcode " + Barcode + " does not exist in the system";
                    return false;
                }

                //Add into InventoryDet -- Check if already existance and non promo            
                if (InvDet != null)
                {

                    IDataReader dr = Item.FetchByParameter(Item.Columns.Barcode, Barcode);
                    if (dr.Read())
                    {
                        InventoryDet tmp;
                        tmp = new InventoryDet();
                        tmp.CostOfGoods = 0;
                        tmp.ItemNo = dr.GetValue(0).ToString();
                        tmp.Discount = 0;
                        tmp.Quantity = 1;
                        tmp.InventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                        tmp.InventoryDetRefNo = tmp.InventoryHdrRefNo + "." + GetInvDetMaxID(out status);
                        if (status != "")
                            return false;
                        //tmp.FactoryPrice = tmp.Item.FactoryPrice;
                        if (InvHdr.CurrencyID.HasValue)
                        {
                            tmp.FactoryPrice = ItemController.FetchCostPrice(InvHdr.CurrencyID.Value, tmp.ItemNo);
                        }
                        else
                        {
                            tmp.FactoryPrice = tmp.Item.FactoryPrice;
                        }
                        tmp.UniqueID = Guid.NewGuid();
                        InvDet.Add(tmp);
                        return true;
                    }
                    return false;
                }
                else
                {
                    status = "Inventory detail has not been initialized ";
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }

        }
        */
        /*
        //Add new item into inventory by scanning barcode
        public bool AddItemIntoInventoryByBarcode(string Barcode, int Qty, out string status)
        {
            status = "";
            try
            {
                if (!(new Item(Item.Columns.Barcode, Barcode).IsLoaded))
                {
                    status = "Item with Barcode " + Barcode + " does not exist in the system";
                    return false;
                }

                //Add into InventoryDet -- Check if already existance and non promo            
                if (InvDet != null)
                {

                    IDataReader dr = Item.FetchByParameter(Item.Columns.Barcode, Barcode);
                    if (dr.Read())
                    {
                        InventoryDet tmp;
                        tmp = new InventoryDet();
                        tmp.CostOfGoods = 0;
                        tmp.ItemNo = dr.GetValue(0).ToString();
                        tmp.Discount = 0;
                        tmp.Quantity = Qty;
                        tmp.InventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                        tmp.InventoryDetRefNo = tmp.InventoryHdrRefNo + "." + GetInvDetMaxID(out status);
                        if (status != "")
                            return false;
                        //tmp.FactoryPrice = tmp.Item.FactoryPrice;
                        if (InvHdr.CurrencyID.HasValue)
                        {
                            tmp.FactoryPrice = ItemController.FetchCostPrice(InvHdr.CurrencyID.Value, tmp.ItemNo);
                        }
                        else
                        {
                            tmp.FactoryPrice = tmp.Item.FactoryPrice;
                        }

                        tmp.UniqueID = Guid.NewGuid();
                        InvDet.Add(tmp);
                        return true;
                    }
                    return false;
                }
                else
                {
                    status = "Inventory detail has not been initialized ";
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }

        }
        */

        //Add item into PurchaseOrder line with Quantity
        public bool AddItemIntoPurchaseOrder(string ItemID, decimal Qty, string ExpiryDate, out string status, out PurchaseOrderDetail AddedItem)
        {
            status = "";
            AddedItem = new PurchaseOrderDetail();

            try
            {
                if (!new Item(ItemID).IsLoaded)
                {
                    status = "Item with ItemNo:" + ItemID + " does not exist";
                    return false;
                }
                //Add into PurchaseOrderDetailail -- Check if already existance and non promo            
                if (InvDet != null)
                {
                    PurchaseOrderDetail tmp;
                    tmp = new PurchaseOrderDetail();

                    tmp.Discount = 0;
                    tmp.Quantity = Qty;
                    tmp.PurchaseOrderHeaderRefNo = InvHdr.PurchaseOrderHeaderRefNo;
                    tmp.PurchaseOrderDetailRefNo = tmp.PurchaseOrderHeaderRefNo + "." + GetPurchaseOrderDetailMaxID(out status);
                    if (status != "")
                    {
                        return false;
                    }
                    tmp.ItemNo = ItemID;
                    tmp.FactoryPrice = tmp.Item.FactoryPrice;
                    tmp.ExpiryDate = Convert.ToDateTime(ExpiryDate);
                    tmp.Userfld6 = "";
                    tmp.SalesQty = 0;
                    InvDet.Add(tmp);
                    InvHdr.Save();
                    InvDet.SaveAll();

                    AddedItem = tmp;
                    return true;
                }
                else
                {
                    status = "PurchaseOrder Detail has not been initialized.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }

        }

        //Add item into PurchaseOrder line with Quantity
        public bool AddItemIntoPurchaseOrder(string ItemID, decimal Qty, string ExpiryDate, string orderrefno, out string status, out PurchaseOrderDetail AddedItem)
        {
            status = "";
            AddedItem = new PurchaseOrderDetail();

            try
            {
                if (!new Item(ItemID).IsLoaded)
                {
                    status = "Item with ItemNo:" + ItemID + " does not exist";
                    return false;
                }
                //Add into PurchaseOrderDetailail -- Check if already existance and non promo            
                if (InvDet != null)
                {
                    //if exist 
                    bool isExist = false;
                    foreach (PurchaseOrderDetail pod in InvDet)
                    {
                        if (pod.ItemNo == ItemID)
                        {
                            pod.Quantity += Qty;
                            pod.SalesQty += Qty;
                            AddedItem = pod;
                            isExist = true;

                            OrderDet od = new OrderDet(orderrefno);

                            if (od != null && !od.IsNew)
                            {
                                pod.Amount += od.Amount;
                                pod.GSTAmount += od.GSTAmount;                               
                            }
                            else
                            {
                                PurchaseOrderDetail old = new PurchaseOrderDetail(orderrefno);
                                if (old != null)
                                {
                                    pod.Amount += old.Amount;
                                    pod.GSTAmount += old.GSTAmount;                                   
                                }
                            }

                        }
                    }

                    PurchaseOrderDetail tmp = new PurchaseOrderDetail();
                    if (!isExist)
                    {
                        OrderDet od = new OrderDet(orderrefno);

                        tmp = new PurchaseOrderDetail();

                        tmp.Discount = 0;
                        tmp.Quantity = Qty;
                        tmp.PurchaseOrderHeaderRefNo = InvHdr.PurchaseOrderHeaderRefNo;
                        tmp.PurchaseOrderDetailRefNo = tmp.PurchaseOrderHeaderRefNo + "." + GetPurchaseOrderDetailMaxID(out status);
                        if (status != "")
                        {
                            return false;
                        }
                        tmp.ItemNo = ItemID;
                        tmp.FactoryPrice = tmp.Item.FactoryPrice;
                        tmp.ExpiryDate = Convert.ToDateTime(ExpiryDate);
                        tmp.Userfld6 = orderrefno;
                        tmp.SalesQty = Qty;

                        if (od != null && !od.IsNew)
                        {
                            tmp.Amount = od.Amount;
                            tmp.GSTAmount = od.GSTAmount;
                            tmp.DiscountDetail = od.DiscountDetail;
                        }
                        else 
                        {
                            PurchaseOrderDetail old = new PurchaseOrderDetail(orderrefno);
                            if (old != null)
                            {
                                tmp.Amount = old.Amount / (old.SalesQty <= 0 ? 1 : old.SalesQty) * tmp.Quantity;
                                tmp.GSTAmount = old.GSTAmount / (old.SalesQty <= 0 ? 1 : old.SalesQty) * tmp.Quantity;
                                tmp.DiscountDetail = od.DiscountDetail;
                            }
                        }

                        InvDet.Add(tmp);
                        AddedItem = tmp;
                    }
                    InvHdr.Save();
                    InvDet.SaveAll();

                    return true;
                }
                else
                {
                    status = "PurchaseOrder Detail has not been initialized.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }

        }

        //Add item into PurchaseOrder line with Quantity
        public bool AddItemIntoPurchaseOrderWithRejectQty(string ItemID, decimal Qty, decimal RejectQty, string ExpiryDate, out string status, out PurchaseOrderDetail AddedItem)
        {
            status = "";
            AddedItem = new PurchaseOrderDetail();

            try
            {
                if (!new Item(ItemID).IsLoaded)
                {
                    status = "Item with ItemNo:" + ItemID + " does not exist";
                    return false;
                }
                //Add into PurchaseOrderDetailail -- Check if already existance and non promo            
                if (InvDet != null)
                {
                    int index = InvDet.Find(PurchaseOrderDetail.Columns.ItemNo, ItemID);
                    if (index >= 0)
                    {
                        PurchaseOrderDetail tmp = InvDet[index];
                        tmp.OriginalQuantity += Qty;
                        tmp.RejectQty = RejectQty;
                        tmp.Quantity = tmp.OriginalQuantity - tmp.RejectQty;
                        InvDet.SaveAll();

                        AddedItem = tmp;
                        return true;
                    }
                    else
                    {
                        PurchaseOrderDetail tmp;
                        tmp = new PurchaseOrderDetail();

                        tmp.Discount = 0;
                        tmp.OriginalQuantity = Qty;
                        tmp.RejectQty = RejectQty;
                        tmp.Quantity = Qty - RejectQty;
                        tmp.PurchaseOrderHeaderRefNo = InvHdr.PurchaseOrderHeaderRefNo;
                        tmp.PurchaseOrderDetailRefNo = tmp.PurchaseOrderHeaderRefNo + "." + GetPurchaseOrderDetailMaxID(out status);
                        if (status != "")
                        {
                            return false;
                        }
                        tmp.ItemNo = ItemID;
                        tmp.FactoryPrice = tmp.Item.FactoryPrice;
                        tmp.ExpiryDate = string.IsNullOrEmpty(ExpiryDate) ? (DateTime?)null : Convert.ToDateTime(ExpiryDate);
                        tmp.Userfld6 = "";
                        tmp.SalesQty = 0;
                        InvDet.Add(tmp);
                        InvHdr.Save();
                        InvDet.SaveAll();

                        AddedItem = tmp;
                        return true;
                    }
                }
                else
                {
                    status = "PurchaseOrder Detail has not been initialized.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }

        }

        //Add item into PurchaseOrder line with Quantity
        public bool AddItemIntoPurchaseOrderWithPriceLevel(string ItemID, decimal Qty, string ExpiryDate, string orderrefno, string priceLevel, out string status, out PurchaseOrderDetail AddedItem)
        {
            status = "";
            AddedItem = new PurchaseOrderDetail();

            try
            {
                if (!new Item(ItemID).IsLoaded)
                {
                    status = "Item with ItemNo:" + ItemID + " does not exist";
                    return false;
                }
                //Add into PurchaseOrderDetailail -- Check if already existance and non promo            
                if (InvDet != null)
                {
                    //if exist 
                    bool isExist = false;
                    foreach (PurchaseOrderDetail pod in InvDet)
                    {
                        if (pod.ItemNo == ItemID)
                        {
                            pod.Quantity += Qty;
                            pod.SalesQty += Qty;
                            AddedItem = pod;
                            isExist = true;

                            OrderDet od = new OrderDet(orderrefno);

                            if (od != null && !od.IsNew)
                            {
                                pod.Amount += od.Amount;
                                pod.GSTAmount += od.GSTAmount;
                            }
                            else
                            {
                                PurchaseOrderDetail old = new PurchaseOrderDetail(orderrefno);
                                if (old != null)
                                {
                                    pod.Amount += old.Amount;
                                    pod.GSTAmount += old.GSTAmount;
                                }
                            }

                        }
                    }

                    PurchaseOrderDetail tmp = new PurchaseOrderDetail();
                    if (!isExist)
                    {
                        OrderDet od = new OrderDet(orderrefno);

                        tmp = new PurchaseOrderDetail();

                        tmp.Discount = 0;
                        tmp.Quantity = Qty;
                        tmp.PurchaseOrderHeaderRefNo = InvHdr.PurchaseOrderHeaderRefNo;
                        tmp.PurchaseOrderDetailRefNo = tmp.PurchaseOrderHeaderRefNo + "." + GetPurchaseOrderDetailMaxID(out status);
                        if (status != "")
                        {
                            return false;
                        }
                        tmp.ItemNo = ItemID;
                        Item itm = new Item(tmp.ItemNo);

                        if (!string.IsNullOrEmpty(priceLevel))
                        {
                            if (priceLevel == "P1")
                                tmp.FactoryPrice = itm.P1Price;
                            else if (priceLevel == "P2")
                                tmp.FactoryPrice = itm.P2Price;
                            else if (priceLevel == "P3")
                                tmp.FactoryPrice = itm.P3Price;
                            else if (priceLevel == "P4")
                                tmp.FactoryPrice = itm.P4Price;
                            else if (priceLevel == "P5")
                                tmp.FactoryPrice = itm.P5Price;
                        }
                        else
                            tmp.FactoryPrice = tmp.Item.FactoryPrice;

                        tmp.ExpiryDate = Convert.ToDateTime(ExpiryDate);
                        tmp.Userfld6 = orderrefno;
                        tmp.SalesQty = Qty;

                        if (od != null && !od.IsNew)
                        {
                            tmp.Amount = od.Amount;
                            tmp.GSTAmount = od.GSTAmount;
                            tmp.DiscountDetail = od.DiscountDetail;
                        }
                        else
                        {
                            PurchaseOrderDetail old = new PurchaseOrderDetail(orderrefno);
                            if (old != null)
                            {
                                tmp.Amount = old.Amount / (old.SalesQty <= 0 ? 1 : old.SalesQty) * tmp.Quantity;
                                tmp.GSTAmount = old.GSTAmount / (old.SalesQty <= 0 ? 1 : old.SalesQty) * tmp.Quantity;
                                tmp.DiscountDetail = od.DiscountDetail;
                            }
                        }

                        InvDet.Add(tmp);
                        AddedItem = tmp;
                    }
                    InvHdr.Save();
                    InvDet.SaveAll();

                    return true;
                }
                else
                {
                    status = "PurchaseOrder Detail has not been initialized.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }

        }

        public static bool IsAvailableToOrder(string PurchaseOrderHeaderRefNo)
        {
            try
            {
                bool result = false;
                PurchaseOrderDetailCollection poDetColl = new PurchaseOrderDetailCollection();
                poDetColl.Where(PurchaseOrderDetail.Columns.PurchaseOrderHeaderRefNo, PurchaseOrderHeaderRefNo);
                poDetColl.Load();
                foreach (PurchaseOrderDetail poDet in poDetColl)
                {
                    string status = "";
                    decimal whBal = GetWarehouseBalance(poDet.ItemNo, DateTime.Now, out status);
                    if (whBal > 0)
                        result = true;
                }
                return result;
            }
            catch (Exception ex) { Logger.writeLog(ex.Message); return false; }
        }


        //Delete an item from inventory detail
        public bool DeleteFromPurchaseOrderDetail(string ID, out string status)
        {
            status = "";
            try
            {
                InvDet.Remove((PowerPOS.PurchaseOrderDetail)InvDet.Find(ID));
                PurchaseOrderDetail.Destroy(ID);
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }

        #endregion

        #region "Change Item Attributes"

        //Set Quantity
        public bool ChangeItemQty(string ID, decimal newQty, out string status)
        {

            try
            {
                status = "";
                PurchaseOrderDetail myTmpDet;

                myTmpDet = (PowerPOS.PurchaseOrderDetail)InvDet.Find(ID);

                if (myTmpDet == null)
                {
                    status = "Inventory detail has not been created";
                    return false;
                }

                myTmpDet.Quantity = newQty;
                InvHdr.Save();
                InvDet.SaveAll();
                return true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return false;
            }
        }

        public bool ChangeItemQtyWithRejectQty(string ID, decimal newQty, decimal newRejectQty, decimal newOriginalQty, out string status)
        {

            try
            {
                status = "";
                PurchaseOrderDetail myTmpDet;

                myTmpDet = (PowerPOS.PurchaseOrderDetail)InvDet.Find(ID);

                if (myTmpDet == null)
                {
                    status = "Inventory detail has not been created";
                    return false;
                }

                myTmpDet.Quantity = newQty;
                myTmpDet.RejectQty = newRejectQty;
                myTmpDet.OriginalQuantity = newOriginalQty;

                InvHdr.Save();
                InvDet.SaveAll();
                return true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return false;
            }
        }

        private bool checkSerialNoIsExist(Item _item, int inventoryLocationID, string serialNo, out string _message)
        {
            _message = "";
            List<string> _serialNo = serialNo.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            ItemTagModel input = new ItemTagModel();
            input.ItemNo = _item.ItemNo;
            input.InventoryLocationID = inventoryLocationID;
            input.SerialNoColl = _serialNo;
            var inputColl = new List<ItemTagModel>();
            inputColl.Add(input);
            bool isValid = false;
             
            isValid = ItemTagController.CheckSerialNoIsExistHelper(inputColl, out _message);

            return isValid;
        }

        public bool ChangeSerialNo(string ID, string newSerialNo, out string status)
        {

            try
            {
                status = "";
                PurchaseOrderDetail myTmpDet;

                myTmpDet = (PowerPOS.PurchaseOrderDetail)InvDet.Find(ID);

                if (myTmpDet == null)
                {
                    status = "Inventory detail has not been created";
                    return false;
                }
                string message = "";
                int invLocID = 0;
                if (!(InvHdr.Supplier.WarehouseID == null))
                {
                    invLocID = InvHdr.Supplier.WarehouseID.GetValueOrDefault(0);
                }

                if (!checkSerialNoIsExist(myTmpDet.Item, invLocID, newSerialNo, out message))
                {
                    status = message;
                    return false;
                }

                myTmpDet.SerialNo = newSerialNo;
                

                InvHdr.Save();
                InvDet.SaveAll();
                return true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return false;
            }
        }

        private Hashtable RollUpItemQty, LastIndex;

        /*
        public bool ChangeQuantityByItemNo(string ItemNo, int newQty, out string status)
        {
            status = "";
            if (RollUpItemQty == null)
            {
                RollUpItemQty = new Hashtable();
            }
            if (LastIndex == null)
            {
                LastIndex = new Hashtable();
            }

            //Find ItemNo in HashTable
            if (RollUpItemQty.ContainsKey(ItemNo))
            {
                RollUpItemQty[ItemNo] = newQty;
            }
            else
            {
                RollUpItemQty.Add(ItemNo, newQty);
            }
            return true;
        }
        */
        /*
        //Set Quantity
        public bool DistributeNewItemNo()
        {
            try
            {
                int NewQty;
                for (int i = 0; i < InvDet.Count; i++)
                {
                    NewQty = int.Parse(RollUpItemQty[InvDet[i].ItemNo].ToString());
                    if (InvDet[i].Quantity <= NewQty)
                    {
                        NewQty -= InvDet[i].Quantity;
                        RollUpItemQty[InvDet[i].ItemNo] = NewQty.ToString();

                        //Extra Quantity ignore it first.... 
                        //There maybe other rows bellow
                        //later scan the roll up hastable for itemqty > 0 and add inventory
                    }
                    else
                    {
                        //ItemQty > New Quantity
                        InvDet[i].Quantity = NewQty;
                        RollUpItemQty[InvDet[i].ItemNo] = 0;
                    }
                }

                foreach (DictionaryEntry de in RollUpItemQty)
                {
                    NewQty = int.Parse(de.Value.ToString());
                    if (NewQty > 0)
                    {
                        for (int i = InvDet.Count - 1; i >= 0; i--)
                        {
                            if (InvDet[i].ItemNo == de.Key.ToString())
                            {
                                InvDet[i].Quantity += NewQty;
                                break;
                                //Add the extra Qty to the last line
                                //It will be marked as discrepancy when doing transer receove
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }
        */

        //Set Factory Price
        public bool ChangeFactoryPrice(string ID, decimal newFactoryPrice, out string status)
        {
            status = "";
            try
            {
                PurchaseOrderDetail myTmpDet;

                myTmpDet = (PowerPOS.PurchaseOrderDetail)InvDet.Find(ID);

                if (myTmpDet == null)
                    return false;

                myTmpDet.FactoryPrice = newFactoryPrice;

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }


        public bool ChangeItemRemark(string ID, string Remark, out string status)
        {
            status = "";
            try
            {
                PurchaseOrderDetail myTmpDet;

                myTmpDet = (PowerPOS.PurchaseOrderDetail)InvDet.Find(ID);
                myTmpDet.Remark = Remark;

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }

        //Set Remark
        public bool ChangeRemarkByItemNo(string ItemNo, string remark, out string status)
        {
            status = "";
            try
            {
                for (int i = 0; i < InvDet.Count; i++)
                {
                    if (InvDet[i].ItemNo == ItemNo)
                    {
                        InvDet[i].Remark = remark;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }

        #endregion

        #region "Set Inventory Header Information"

        public bool SetPurchaseOrderHeaderInfo(string Supplier,
            string Remark, decimal freightCharges, double ExchangeRate, decimal Discount)
        {

            //InvHdr.PurchaseOrderNo = PurchaseOrderNo;
            InvHdr.SupplierID = Convert.ToInt16(Supplier);
            InvHdr.Remark = Remark;
            //InvHdr.FreightCharge = freightCharges;
            //InvHdr.ExchangeRate = ExchangeRate;
            InvHdr.Discount = Discount;
            return true;
        }

        public bool SetPurchaseOrderHeaderInfo(string SupplierID,
            string Remark, decimal freightCharges, double ExchangeRate, decimal Discount, string member, string project)
        {

            //InvHdr.PurchaseOrderNo = PurchaseOrderNo;
            InvHdr.Supplier = new Supplier(SupplierID);
            InvHdr.Remark = Remark;
            //InvHdr.FreightCharge = freightCharges;
            //InvHdr.ExchangeRate = ExchangeRate;
            InvHdr.Discount = Discount;
            InvHdr.Userfld1 = member;
            InvHdr.Userfld2 = project;
            return true;
        }
        public bool AssignMembership(string member, out string status)
        {
            status = "";
            try
            {
                InvHdr.Userfld1 = member;

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }

        public bool SetPurchaseOrder(string PurchaseOrderNo)
        {
            InvHdr.PurchaseOrderHeaderRefNo = PurchaseOrderNo;

            return true;
        }

        public bool SetPurchaseOrderDate(DateTime InventoryDate)
        {
            InvHdr.PurchaseOrderDate = InventoryDate;

            return true;
        }

        public bool SetRemark(String Remark)
        {
            InvHdr.Remark = Remark;

            return true;
        }

        public bool SetSupplier(String Remark)
        {
            InvHdr.Userfld4 = Remark;
            return true;
        }

        public bool SetFreightCharges(decimal FreightCharges)
        {
            InvHdr.Userfloat1 = FreightCharges;

            return true;
        }

        public bool SetDiscount(decimal Discount)
        {
            InvHdr.Discount = Discount;

            return true;
        }

        public bool SetExchangeRate(double ExchangeRate)
        {
            InvHdr.Userfloat2 = (decimal)ExchangeRate;

            return true;
        }

        public Guid getUniqueID()
        {
            return new Guid();
        }

        public bool SetDateLoadFromSales(DateTime StartFrom, DateTime StartTo)
        {
            InvHdr.DateFrom = StartFrom;
            InvHdr.DateTo = StartTo;

            InvHdr.Save();
            return true;
        }

        public bool SetSalesPersonID(string username)
        {
            InvHdr.SalesPersonID = username;
            InvHdr.Save();
            return true;
        }


        #endregion
        /*
        public int GetRollUpQuantityByItem(string ItemNo)
        {
            int totalQty = 0;
            for (int i = 0; i < InvDet.Count; i++)
            {
                if (InvDet[i].ItemNo == ItemNo)
                {
                    totalQty += InvDet[i].Quantity;
                }
            }
            return totalQty;
        }
        */
        public int GetNumberOfLineItem()
        {
            return InvDet.Count;
        }

        /*public bool CreateStockTakeEntries(string username, string takenBy, string verifiedBy, out string status)
        {
            try
            {
                PurchaseOrderDetCollection mergedTmpDet = new InventoryDetCollection();
                MergePurchaseOrderDet(ref mergedTmpDet);
                InvDet = mergedTmpDet;
                QueryCommandCollection cmd = new QueryCommandCollection();
                //loop through inventory det
                for (int i = 0; i < InvDet.Count; i++)
                {

                    //check if stocktake entry exist
                    string SQL = "select top 1 stocktakeid from stocktake " +
                                "where itemno = '" + InvDet[i].ItemNo + "' and inventorylocationid =" + InvHdr.InventoryLocationID.Value + " and isadjusted=0 " +
                                "order by stocktakedate desc";
                    QueryCommand cmdTmp = new QueryCommand(SQL, "PowerPOS");
                    object tmpObj = DataService.ExecuteScalar(cmdTmp);
                    if (tmpObj != null && tmpObj is int)
                    {
                        int BalQtyAtEntry = InventoryController.GetStockBalanceQtyByItemByDate(InvDet[i].ItemNo, InvHdr.InventoryLocationID.Value, InvHdr.InventoryDate, out status);
                        int AdjustmentQty = InvDet[i].Quantity - BalQtyAtEntry;
                        SQL =
                            "UPDATE StockTake " +
                            "SET StockTakeQty = " + InvDet[i].Quantity + " " +
                                ", BalQtyAtEntry = " + BalQtyAtEntry + " " +
                                ", AdjustmentQty = " + AdjustmentQty + " " +
                                ", takenby = '" + takenBy + "' " +
                                ", verifiedBy = '" + verifiedBy + "' " +
                                ", stocktakedate = '" + InvHdr.InventoryDate.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                ", ModifiedOn = GETDATE(), ModifiedBy = '" + username + "' " +
                            "WHERE stocktakeid=" + tmpObj.ToString() + "";
                        cmdTmp = new QueryCommand(SQL, "PowerPOS");
                        DataService.ExecuteQuery(cmdTmp);
                    }
                    else
                    {
                        //if exist, update the count qty
                        //for every item, create insert statement
                        StockTake st = new StockTake();
                        st.IsAdjusted = false;
                        st.ItemNo = InvDet[i].ItemNo;
                        st.StockTakeDate = InvHdr.InventoryDate;
                        st.StockTakeQty = InvDet[i].Quantity;
                        st.BalQtyAtEntry = InventoryController.GetStockBalanceQtyByItemByDate(st.ItemNo, InvHdr.InventoryLocationID.Value, InvHdr.InventoryDate, out status);
                        st.AdjustmentQty = st.StockTakeQty - st.BalQtyAtEntry;
                        st.CostOfGoods = InventoryController.FetchAverageCostOfGoodsLeftByItemNo(st.BalQtyAtEntry, InvDet[i].ItemNo, InvHdr.InventoryLocationID.Value);
                        st.TakenBy = takenBy;
                        st.VerifiedBy = verifiedBy;
                        st.Marked = false;
                        st.InventoryLocationID = InvHdr.InventoryLocationID.Value;
                        cmd.Add(st.GetInsertCommand(username));
                    }
                }
                DataService.ExecuteTransaction(cmd);
                status = "";
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }*/

        public void SetInventoryLocation(int InventoryLocationID)
        {
            InvHdr.InventoryLocationID = InventoryLocationID;
        }

        private DataTable CreateCSVImportErrorMessageDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("row", System.Type.GetType("System.Int32"));
            dt.Columns.Add("barcode");
            dt.Columns.Add("itemno");
            dt.Columns.Add("quantity");
            dt.Columns.Add("timestamp");
            dt.Columns.Add("error");

            return dt;
        }
        public void AddNewImportErrorMessage(int row, string barcode, string itemno, string quantity, string timestamp, string error, ref DataTable dt)
        {
            DataRow dr = dt.NewRow();
            dr["row"] = row;
            dr["barcode"] = barcode;
            dr["itemno"] = itemno;
            dr["quantity"] = quantity;
            dr["timestamp"] = timestamp;
            dr["error"] = error;
            dt.Rows.Add(dr);

            return;

        }
        /*public bool ImportFromDataCollectorTextFile(string filepath, out DataTable message)
        {
            InventoryDetCollection beforeModification = new InventoryDetCollection();
            InvDet.CopyTo(beforeModification);
            DataTable dt = CreateCSVImportErrorMessageDataTable();
            try
            {
                char[] IMPORT_DELIMITER = { ',' };

                //ArrayList problematicLine = new ArrayList();


                string line;
                string[] splitted;
                // Read the file and display it line by line.
                System.IO.StreamReader file =
                   new System.IO.StreamReader(filepath);


                int rowNumber = 0;
                while ((line = file.ReadLine()) != null)
                {
                    rowNumber += 1;
                    string itemno;
                    splitted = line.Split(IMPORT_DELIMITER, 3);

                    if (splitted.Length == 3) //correct format
                    {
                        int Qty;
                        //parse if quantity is wrong....
                        splitted[1] = splitted[1].Replace("\"", ""); //remove quote if any
                        if (int.TryParse(splitted[1], out Qty))
                        {
                            //remove quote if any
                            splitted[0] = splitted[0].Replace("\"", "");
                            //Get product by barcode....                        
                            if (ItemController.IsInventoryItemBarcode(splitted[0].ToString(), out itemno))
                            {
                                string status;
                                //add to temporary inventory detail....
                                AddItemIntoInventory(itemno, Qty, out status);
                                if (status != "")
                                {
                                    AddNewImportErrorMessage(rowNumber, splitted[0], itemno, splitted[1], splitted[2], "Unable to add item. " + status, ref dt);
                                    Logger.writeLog(rowNumber + " " + splitted[0] + " " + itemno + " " + splitted[1] + " " + splitted[2] + " Unable to add item: " + status);

                                }
                            }
                            else
                            {
                                AddNewImportErrorMessage(rowNumber, splitted[0], "-", splitted[1], splitted[2], "Unable to add item. Barcode cant be recognized", ref dt);
                                Logger.writeLog(rowNumber + " " + splitted[0] + " " + splitted[1] + " " + splitted[2] + "Unable to add item. Non Inventory item barcode or item is deleted");
                            }
                        }
                        else
                        {
                            AddNewImportErrorMessage(rowNumber, splitted[0], "-", splitted[1], splitted[2], "Quantity in a wrong format", ref dt);
                            Logger.writeLog(rowNumber + " " + splitted[0] + " " + "-" + " " + splitted[1] + " " + splitted[2] + "Quantity in a wrong format");
                        }
                    }
                    else
                    {
                        AddNewImportErrorMessage(rowNumber, "", "", "", "", "Line in wrong format:" + line, ref dt);
                        Logger.writeLog("Row: " + rowNumber + "Line in wrong format:" + line);
                        //return error message on invalid rows...                    
                    }
                }
                file.Close();
                message = dt;
                if (dt != null && dt.Rows.Count > 0)
                {

                    InvDet = beforeModification;
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error importing CSV file. ");
                Logger.writeLog(ex);
                InvDet = beforeModification;
                message = dt;
                return false;
            }
        }
        public bool ImportFromCSVTextFile(string filepath, out DataTable message)
        {
            InventoryDetCollection beforeModification = new InventoryDetCollection();
            InvDet.CopyTo(beforeModification);
            DataTable dt = CreateCSVImportErrorMessageDataTable();
            try
            {
                char[] IMPORT_DELIMITER = { ',' };

                string line;
                string[] splitted;
                // Read the file and display it line by line.
                System.IO.StreamReader file =
                   new System.IO.StreamReader(filepath);


                int rowNumber = 0;
                while ((line = file.ReadLine()) != null)
                {
                    rowNumber += 1;
                    string itemno;
                    splitted = line.Split(IMPORT_DELIMITER, 3);

                    if (splitted.Length == 2) //correct format
                    {
                        int Qty;
                        //parse if quantity is wrong....
                        splitted[1] = splitted[1].Replace("\"", ""); //remove quote if any
                        if (int.TryParse(splitted[1], out Qty))
                        {
                            //remove quote if any
                            splitted[0] = splitted[0].Replace("\"", "");

                            //Get product by barcode....                        
                            if (true)
                            {
                                string status;
                                itemno = splitted[0].ToString();
                                //add to temporary inventory detail....
                                AddItemIntoInventory(itemno, Qty, out status);
                                if (status != "")
                                {
                                    AddNewImportErrorMessage(rowNumber, splitted[0], itemno, splitted[1], "", "Unable to add item. " + status, ref dt);
                                    Logger.writeLog(rowNumber + " " + splitted[0] + " " + itemno + " " + splitted[1] + " " + "" + " Unable to add item: " + status);

                                }
                            }
                            else
                            {
                                //unreachable code
                                //AddNewImportErrorMessage(rowNumber, splitted[0], "-", splitted[1], splitted[2], "Unable to add item. Barcode cant be recognized", ref dt);
                                //Logger.writeLog(rowNumber + " " + splitted[0] + " " + splitted[1] + " " + splitted[2] + "Unable to add item. Non Inventory item barcode or item is deleted");
                            }
                        }
                        else
                        {
                            AddNewImportErrorMessage(rowNumber, splitted[0], "-", splitted[1], "", "Quantity in a wrong format", ref dt);
                            Logger.writeLog(rowNumber + " " + splitted[0] + " " + "-" + " " + splitted[1] + " " + "" + "Quantity in a wrong format");
                        }
                    }
                    else
                    {
                        AddNewImportErrorMessage(rowNumber, "", "", "", "", "Line in wrong format:" + line, ref dt);
                        Logger.writeLog("Row: " + rowNumber + "Line in wrong format:" + line);
                        //return error message on invalid rows...                    
                    }
                }
                file.Close();
                message = dt;
                if (dt != null && dt.Rows.Count > 0)
                {

                    InvDet = beforeModification;
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error importing CSV file. ");
                Logger.writeLog(ex);
                InvDet = beforeModification;
                message = dt;
                return false;
            }
        }
        public bool ImportFromDataTable(DataTable message, out DataTable ErrorMessage)
        {
            ErrorMessage = CreateCSVImportErrorMessageDataTable();
            InventoryDetCollection beforeModification = new InventoryDetCollection();
            InvDet.CopyTo(beforeModification);
            DataTable ErrorDb = CreateCSVImportErrorMessageDataTable();
            try
            {
                string ItemNoColumnName = "";
                string QtyColumnName = "";
                string BarcodeColumnName = "";

                #region *) Get Columns' Name
                if (message.Columns.Contains("Item No"))
                { ItemNoColumnName = "Item No"; }
                else if (message.Columns.Contains("ItemNo"))
                { ItemNoColumnName = "ItemNo"; }
                else
                { ItemNoColumnName = ""; }
                //{ throw new Exception("Cannot find ItemNo field"); }

                if (message.Columns.Contains("Qty"))
                { QtyColumnName = "Qty"; }
                else if (message.Columns.Contains("Quantity"))
                { QtyColumnName = "Quantity"; }
                else
                { QtyColumnName = ""; }

                if (message.Columns.Contains("Barcode"))
                { BarcodeColumnName = "Barcode"; }
                else
                {
                    if (ItemNoColumnName == "")
                        throw new Exception("Cannot find ItemNo/Barcode field");
                    else
                        BarcodeColumnName = "";
                }
                #endregion

                DataTable newMessage = new DataTable();
                newMessage.Columns.Add("ItemNo", Type.GetType("System.String"));
                newMessage.Columns.Add("Qty", Type.GetType("System.Int32"));

                List<string> ItemNoCollection = new List<string>();
                for (int Counter = 0; Counter < message.Rows.Count; Counter++)
                //foreach (DataRow Rw in message.Rows)
                {
                    DataRow Rw = message.Rows[Counter];

                    //string ItemNo = Rw[ItemNoColumnName].ToString();
                    string ItemNo = "";
                    string Barcode = "";
                    string sQty = "";

                    #region *) Get Raw information 
                    if (ItemNoColumnName != "")
                        ItemNo = Rw[ItemNoColumnName].ToString();

                    if (BarcodeColumnName != "")
                        Barcode = Rw[BarcodeColumnName].ToString();

                    sQty = string.IsNullOrEmpty(QtyColumnName) ? "0" : Rw[QtyColumnName].ToString();
                    #endregion

                    int Qty = 0;
                    #region *) Validation: If no Quantity, ignore
                    if (string.IsNullOrEmpty(sQty) || (string.IsNullOrEmpty(ItemNo) && string.IsNullOrEmpty(Barcode)))
                        continue;
                    #endregion

                    #region *) Get ItemNo from Barcode, and Vice Versa
                    if (ItemNo == "" && Barcode == "")
                    {
                        AddNewImportErrorMessage(Counter, Barcode, ItemNo, sQty, "", "Unable to read this Item No / Barcode for row #" + Counter.ToString(), ref ErrorDb);
                        continue;
                    }
                    else if (ItemNo == "")
                    {
                        ItemCollection MatchItem = new ItemCollection();
                        MatchItem.LoadByBarcode(Barcode);

                        if (MatchItem.Count == 0)
                        {
                            AddNewImportErrorMessage(Counter, Rw[BarcodeColumnName].ToString(), ItemNo, sQty, "", "Barcode not found. Please check this item on the POS Back-End web", ref ErrorDb);
                            continue;
                        }
                        else if (MatchItem.Count > 1)
                        {
                            AddNewImportErrorMessage(Counter, Rw[BarcodeColumnName].ToString(), ItemNo, sQty, "", "Duplicated barcode found. Please check this item on the POS Back-End web", ref ErrorDb);
                            continue;
                        }
                        else
                        {
                            ItemNo = MatchItem[0].ItemNo;
                        }
                    }
                    else if (Barcode == "")
                    {
                        Item MatchItem = new Item(ItemNo);
                        Barcode = MatchItem.Barcode;
                    }
                    #endregion

                    if (!int.TryParse(sQty, out Qty))
                        AddNewImportErrorMessage(Counter, Barcode, ItemNo, sQty, "", "Cannot read Quantity value. Please check the file that you are trying to import.", ref ErrorDb);

                    string status;
                    AddItemIntoInventory(ItemNo, Qty, out status);

                    if (status != "")
                    {
                        AddNewImportErrorMessage(Counter, Barcode, ItemNo, sQty, "", "Unable to add item. " + status, ref ErrorDb);
                        Logger.writeLog(Counter + " " + BarcodeColumnName + " " + ItemNoColumnName + " " + sQty + " " + "" + " Unable to add item: " + status);
                    }


                    if (ItemNoCollection.Contains(ItemNo))
                        AddNewImportErrorMessage(Counter, Barcode, ItemNo, sQty, "", "Duplicated Item No", ref ErrorDb);

                    ItemNoCollection.Add(ItemNo);
                }


                /// Important Part
                //string status;
                //itemno = splitted[0].ToString();
                ////add to temporary inventory detail....
                //AddItemIntoInventory(itemno, Qty, out status);
                //if (status != "")
                //{
                //    AddNewImportErrorMessage(rowNumber, splitted[0], itemno, splitted[1], "", "Unable to add item. " + status, ref dt);
                //    Logger.writeLog(rowNumber + " " + splitted[0] + " " + itemno + " " + splitted[1] + " " + "" + " Unable to add item: " + status);

                //}

                /// If error occurred (Revert)
                ErrorMessage = ErrorDb;
                if (ErrorDb != null && ErrorDb.Rows.Count > 0)
                {

                    InvDet = beforeModification;
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error importing CSV file. ");
                Logger.writeLog(ex);
                InvDet = beforeModification;
                ErrorMessage = ErrorDb;
                return false;
            }
        }*/

        public void UpdateCurrency(int CurrencyID)
        {
            Currency c = new Currency(CurrencyID);
            if (c.IsLoaded && !c.IsNew)
            {
                //assign...
                InvHdr.CurrencyID = CurrencyID;

                //work on InvDetCollection
                for (int i = 0; i < InvDet.Count; i++)
                {
                    InvDet[i].FactoryPrice = ItemController.FetchCostPrice
                        (CurrencyID, InvDet[i].ItemNo);
                }
            }
        }

        public int getCurrencyID()
        {
            if (InvHdr.CurrencyID.HasValue) return InvHdr.CurrencyID.Value;

            return 0;
        }

        public bool IsNew()
        {
            //saved or unsaved
            return InvHdr.IsNew;

        }

        public DataTable InvHdrToDataTable()
        {
            try
            {
                PurchaseOrderHeaderCollection tmp = new PurchaseOrderHeaderCollection();
                tmp.Add(InvHdr);
                return tmp.ToDataTable();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        public DataTable InvDetToDataTable()
        {
            try
            {
                return InvDet.ToDataTable();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        public void createNewGUID()
        {
            //InvHdr.UniqueID = Guid.NewGuid();
        }

        /*public static DataTable FetchItemTrace
            (bool useStartDate, bool useEndDate,
             DateTime StartDate, DateTime EndDate,
             string ItemNo)
        {

            ViewInventoryActivityCollection myViewInventoryActivity = new ViewInventoryActivityCollection();
            if (useStartDate & useEndDate)
            {
                myViewInventoryActivity.BetweenAnd(ViewInventoryActivity.Columns.InventoryDate, StartDate, EndDate);
            }
            else if (useStartDate)
            {
                myViewInventoryActivity.Where(ViewInventoryActivity.Columns.InventoryDate, SubSonic.Comparison.GreaterOrEquals, StartDate);
            }
            else if (useEndDate)
            {
                myViewInventoryActivity.Where(ViewInventoryActivity.Columns.InventoryDate, SubSonic.Comparison.LessOrEquals, EndDate);
            }

            myViewInventoryActivity.Where(ViewInventoryActivity.Columns.ItemNo, ItemNo);

            myViewInventoryActivity.OrderByAsc(ViewInventoryActivity.Columns.InventoryDate);

            DataTable dt = myViewInventoryActivity.Load().ToDataTable();
            InventoryLocationCollection inv = new InventoryLocationCollection();
            inv.Where(InventoryLocation.Columns.Deleted, false);
            inv.Load();
            string status;
            dt.Columns.Add("RefNo");
            dt.Columns.Add("OnHand", Type.GetType("System.Int32"));
            for (int i = 0; i < inv.Count; i++)
            {
                dt.Columns.Add(inv[i].InventoryLocationName, Type.GetType("System.Int32"));
            }
            //populate the rows...
            DateTime myDate;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DateTime.TryParse(dt.Rows[i]["InventoryDate"].ToString(), out myDate);
                dt.Rows[i]["OnHand"] = InventoryController.GetStockBalanceQtyByItemByDate(ItemNo, 0, myDate.AddSeconds(1), out status);

                switch (dt.Rows[i]["MovementType"].ToString())
                {
                    case InventoryController.InventoryMovementType_StockOut:
                        if (dt.Rows[i]["StockOutReasonID"].ToString() == "0")
                        {
                            dt.Rows[i]["MovementType"] = dt.Rows[i]["ReasonName"].ToString();
                            //dt.Rows[i]["RefNo"] = "Rcp#: OR" + dt.Rows[i]["OrderRefNo"].ToString();
                            dt.Rows[i]["RefNo"] = dt.Rows[i]["InventoryHdrRefNo"].ToString();
                        }
                        else
                        {
                            dt.Rows[i]["MovementType"] = dt.Rows[i]["ReasonName"].ToString();
                        }
                        break;
                    case InventoryController.InventoryMovementType_StockIn:
                        //dt.Rows[i]["RefNo"] = "PO #: " + dt.Rows[i]["PurchaseOrderNo"].ToString();
                        dt.Rows[i]["RefNo"] = dt.Rows[i]["InventoryHdrRefNo"].ToString();
                        break;
                    case InventoryController.InventoryMovementType_TransferIn:
                        dt.Rows[i]["RefNo"] = dt.Rows[i]["InventoryHdrRefNo"].ToString();
                        break;
                    case InventoryController.InventoryMovementType_TransferOut:
                        dt.Rows[i]["RefNo"] = dt.Rows[i]["InventoryHdrRefNo"].ToString();
                        break;
                    case InventoryController.InventoryMovementType_AdjustmentIn:
                        dt.Rows[i]["RefNo"] = dt.Rows[i]["InventoryHdrRefNo"].ToString();
                        break;
                    case InventoryController.InventoryMovementType_AdjustmentOut:
                        dt.Rows[i]["RefNo"] = dt.Rows[i]["InventoryHdrRefNo"].ToString();
                        break;
                }
                for (int j = 0; j < inv.Count; j++)
                {
                    dt.Rows[i][inv[j].InventoryLocationName] =
                        InventoryController.GetStockBalanceQtyByItemByDate
                        (ItemNo, inv[j].InventoryLocationID, myDate.AddSeconds(1), out status).ToString().Replace("'", "").Replace("`", "");
                }
            }

            //remove unwanted columns
            //dt.Columns.Remove("");
            dt.Columns.Remove("inventorydetrefno");
            dt.Columns.Remove("itemno");
            dt.Columns.Remove("remainingqty");
            dt.Columns.Remove("itemremark");
            dt.Columns.Remove("invoiceno");
            dt.Columns.Remove("supplier");
            dt.Columns.Remove("remark");
            dt.Columns.Remove("username");
            dt.Columns.Remove("inventoryhdrrefno");
            dt.Columns.Remove("itemname");
            dt.Columns.Remove("categoryname");
            dt.Columns.Remove("inventorylocationname");
            dt.Columns.Remove("costofgoods");
            dt.Columns.Remove("stockinrefno");
            dt.Columns.Remove("inventorylocationid");
            dt.Columns.Remove("isdiscrepancy");
            dt.Columns.Remove("reasonname");
            dt.Columns.Remove("factoryprice");
            dt.Columns.Remove("gst");
            dt.Columns.Remove("stockoutreasonid");
            dt.Columns.Remove("retailprice");
            dt.Columns.Remove("factorypriceusd");
            dt.Columns.Remove("productline");
            dt.Columns.Remove("departmentid");
            dt.Columns.Remove("productiondate");
            dt.Columns.Remove("attributes1");
            dt.Columns.Remove("attributes2");
            dt.Columns.Remove("attributes3");
            dt.Columns.Remove("attributes4");
            dt.Columns.Remove("attributes5");
            dt.Columns.Remove("attributes6");
            dt.Columns.Remove("attributes7");
            dt.Columns.Remove("attributes8");
            dt.Columns.Remove("HasWarranty");
            dt.Columns.Remove("IsDelivery");
            dt.Columns.Remove("GSTrule");
            dt.Columns.Remove("isVitaMix");
            dt.Columns.Remove("iswaterfilter");
            dt.Columns.Remove("isyoung");
            dt.Columns.Remove("isjuiceplus");
            dt.Columns.Remove("iscourse");
            dt.Columns.Remove("coursetypeid");
            dt.Columns.Remove("isserviceitem");
            dt.Columns.Remove("search");
            dt.Columns.Remove("departmentname");
            dt.Columns.Remove("itemdepartmentid");
            dt.Columns.Remove("barcode");
            dt.Columns.Remove("itemdesc");
            dt.Columns.Remove("minimumprice");
            dt.Columns.Remove("isininventory");
            dt.Columns.Remove("brand");
            dt.Columns.Remove("isnondiscountable");
            dt.Columns.Remove("deleted");
            dt.Columns.Remove("expirydate");
            dt.Columns.Remove("balancebefore");
            dt.Columns.Remove("balanceafter");
            dt.Columns.Remove("expr1");

            dt.Columns["MovementType"].SetOrdinal(2);
            dt.Columns["MovementType"].ColumnName = "Event";
            dt.Columns["inventorydate"].ColumnName = "Date";
            dt.Columns["Quantity"].SetOrdinal(4);
            dt.Columns["Quantity"].ColumnName = "Qty";

            dt.TableName = "Item Trace For " + ItemNo;

            //pull out from View
            ViewTransactionDetailCollection v = new ViewTransactionDetailCollection();
            v.Where(ViewTransactionDetail.Columns.IsVoided, false);
            v.Where(ViewTransactionDetail.Columns.IsLineVoided, false);
            v.Where(ViewTransactionDetail.Columns.OrderDetDate, Comparison.GreaterOrEquals, StartDate);
            v.Where(ViewTransactionDetail.Columns.OrderDetDate, Comparison.LessOrEquals, EndDate);
            v.Where(ViewTransactionDetail.Columns.ItemNo, ItemNo);
            v.OrderByAsc(ViewTransactionDetail.Columns.OrderDetDate);
            v.Load();

            ArrayList removeList = new ArrayList();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < v.Count; j++)
                {
                    if (string.IsNullOrEmpty(v[j].InventoryHdrRefNo)) continue;

                    if (dt.Rows[i]["RefNo"].ToString() == v[j].InventoryHdrRefNo)
                    {
                        dt.Rows[i]["RefNo"] = "Receipt No:" + v[j].OrderRefNo;
                       
                        removeList.Add(j);
                    }
                }
            }
            removeList.Sort();
            for (int p = removeList.Count - 1; p >= 0; --p)
            {
                if (p < v.Count)
                    v.RemoveAt((int)removeList[p]);
            }
            for (int p = 0; p < v.Count; p++)
            {
                DataRow drNewRow = dt.NewRow();
                ViewTransactionDetail myReceipt = v[p];
                drNewRow["Date"] = myReceipt.OrderDetDate;
                drNewRow["Qty"] = myReceipt.Quantity;
                if (v[p].InventoryHdrRefNo == "ADJUSTED")
                {
                    drNewRow["Event"] = "Sales (Adjusted)";
                }
                else if (v[p].InventoryHdrRefNo == "")
                {
                    drNewRow["Event"] = "Sales (Undeducted)";
                }
                else 
                {                
                    drNewRow["Event"] = "Sales (Unknown ID:" + v[p].InventoryHdrRefNo + ")";
                }

                drNewRow["RefNo"] = myReceipt.OrderRefNo;
                dt.Rows.Add(drNewRow);
            }

            return dt;
        }

        public static DataTable FetchItemTrace_New
            (bool useStartDate, bool useEndDate, DateTime StartDate, DateTime EndDate, string ItemNo)
        {

            ViewInventoryActivityCollection myViewInventoryActivity = new ViewInventoryActivityCollection();
            if (useStartDate & useEndDate)
            {
                myViewInventoryActivity.BetweenAnd(ViewInventoryActivity.Columns.InventoryDate, StartDate, EndDate);
            }
            else if (useStartDate)
            {
                myViewInventoryActivity.Where(ViewInventoryActivity.Columns.InventoryDate, SubSonic.Comparison.GreaterOrEquals, StartDate);
            }
            else if (useEndDate)
            {
                myViewInventoryActivity.Where(ViewInventoryActivity.Columns.InventoryDate, SubSonic.Comparison.LessOrEquals, EndDate);
            }

            myViewInventoryActivity.Where(ViewInventoryActivity.Columns.ItemNo, ItemNo);

            myViewInventoryActivity.OrderByAsc(ViewInventoryActivity.Columns.InventoryDate);

            DataTable dt = myViewInventoryActivity.Load().ToDataTable();
            InventoryLocationCollection inv = new InventoryLocationCollection();
            inv.Where(InventoryLocation.Columns.Deleted, false);
            inv.Load();
            string status;
            dt.Columns.Add("RefNo");
            dt.Columns.Add("OnHand", Type.GetType("System.Int32"));
            for (int i = 0; i < inv.Count; i++)
            {
                dt.Columns.Add(inv[i].InventoryLocationName, Type.GetType("System.Int32"));
            }
            //populate the rows...
            DateTime myDate;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DateTime.TryParse(dt.Rows[i]["InventoryDate"].ToString(), out myDate);
                dt.Rows[i]["OnHand"] = InventoryController.GetStockBalanceQtyByItemByDate(ItemNo, 0, myDate.AddSeconds(1), out status);

                switch (dt.Rows[i]["MovementType"].ToString())
                {
                    case InventoryController.InventoryMovementType_StockOut:
                        if (dt.Rows[i]["StockOutReasonID"].ToString() == "0")
                        {
                            dt.Rows[i]["MovementType"] = dt.Rows[i]["ReasonName"].ToString();
                            //dt.Rows[i]["RefNo"] = "Rcp#: OR" + dt.Rows[i]["OrderRefNo"].ToString();
                            dt.Rows[i]["RefNo"] = dt.Rows[i]["InventoryHdrRefNo"].ToString();
                        }
                        else
                        {
                            dt.Rows[i]["MovementType"] = dt.Rows[i]["ReasonName"].ToString();
                        }
                        break;
                    case InventoryController.InventoryMovementType_StockIn:
                        //dt.Rows[i]["RefNo"] = "PO #: " + dt.Rows[i]["PurchaseOrderNo"].ToString();
                        dt.Rows[i]["RefNo"] = dt.Rows[i]["InventoryHdrRefNo"].ToString();
                        break;
                    case InventoryController.InventoryMovementType_TransferIn:
                        dt.Rows[i]["RefNo"] = dt.Rows[i]["InventoryHdrRefNo"].ToString();
                        break;
                    case InventoryController.InventoryMovementType_TransferOut:
                        dt.Rows[i]["RefNo"] = dt.Rows[i]["InventoryHdrRefNo"].ToString();
                        break;
                    case InventoryController.InventoryMovementType_AdjustmentIn:
                        dt.Rows[i]["RefNo"] = dt.Rows[i]["InventoryHdrRefNo"].ToString();
                        break;
                    case InventoryController.InventoryMovementType_AdjustmentOut:
                        dt.Rows[i]["RefNo"] = dt.Rows[i]["InventoryHdrRefNo"].ToString();
                        break;
                }
                for (int j = 0; j < inv.Count; j++)
                {
                    dt.Rows[i][inv[j].InventoryLocationName] =
                        InventoryController.GetStockBalanceQtyByItemByDate
                        (ItemNo, inv[j].InventoryLocationID, myDate.AddSeconds(1), out status).ToString().Replace("'", "").Replace("`", "");
                }
            }

            //remove unwanted columns
            //dt.Columns.Remove("");
            dt.Columns.Remove("inventorydetrefno");
            dt.Columns.Remove("itemno");
            dt.Columns.Remove("remainingqty");
            dt.Columns.Remove("itemremark");
            dt.Columns.Remove("invoiceno");
            dt.Columns.Remove("supplier");
            dt.Columns.Remove("remark");
            dt.Columns.Remove("username");
            dt.Columns.Remove("inventoryhdrrefno");
            dt.Columns.Remove("itemname");
            dt.Columns.Remove("categoryname");
            dt.Columns.Remove("inventorylocationname");
            dt.Columns.Remove("costofgoods");
            dt.Columns.Remove("stockinrefno");
            dt.Columns.Remove("inventorylocationid");
            dt.Columns.Remove("isdiscrepancy");
            dt.Columns.Remove("reasonname");
            dt.Columns.Remove("factoryprice");
            dt.Columns.Remove("gst");
            dt.Columns.Remove("stockoutreasonid");
            dt.Columns.Remove("retailprice");
            dt.Columns.Remove("factorypriceusd");
            dt.Columns.Remove("productline");
            dt.Columns.Remove("departmentid");
            dt.Columns.Remove("productiondate");
            dt.Columns.Remove("attributes1");
            dt.Columns.Remove("attributes2");
            dt.Columns.Remove("attributes3");
            dt.Columns.Remove("attributes4");
            dt.Columns.Remove("attributes5");
            dt.Columns.Remove("attributes6");
            dt.Columns.Remove("attributes7");
            dt.Columns.Remove("attributes8");
            dt.Columns.Remove("HasWarranty");
            dt.Columns.Remove("IsDelivery");
            dt.Columns.Remove("GSTrule");
            dt.Columns.Remove("isVitaMix");
            dt.Columns.Remove("iswaterfilter");
            dt.Columns.Remove("isyoung");
            dt.Columns.Remove("isjuiceplus");
            dt.Columns.Remove("iscourse");
            dt.Columns.Remove("coursetypeid");
            dt.Columns.Remove("isserviceitem");
            dt.Columns.Remove("search");
            dt.Columns.Remove("departmentname");
            dt.Columns.Remove("itemdepartmentid");
            dt.Columns.Remove("barcode");
            dt.Columns.Remove("itemdesc");
            dt.Columns.Remove("minimumprice");
            dt.Columns.Remove("isininventory");
            dt.Columns.Remove("brand");
            dt.Columns.Remove("isnondiscountable");
            dt.Columns.Remove("deleted");
            dt.Columns.Remove("expirydate");
            dt.Columns.Remove("balancebefore");
            dt.Columns.Remove("balanceafter");
            dt.Columns.Remove("expr1");

            dt.Columns["MovementType"].SetOrdinal(2);
            dt.Columns["MovementType"].ColumnName = "Event";
            dt.Columns["inventorydate"].ColumnName = "Date";
            dt.Columns["Quantity"].SetOrdinal(4);
            dt.Columns["Quantity"].ColumnName = "Qty";

            dt.TableName = "Item Trace For " + ItemNo;

            //pull out from View
            ViewTransactionDetailCollection v = new ViewTransactionDetailCollection();
            v.Where(ViewTransactionDetail.Columns.IsVoided, false);
            v.Where(ViewTransactionDetail.Columns.IsLineVoided, false);
            v.Where(ViewTransactionDetail.Columns.OrderDetDate, Comparison.GreaterOrEquals, StartDate);
            v.Where(ViewTransactionDetail.Columns.OrderDetDate, Comparison.LessOrEquals, EndDate);
            v.Where(ViewTransactionDetail.Columns.ItemNo, ItemNo);
            v.OrderByAsc(ViewTransactionDetail.Columns.OrderDetDate);
            v.Load();

            ArrayList removeList = new ArrayList();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < v.Count; j++)
                {
                    if (string.IsNullOrEmpty(v[j].InventoryHdrRefNo)) continue;

                    if (dt.Rows[i]["RefNo"].ToString() == v[j].InventoryHdrRefNo)
                    {
                        dt.Rows[i]["RefNo"] = "Receipt No:" + v[j].OrderRefNo;
                        removeList.Add(j);
                    }
                }
            }
            removeList.Sort();
            for (int p = removeList.Count - 1; p >= 0; --p)
            {
                if (p < v.Count)
                    v.RemoveAt((int)removeList[p]);
            }
            for (int p = 0; p < v.Count; p++)
            {
                DataRow drNewRow = dt.NewRow();
                ViewTransactionDetail myReceipt = v[p];
                drNewRow["Date"] = myReceipt.OrderDetDate;
                drNewRow["Qty"] = myReceipt.Quantity;
                if (v[p].InventoryHdrRefNo == "ADJUSTED")
                {
                    drNewRow["Event"] = "Sales (Adjusted)";
                }
                else if (v[p].InventoryHdrRefNo == "")
                {
                    drNewRow["Event"] = "Sales (Undeducted)";
                }
                else
                {
                    drNewRow["Event"] = "Sales (Unknown ID:" + v[p].InventoryHdrRefNo + ")";
                }

                drNewRow["RefNo"] = myReceipt.OrderRefNo;
                dt.Rows.Add(drNewRow);
            }

            return dt;
        }*/

        public void SetInventoryHdrUserName(string pUserName)
        {
            InvHdr.UserName = pUserName;
        }

        public string GetInventoryLocation()
        {
            string invLoc = "";

            InventoryLocation il = new InventoryLocation(InvHdr.InventoryLocationID.GetValueOrDefault(0));
            if (!il.IsNew)
                invLoc = il.InventoryLocationName;

            return invLoc;
        }

        public int GetInventoryLocationID()
        {
            return InvHdr.InventoryLocationID.GetValueOrDefault(0);
        }

        public string getSupplier()
        {
            return "";
        }

        public int getSupplierID() {
            return InvHdr.SupplierID.GetValueOrDefault(0);
        }

        public int getWarehouseID()
        {
            return InvHdr.WarehouseID;
        }

        public string getSupplierName()
        {
            Supplier s = new Supplier(InvHdr.Supplier);
            if (s != null && s.IsLoaded)
            {
                return s.SupplierName;
            }
            return "";
        }

        public decimal GetFreightCharges()
        {
            /*if (InvHdr.FreightCharge.HasValue)
                return InvHdr.FreightCharge.Value;*/
            return 0;
        }

        public decimal getDiscount()
        {
            if (InvHdr.Discount.HasValue)
                return InvHdr.Discount.Value;
            return 0;
        }

        public string getSalesPersonID()
        {
           return InvHdr.SalesPersonID;
        }

        public double getExchangeRate()
        {
            return 0; //InvHdr.ExchangeRate;
        }

        public static string getNewPurchaseOrderRefNo(int InventoryLocationID)
        {
            int runningNo = 0;
            string PurchaseOrderRefNo;
            /*
            object ob = SPs.GetNewInventoryRefNoByInventoryLocationID(InventoryLocationID).ExecuteScalar();
            if (ob != null)
            {
                int.TryParse(ob.ToString(), out runningNo);
            }
            */
            string header = "PO" + DateTime.Now.ToString("yyMMdd") + InventoryLocationID.ToString().PadLeft(4, '0');
            Query qr = PurchaseOrderHeader.CreateQuery();
            qr.AddWhere(PurchaseOrderHeader.Columns.PurchaseOrderHeaderRefNo, Comparison.Like, header + "%");
            qr.QueryType = QueryType.Select;
            qr.Top = "1";
            qr.SetSelectList("PurchaseOrderHeaderRefNo");
            qr.OrderBy = OrderBy.Desc("PurchaseOrderHeaderRefNo");

            DataSet ds = qr.ExecuteDataSet();
            if (ds.Tables[0].Rows.Count > 0)
            {
                runningNo = int.Parse(ds.Tables[0].Rows[0][0].ToString().Substring(header.Length));
            }
            else
            {
                runningNo = 0;
            }
            runningNo += 1;

            //INYYMMDDSSSSNNNN                
            //YY - year
            //MM - month
            //DD - day
            //SSSS - PointOfSale ID
            //NNNN - Running No
            PurchaseOrderRefNo = header + runningNo.ToString().PadLeft(4, '0');

            return PurchaseOrderRefNo;
        }

        public static string getNewBackOrderPurchaseOrderRefNo(int InventoryLocationID)
        {
            int runningNo = 0;
            string PurchaseOrderRefNo;
            /*
            object ob = SPs.GetNewInventoryRefNoByInventoryLocationID(InventoryLocationID).ExecuteScalar();
            if (ob != null)
            {
                int.TryParse(ob.ToString(), out runningNo);
            }
            */
            string header = "BK" + DateTime.Now.ToString("yyMMdd") + InventoryLocationID.ToString().PadLeft(4, '0');
            Query qr = PurchaseOrderHeader.CreateQuery();
            qr.AddWhere(PurchaseOrderHeader.Columns.PurchaseOrderHeaderRefNo, Comparison.Like, header + "%");
            qr.QueryType = QueryType.Select;
            qr.Top = "1";
            qr.SetSelectList("PurchaseOrderHeaderRefNo");
            qr.OrderBy = OrderBy.Desc("PurchaseOrderHeaderRefNo");

            DataSet ds = qr.ExecuteDataSet();
            if (ds.Tables[0].Rows.Count > 0)
            {
                runningNo = int.Parse(ds.Tables[0].Rows[0][0].ToString().Substring(header.Length));
            }
            else
            {
                runningNo = 0;
            }
            runningNo += 1;

            //INYYMMDDSSSSNNNN                
            //YY - year
            //MM - month
            //DD - day
            //SSSS - PointOfSale ID
            //NNNN - Running No
            PurchaseOrderRefNo = header + runningNo.ToString().PadLeft(4, '0');

            return PurchaseOrderRefNo;
        }

        public static string getNewQuotationOrderRefNo(int InventoryLocationID)
        {
            int runningNo = 0;
            string PurchaseOrderRefNo;
            /*
            object ob = SPs.GetNewInventoryRefNoByInventoryLocationID(InventoryLocationID).ExecuteScalar();
            if (ob != null)
            {
                int.TryParse(ob.ToString(), out runningNo);
            }
            */
            string header = "Q" + DateTime.Now.ToString("yyMMdd") + InventoryLocationID.ToString().PadLeft(4, '0');
            Query qr = PurchaseOrderHeader.CreateQuery();
            qr.AddWhere(PurchaseOrderHeader.Columns.PurchaseOrderHeaderRefNo, Comparison.Like, header + "%");
            qr.QueryType = QueryType.Select;
            qr.Top = "1";
            qr.SetSelectList("PurchaseOrderHeaderRefNo");
            qr.OrderBy = OrderBy.Desc("PurchaseOrderHeaderRefNo");

            DataSet ds = qr.ExecuteDataSet();
            if (ds.Tables[0].Rows.Count > 0)
            {
                runningNo = int.Parse(ds.Tables[0].Rows[0][0].ToString().Substring(header.Length));
            }
            else
            {
                runningNo = 0;
            }
            runningNo += 1;

            //INYYMMDDSSSSNNNN                
            //YY - year
            //MM - month
            //DD - day
            //SSSS - PointOfSale ID
            //NNNN - Running No
            PurchaseOrderRefNo = header + runningNo.ToString().PadLeft(4, '0');

            return PurchaseOrderRefNo;
        }

        public bool CreateOrder(string username, int InventoryLocationID, out string status)
        {
            try
            {
                //SupplierItemMap.CreateSupplierItemMapTable();

                QueryCommandCollection cmd;
                cmd = getInsertCommand(username, InventoryLocationID);
                SubSonic.DataService.ExecuteTransaction(cmd);

                InvHdr.IsNew = false;
                status = "";
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




        private QueryCommandCollection getInsertCommand(string username, int InventoryLocationID)
        {
            #region *) Validation: Check whether it is a valid order
            if (InvHdr == null | InvDet == null) //Valid Order?
                throw new Exception("(error)Invalid Inventory data. Unable to receive Inventory.");
            #endregion

            for (int i = 0; i < InvDet.Count; i++)
                if (InvDet[i].Quantity <= 0)
                    throw new Exception("(error)Error: Quantity must be larger than zero.");

            #region *) Conditioning: Set header information
            if (InvHdr.IsNew)
            {
                if (InvHdr.PurchaseOrderHeaderRefNo == null || !InvHdr.PurchaseOrderHeaderRefNo.StartsWith("ST") || !InvHdr.PurchaseOrderHeaderRefNo.StartsWith("BK") || InvHdr.UserName.ToLower() != "system")
                    InvHdr.PurchaseOrderHeaderRefNo = PurchaseOrderController.getNewPurchaseOrderRefNo(InventoryLocationID);
            }
            #endregion
            InvHdr.UserName = username;
            InvHdr.InventoryLocationID = InventoryLocationID;

            //if (!InvHdr.FreightCharge.HasValue) InvHdr.FreightCharge = 0;

            QueryCommandCollection cmd = new QueryCommandCollection();
            QueryCommand mycmd;

            #region *) Save: Generate Save Script for InventoryHdr
            if (InvHdr.IsNew)
            {
                InvHdr.Discount = InvHdr.Discount * 1; //(decimal)InvHdr.ExchangeRate;
                mycmd = InvHdr.GetInsertCommand(UserInfo.username);
            }
            else
            {
                mycmd = InvHdr.GetUpdateCommand(UserInfo.username);
            }
            cmd.Add(mycmd);
            #endregion

            decimal SumOfFactoryPriceAndQty = 0;
            #region *) Calculate: calculate sum of factory price * quantity for distributing Freight charges
            for (int i = 0; i < InvDet.Count; i++)
                SumOfFactoryPriceAndQty += (decimal)InvDet[i].FactoryPrice * (decimal) InvDet[i].Quantity;

            if (SumOfFactoryPriceAndQty == 0)
                SumOfFactoryPriceAndQty = 0.0001M;
            #endregion

            for (int i = 0; i < InvDet.Count; i++)
            {
                #region *) Conditioning: Set detail information
                decimal weight = ((decimal)InvDet[i].FactoryPrice * (decimal) InvDet[i].Quantity) / SumOfFactoryPriceAndQty;

                InvDet[i].PurchaseOrderHeaderRefNo = InvHdr.PurchaseOrderHeaderRefNo;
                InvDet[i].PurchaseOrderDetailRefNo = InvHdr.PurchaseOrderHeaderRefNo + "." + (i + 1).ToString();
                //InvDet[i].RemainingQty = InvDet[i].Quantity;

                //int GetBalBeforeQty = GetStockBalanceQtyByItemByDate(InvDet[i].ItemNo, InvHdr.InventoryLocationID.Value, DateTime.Now, out status);
                //InvDet[i].BalanceBefore = GetBalBeforeQty;
                //InvDet[i].BalanceAfter = GetBalBeforeQty + InvDet[i].Quantity;

                if (InvDet[i].Quantity > 0)
                {
                    //if (InvHdr.ExchangeRate > 0)
                    //    InvDet[i].FactoryPrice = InvDet[i].FactoryPrice * (decimal)InvHdr.ExchangeRate;

                    InvDet[i].FactoryPrice =
                        InvDet[i].FactoryPrice - ((weight * InvHdr.Discount.Value) / InvDet[i].Quantity);

                    //InvDet[i].CostOfGoods = InvDet[i].FactoryPrice
                    //    + (decimal)((weight * InvHdr.FreightCharge.Value) / InvDet[i].Quantity);
                }

                //InvDet[i].Gst = GetGST();
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

                /*if (InvHdr.Supplier != string.Empty) // only if Supplier is not string.empty (--select supplier-- means string empty)
                {
                    SupplierItemMap supItemMap = new SupplierItemMap();
                    supItemMap.SupplierID = int.Parse(InvHdr.Supplier);
                    supItemMap.ItemNo = InvDet[i].ItemNo;
                    supItemMap.UniqueID = Guid.NewGuid();
                    supItemMap.Deleted = false;

                    cmd.Add(supItemMap.GetInsertIfNotExistsCommand());
                }*/

                #endregion
            }
            return cmd;
        }

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

        public bool MarkAsDeletedFromPurchaseOrderDetail(string ID, bool value, out string status)
        {
            status = "";
            try
            {
                ((PowerPOS.PurchaseOrderDetail)InvDet.Find(ID)).Deleted = value;
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }

        public bool ChangeRemark(string ID, string remark, out string status)
        {
            status = "";
            try
            {
                PurchaseOrderDetail myTmpDet;

                myTmpDet = (PowerPOS.PurchaseOrderDetail)InvDet.Find(ID);

                if (myTmpDet == null)
                    return false;

                myTmpDet.Remark = remark;

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }

        public bool LoadFromDataTable(DataTable hdr, DataTable det)
        {
            try
            {
                InvHdr.Load(hdr);
                InvDet.Load(det);
                InvHdr.IsNew = true;
                for (int i = 0; i < InvDet.Count; i++)
                {
                    InvDet[i].IsNew = true;
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }

        public bool ImportFromDataTable(DataTable message, out DataTable ErrorMessage)
        {
            ErrorMessage = CreateCSVImportErrorMessageDataTable();
            PurchaseOrderDetailCollection beforeModification = new PurchaseOrderDetailCollection();
            InvDet.CopyTo(beforeModification);
            DataTable ErrorDb = CreateCSVImportErrorMessageDataTable();
            try
            {
                string ItemNoColumnName = "";
                string QtyColumnName = "";
                string BarcodeColumnName = "";
                string CostPriceColumnName = "";

                #region *) Get Columns' Name
                if (message.Columns.Contains("Item No"))
                { ItemNoColumnName = "Item No"; }
                else if (message.Columns.Contains("ItemNo"))
                { ItemNoColumnName = "ItemNo"; }
                else
                { ItemNoColumnName = ""; }
                //{ throw new Exception("Cannot find ItemNo field"); }

                if (message.Columns.Contains("Qty"))
                { QtyColumnName = "Qty"; }
                else if (message.Columns.Contains("Quantity"))
                { QtyColumnName = "Quantity"; }
                else
                { QtyColumnName = ""; }

                if (message.Columns.Contains("Barcode"))
                { BarcodeColumnName = "Barcode"; }
                else
                {
                    if (ItemNoColumnName == "")
                        throw new Exception("Cannot find ItemNo/Barcode field");
                    else
                        BarcodeColumnName = "";
                }

                if (message.Columns.Contains("CostPrice"))
                { CostPriceColumnName = "CostPrice"; }
                else
                {
                    CostPriceColumnName = "";
                }

                #endregion

                DataTable newMessage = new DataTable();
                newMessage.Columns.Add("ItemNo", Type.GetType("System.String"));
                newMessage.Columns.Add("Qty", Type.GetType("System.Int32"));

                List<string> ItemNoCollection = new List<string>();
                for (int Counter = 0; Counter < message.Rows.Count; Counter++)
                //foreach (DataRow Rw in message.Rows)
                {
                    DataRow Rw = message.Rows[Counter];

                    //string ItemNo = Rw[ItemNoColumnName].ToString();
                    string ItemNo = "";
                    string Barcode = "";
                    string sQty = "";
                    string CPrice = "";

                    #region *) Get Raw information
                    if (ItemNoColumnName != "")
                        ItemNo = Rw[ItemNoColumnName].ToString();

                    if (BarcodeColumnName != "")
                        Barcode = Rw[BarcodeColumnName].ToString();

                    sQty = string.IsNullOrEmpty(QtyColumnName) ? "0" : Rw[QtyColumnName].ToString();
                    CPrice = string.IsNullOrEmpty(CostPriceColumnName) ? "0" : Rw[CostPriceColumnName].ToString();
                    #endregion

                    int Qty = 0;
                    decimal CostPrice = 0;
                    #region *) Validation: If no Quantity, ignore
                    if (string.IsNullOrEmpty(sQty) || (string.IsNullOrEmpty(ItemNo) && string.IsNullOrEmpty(Barcode)))
                        continue;
                    #endregion

                    #region *) Get ItemNo from Barcode, and Vice Versa
                    if (ItemNo == "" && Barcode == "")
                    {
                        AddNewImportErrorMessage(Counter, Barcode, ItemNo, sQty, "", "Unable to read this Item No / Barcode for row #" + Counter.ToString(), ref ErrorDb);
                        continue;
                    }
                    else if (ItemNo == "")
                    {
                        ItemCollection MatchItem = new ItemCollection();
                        MatchItem.LoadByBarcode(Barcode);

                        if (MatchItem.Count == 0)
                        {
                            AddNewImportErrorMessage(Counter, Rw[BarcodeColumnName].ToString(), ItemNo, sQty, "", "Barcode not found. Please check this item on the POS Back-End web", ref ErrorDb);
                            continue;
                        }
                        else if (MatchItem.Count > 1)
                        {
                            AddNewImportErrorMessage(Counter, Rw[BarcodeColumnName].ToString(), ItemNo, sQty, "", "Duplicated barcode found. Please check this item on the POS Back-End web", ref ErrorDb);
                            continue;
                        }
                        else
                        {
                            ItemNo = MatchItem[0].ItemNo;
                        }
                    }
                    else if (Barcode == "")
                    {
                        Item MatchItem = new Item(ItemNo);
                        Barcode = MatchItem.Barcode;
                    }
                    #endregion

                    if (!int.TryParse(sQty, out Qty))
                        AddNewImportErrorMessage(Counter, Barcode, ItemNo, sQty, "", "Cannot read Quantity value. Please check the file that you are trying to import.", ref ErrorDb);

                    if (!decimal.TryParse(CPrice, out CostPrice))
                        AddNewImportErrorMessage(Counter, Barcode, ItemNo, CPrice, "", "Cannot read Cost Price value. Please check the file that you are trying to import.", ref ErrorDb);

                    string status;
                    if (CostPriceColumnName != "")
                    {
                        AddItemIntoInventory1(ItemNo, Qty, CostPrice, out status);
                    }
                    else
                    {
                        AddItemIntoInventory(ItemNo, Qty, out status);
                    }

                    if (status != "")
                    {
                        AddNewImportErrorMessage(Counter, Barcode, ItemNo, sQty, "", "Unable to add item. " + status, ref ErrorDb);
                        Logger.writeLog(Counter + " " + BarcodeColumnName + " " + ItemNoColumnName + " " + sQty + " " + "" + " Unable to add item: " + status);
                    }


                    if (ItemNoCollection.Contains(ItemNo))
                        AddNewImportErrorMessage(Counter, Barcode, ItemNo, sQty, "", "Duplicated Item No", ref ErrorDb);

                    ItemNoCollection.Add(ItemNo);
                }


                /// Important Part
                //string status;
                //itemno = splitted[0].ToString();
                ////add to temporary inventory detail....
                //AddItemIntoInventory(itemno, Qty, out status);
                //if (status != "")
                //{
                //    AddNewImportErrorMessage(rowNumber, splitted[0], itemno, splitted[1], "", "Unable to add item. " + status, ref dt);
                //    Logger.writeLog(rowNumber + " " + splitted[0] + " " + itemno + " " + splitted[1] + " " + "" + " Unable to add item: " + status);

                //}

                /// If error occurred (Revert)
                ErrorMessage = ErrorDb;
                if (ErrorDb != null && ErrorDb.Rows.Count > 0)
                {

                    InvDet = beforeModification;
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error importing CSV file. ");
                Logger.writeLog(ex);
                InvDet = beforeModification;
                ErrorMessage = ErrorDb;
                return false;
            }
        }

        #region "Get Purchase Order Header Information"

        public PurchaseOrderHeader GetPOHeader()
        {
            try
            {
                return InvHdr;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }
        #endregion

        //Delete PurchaseOrder Header & Detail
        public bool DeleteFromPurchaseOrder(string PurchaseOrderHeaderRefNo, out string status)
        {
            status = "";
            try
            {
                foreach (PurchaseOrderDetail det in InvDet)
                {
                    PurchaseOrderDetail.Delete(det.PurchaseOrderDetailRefNo);
                }
                InvDet.Clear();

                PurchaseOrderHeader.Delete(PurchaseOrderHeaderRefNo);

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }

        //Set Expiry Date
        public bool ChangeItemExpiryDate(string ID, DateTime expiryDate, out string status)
        {

            try
            {
                status = "";
                PurchaseOrderDetail myTmpDetail;

                myTmpDetail = (PowerPOS.PurchaseOrderDetail)InvDet.Find(ID);

                if (myTmpDetail == null)
                {
                    status = "PurchaseOrder Detail has not been created";
                    return false;
                }

                myTmpDetail.ExpiryDate = expiryDate;
                InvHdr.Save();
                InvDet.SaveAll();
                return true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return false;
            }
        }

        //Set Status, QtyApproved, and Remark for PurchaseOrderDetailail
        public bool UpdateDetailStatus(string ID, decimal qtyApproved, string newRemark, string newStatus, out string status)
        {
            status = "";
            try
            {
                PurchaseOrderDetail myTmpDetail;

                myTmpDetail = (PowerPOS.PurchaseOrderDetail)InvDet.Find(ID);

                if (myTmpDetail == null)
                {
                    status = "PurchaseOrder Detail has not been created";
                    return false;
                }

                decimal oldqty = myTmpDetail.QtyApproved == 0 ? (myTmpDetail.Quantity ?? 0) : myTmpDetail.QtyApproved;

                myTmpDetail.QtyApproved = qtyApproved;
                myTmpDetail.Remark = newRemark;
                myTmpDetail.Status = newStatus;

                if (myTmpDetail.Amount > 0)
                {
                    myTmpDetail.Amount = myTmpDetail.Amount / (oldqty <= 0 ? 1 : oldqty) * myTmpDetail.QtyApproved;
                    myTmpDetail.GSTAmount = myTmpDetail.GSTAmount / (oldqty <= 0 ? 1 : oldqty) * myTmpDetail.QtyApproved;
                }

                InvHdr.Save();
                InvDet.SaveAll();
                return true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return false;
            }
        }

        //Update PurchaseOrderHeader Status based on PurchaseOrderDetail Status
        public bool UpdateHeaderStatus(out string PurchaseOrderHeaderStatus, out string status, string username)
        {
            status = "";
            PurchaseOrderHeaderStatus = "";
            try
            {
                List<PurchaseOrderDetail> poDet = InvDet.GetList();
                if (poDet.Exists(p => p.Status == "Approved"))
                    InvHdr.Status = "Approved";
                else
                    InvHdr.Status = "Rejected";

                InvHdr.ApprovalDate = DateTime.Now;
                InvHdr.ApprovedBy = username;
                InvHdr.Save();
                PurchaseOrderHeaderStatus = InvHdr.Status;
                return true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return false;
            }
        }

        public bool ChangeRemarkHeader(string Remark, string username,out string status)
        {
            status = "";
           
            try
            {
                InvHdr.Remark = Remark;               
                InvHdr.Save(username);
                
                return true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return false;
            }
        }

        public QueryCommandCollection UpdateHeaderStatusToQueryCommand(out string PurchaseOrderHeaderStatus, out string status, string username)
        {
            status = "";
            PurchaseOrderHeaderStatus = "";
            QueryCommandCollection tmp = new QueryCommandCollection();
            try
            {
                List<PurchaseOrderDetail> poDet = InvDet.GetList();
                if (poDet.Exists(p => p.Status == "Approved"))
                    InvHdr.Status = "Approved";
                else
                {
                    InvHdr.Status = "Rejected";
                }

                InvHdr.ApprovalDate = DateTime.Now;
                InvHdr.ApprovedBy = username;
                tmp.Add(InvHdr.GetUpdateCommand(username));
                PurchaseOrderHeaderStatus = InvHdr.Status;
                return tmp;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return tmp;
            }
        }

        //Update PurchaseOrderHeader Status based on PurchaseOrderDetail Status
        public bool UpdateStockOutInRefNo(string StockOutRefNo, string StockInRefNo)
        {
            try
            {
                if (StockOutRefNo != null) InvHdr.Userfld7 = StockOutRefNo;
                if (StockInRefNo != null) InvHdr.Userfld8 = StockInRefNo;
                InvHdr.Save();
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }


        public static bool createPurchaseOrderFromSales(POSController pos, bool fromWeb, out string status)
        {
            status = "";
            try
            {
                // init replenish 
                PurchaseOrderController por = new PurchaseOrderController();
                por.InvHdr.PurchaseOrderHeaderRefNo = getNewPurchaseOrderRefNo((int)pos.myOrderHdr.PointOfSale.Outlet.InventoryLocationID);
                por.InvHdr.PurchaseOrderDate = DateTime.Now;
                por.InvHdr.DateNeededBy = DateTime.Now.AddDays(1);
                por.InvHdr.POType = "Replenish";
                por.InvHdr.Status = "Submitted";
                por.InvHdr.Discount = 0;
                por.InvHdr.InventoryLocationID = (int)pos.myOrderHdr.PointOfSale.Outlet.InventoryLocationID;
                por.InvHdr.RequestedBy = pos.myOrderHdr.CashierID;

                foreach (OrderDet od in pos.myOrderDet)
                {
                    if (!od.IsVoided)
                    {
                        PurchaseOrderDetail pod = new PurchaseOrderDetail();
                        por.AddItemIntoPurchaseOrder(od.ItemNo, od.Quantity.GetValueOrDefault(0).GetIntValue(), od.OrderDetDate.AddYears(100).ToString("yyyy-MM-dd HH:mm:ss"), out status, out pod);
                    }
                }
                string newPurchaseOrderHdrRefNo;
                //string status;

                if (!fromWeb)
                {
                    SyncClientController.Load_WS_URL();
                    PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                    ws.Timeout = 100000;
                    ws.Url = SyncClientController.WS_URL;
                    DataSet myDataSet = new DataSet();
                    myDataSet.Tables.Add(por.InvHdrToDataTable());
                    myDataSet.Tables.Add(por.InvDetToDataTable());
                    byte[] data = SyncClientController.CompressDataSetToByteArray(myDataSet);

                    if (ws.PurchaseOrderCompressed
                        (data,
                        UserInfo.username,
                        (int)pos.myOrderHdr.PointOfSale.Outlet.InventoryLocationID,
                        false, out newPurchaseOrderHdrRefNo, out status))
                    {
                        //download inventoryhdr and inventorydet                        
                        if (SyncClientController.GetCurrentPurchaseOrder())
                        {
                            //isSuccess = true;
                            status = "";
                            return true;
                        }
                        else
                        {
                            Logger.writeLog("Unable to download data from server: " + status);
                            return false;
                        }
                    }


                }
                else
                {
                    if (por.CreateOrder(UserInfo.username,
                            (int)pos.myOrderHdr.PointOfSale.Outlet.InventoryLocationID,
                            out status))
                    {
                        //InventoryController.AssignStockOutToConfirmedOrderUsingTransactionScope();
                        //pnlLoading.Visible = false;
                        newPurchaseOrderHdrRefNo = por.GetPurchaseOrderHeaderRefNo();
                        //isSuccess = true;
                        //MessageBox.Show("Purchase Order successful");
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }

                /*por.InvHdr.Save();
                por.InvDet.SaveAll();*/

                return true;
            }
            catch (Exception ex) { Logger.writeLog(ex.Message); status = ex.Message; return false; }
        }
        // Obsolete Function From Version 3.6.6.0
        /*public static bool PurchaseOrderApproval(string PurchaseOrderHeaderRefNo, bool autoStockIn, out string status, out string BackOrderNo)
        {
            try
            {
                QueryCommandCollection col = new QueryCommandCollection();
                BackOrderNo = "";
                PurchaseOrderController por = new PurchaseOrderController(PurchaseOrderHeaderRefNo);

                //init Back Order 
                PurchaseOrderController backOrder = new PurchaseOrderController();

                //check for existing BackOrder for this inventorylocation
                PurchaseOrderHeaderCollection bCol = new PurchaseOrderHeaderCollection();
                bCol.Where(PurchaseOrderHeader.Columns.Userfld1, "Submitted");
                bCol.Where(PurchaseOrderHeader.Columns.Userfld2, "Back Order");
                bCol.Where(PurchaseOrderHeader.Columns.InventoryLocationID, por.GetInventoryLocationID());
                bCol.Load();

                if (bCol.Count > 0)
                {
                    backOrder = new PurchaseOrderController(bCol[0].PurchaseOrderHeaderRefNo);
                }
                else
                {
                    backOrder.InvHdr.PurchaseOrderHeaderRefNo = getNewBackOrderPurchaseOrderRefNo(por.GetInventoryLocationID());
                    backOrder.InvHdr.PurchaseOrderDate = DateTime.Now;
                    backOrder.InvHdr.DateNeededBy = DateTime.Now.AddDays(1);
                    backOrder.InvHdr.POType = "Back Order";
                    backOrder.InvHdr.Status = "Submitted";
                    backOrder.InvHdr.Discount = 0;
                    backOrder.InvHdr.InventoryLocationID = por.GetInventoryLocationID();
                    backOrder.InvHdr.RequestedBy = por.GetPOHeader().RequestedBy;
                    backOrder.InvHdr.Userfld10 = por.GetPOHeader().Userfld10;
                }


                if (por == null || por.GetPurchaseOrderHeaderRefNo() == "")
                {
                    status = "Purchase Order Not Exist";
                    return false;
                }
                //string status = "";
                string poHdrStatus = "";

                if (InventoryLocationController.IsDeleted(por.GetInventoryLocationID()))
                {
                    status = ERR_CLINIC_DELETED;
                    return false;
                }

                if (StockTakeController.IsInventoryLocationFrozen(por.GetInventoryLocationID()))
                {
                    status = ERR_CLINIC_FROZEN;
                    return false;
                }

                PurchaseOrderHeader poHdr = por.GetPOHeader();
                if (poHdr.Status != "Submitted")
                {
                    status = "Cannot change this document's status because it is already " + poHdr.Status;
                    return false;
                }

                string poType = por.GetPOHeader().POType.ToUpper();

                foreach (PurchaseOrderDetail od in por.GetPODetail())
                {

                    decimal whBal = GetWarehouseBalance(od.ItemNo, DateTime.Now, out status);
                    PurchaseOrderDetail pod = new PurchaseOrderDetail();
                    if (od.Quantity <= whBal)
                    {// add all quantity into replenish purchase order
                        od.QtyApproved = od.Quantity ?? 0;
                        SalesOrderMappingController.ApproveAllSalesOrderMapping(od.PurchaseOrderDetailRefNo);
                    }
                    else
                    {
                        //if (poType.ToLower() == "replenish")
                        //{
                        //add wh bal quantity in the replenish if > 0 and set the rest in the back order
                        if (whBal <= 0)
                        {
                            od.QtyApproved = 0;
                            if (backOrder.AddItemIntoPurchaseOrder(od.ItemNo, od.Quantity ?? 0, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd HH:mm:ss"), od.PurchaseOrderDetailRefNo, out status, out pod))
                            {
                                SalesOrderMappingController.DistributeBackOrderSalesMapping(od.PurchaseOrderDetailRefNo, pod.PurchaseOrderDetailRefNo, od.Quantity ?? 0);
                            }
                        }
                        else
                        {
                            od.QtyApproved = whBal;
                            //por.AddItemIntoPurchaseOrder(od.ItemNo, whBal, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd HH:mm:ss"), od.Userfld6, out status, out pod);
                            if (backOrder.AddItemIntoPurchaseOrder(od.ItemNo, (od.Quantity ?? 0) - whBal, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd HH:mm:ss"), od.PurchaseOrderDetailRefNo, out status, out pod))
                            {
                                SalesOrderMappingController.DistributeBackOrderSalesMapping(od.PurchaseOrderDetailRefNo, pod.PurchaseOrderDetailRefNo, (od.Quantity ?? 0) - whBal);
                            }
                        }
                        BackOrderNo = backOrder.GetPurchaseOrderHeaderRefNo();
                        
                    }
                    if (od.QtyApproved > 0)
                        od.Status = "Approved";
                    else
                        od.Status = "Rejected";
                    col.Add(od.GetUpdateCommand(UserInfo.username));


                }
                DataService.ExecuteTransaction(col);
                por.UpdateHeaderStatus(out poHdrStatus, out status, "SYSTEM");

                if (poHdr.Status == "Approved")
                {
                    int invLocID = new InventoryLocation("InventoryLocationName", ConfigurationManager.AppSettings["WarehouseName"]).InventoryLocationID;
                    
                    StockOutFromPurchaseOrderHeaderRefNo(por.GetPurchaseOrderHeaderRefNo(), UserInfo.username, 0, invLocID, false, false, "");
                    if (autoStockIn)
                    {
                        StockInFromPurchaseOrderHeaderRefNo(por.GetPurchaseOrderHeaderRefNo(), UserInfo.username, 0, por.GetInventoryLocationID(), false, false, "");
                    }
                    //}
                    //}
                }

                return true;
            }
            catch (Exception ex) { Logger.writeLog(ex.Message); status = ex.Message; BackOrderNo = ""; return false; }
        }*/

        //* Purchase order approval For wholesale with Reject Qty*/
        public static bool PurchaseOrderApprovalXY(string PurchaseOrderHeaderRefNo, bool autoStockIn, out string status, out string BackOrderNo)
        {
            try
            {
                QueryCommandCollection col = new QueryCommandCollection();
                BackOrderNo = "";
                PurchaseOrderController por = new PurchaseOrderController(PurchaseOrderHeaderRefNo);
                bool isTransactionSuccess = true;
                //init Back Order 
                PurchaseOrderController backOrder = new PurchaseOrderController();

                //check for existing BackOrder for this inventorylocation
                PurchaseOrderHeaderCollection bCol = new PurchaseOrderHeaderCollection();
                bCol.Where(PurchaseOrderHeader.Columns.Userfld1, "Submitted");
                bCol.Where(PurchaseOrderHeader.Columns.Userfld2, "Back Order");
                bCol.Where(PurchaseOrderHeader.Columns.InventoryLocationID, por.GetInventoryLocationID());
                bCol.Load();

                if (bCol.Count > 0)
                {
                    backOrder = new PurchaseOrderController(bCol[0].PurchaseOrderHeaderRefNo);
                }
                else
                {
                    backOrder.InvHdr.PurchaseOrderHeaderRefNo = getNewBackOrderPurchaseOrderRefNo(por.GetInventoryLocationID());
                    backOrder.InvHdr.PurchaseOrderDate = DateTime.Now;
                    backOrder.InvHdr.DateNeededBy = DateTime.Now.AddDays(1);
                    backOrder.InvHdr.POType = "Back Order";
                    backOrder.InvHdr.Status = "Submitted";
                    backOrder.InvHdr.Discount = 0;
                    backOrder.InvHdr.InventoryLocationID = por.GetInventoryLocationID();
                    backOrder.InvHdr.RequestedBy = por.GetPOHeader().RequestedBy;
                    backOrder.InvHdr.Userfld10 = por.GetPOHeader().Userfld10;
                }


                if (por == null || por.GetPurchaseOrderHeaderRefNo() == "")
                {
                    status = "Purchase Order Not Exist";
                    return false;
                }
                //string status = "";
                string poHdrStatus = "";

                if (InventoryLocationController.IsDeleted(por.GetInventoryLocationID()))
                {
                    status = ERR_CLINIC_DELETED;
                    return false;
                }

                if (StockTakeController.IsInventoryLocationFrozen(por.GetInventoryLocationID()))
                {
                    status = ERR_CLINIC_FROZEN;
                    return false;
                }

                PurchaseOrderHeader poHdr = por.GetPOHeader();
                if (poHdr.Status != "Submitted")
                {
                    status = "Cannot change this document's status because it is already " + poHdr.Status;
                    return false;
                }

                string poType = por.GetPOHeader().POType.ToUpper();

                //using (var ts = new System.Transactions.TransactionScope())
                //{
                    
                    foreach (PurchaseOrderDetail od in por.GetPODetail())
                    {
                        if (poType.ToLower() == "replenish" || poType.ToLower() == "back order" || poType.ToLower() == "special order")
                        {
                            decimal whBal = GetWarehouseBalance(od.ItemNo, DateTime.Now, out status);
                            PurchaseOrderDetail pod = new PurchaseOrderDetail();
                            if (od.Quantity <= whBal)
                            {// add all quantity into replenish purchase order
                                od.QtyApproved = od.Quantity ?? 0;

                                if (od.RejectQty > 0)
                                {
                                    if (backOrder.AddItemIntoPurchaseOrder(od.ItemNo, od.RejectQty, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd HH:mm:ss"), od.PurchaseOrderDetailRefNo, out status, out pod))
                                    {
                                        col.AddRange(SalesOrderMappingController.DistributeBackOrderSalesMappingToQueryCommand(od.PurchaseOrderDetailRefNo, pod.PurchaseOrderDetailRefNo, od.RejectQty));
                                    }
                                }
                                else
                                {
                                    QueryCommandCollection orderMappingCol = new QueryCommandCollection();
                                    if (!SalesOrderMappingController.ApproveAllSalesOrderMappingToQueryCommand(od.PurchaseOrderDetailRefNo, out orderMappingCol))
                                        isTransactionSuccess = false;

                                    col.AddRange(orderMappingCol);
                                }
                            }
                            else
                            {   //If Ordered Qty > Warehouse Balance 
                                // Check if WH <= 0 then set all to backorder  
                                if (whBal <= 0)
                                {
                                    od.QtyApproved = 0;
                                    if (backOrder.AddItemIntoPurchaseOrder(od.ItemNo, (od.Quantity ?? 0) + od.RejectQty, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd HH:mm:ss"), od.PurchaseOrderDetailRefNo, out status, out pod))
                                    {
                                        col.AddRange(SalesOrderMappingController.DistributeBackOrderSalesMappingToQueryCommand(od.PurchaseOrderDetailRefNo, pod.PurchaseOrderDetailRefNo, (od.Quantity ?? 0) + od.RejectQty));
                                    }
                                }
                                else
                                {
                                    // Check if WH <= 0 then set all to backorder  
                                    od.QtyApproved = whBal;

                                    if (backOrder.AddItemIntoPurchaseOrder(od.ItemNo, (od.Quantity ?? 0) - whBal + od.RejectQty, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd HH:mm:ss"), od.PurchaseOrderDetailRefNo, out status, out pod))
                                    {
                                        col.AddRange(SalesOrderMappingController.DistributeBackOrderSalesMappingToQueryCommand(od.PurchaseOrderDetailRefNo, pod.PurchaseOrderDetailRefNo, (od.Quantity ?? 0) - whBal + od.RejectQty));
                                    }
                                }
                                BackOrderNo = backOrder.GetPurchaseOrderHeaderRefNo();

                            }
                            if (od.QtyApproved > 0)
                                od.Status = "Approved";
                            else
                                od.Status = "Rejected";
                            col.Add(od.GetUpdateCommand(UserInfo.username));
                        }
                        else if (poType == "RETURN" || poType == "ADJUSTMENT OUT")
                        {
                            Item itm = new Item(od.ItemNo);
                            decimal divider = 1;
                            if (itm.NonInventoryProduct)
                            {
                                divider = (itm.DeductConvType ? (1 / itm.DeductConvRate == 0 ? 1 : itm.DeductConvRate) : (itm.DeductConvRate));
                            }

                            string ItemNo = itm.NonInventoryProduct ? itm.DeductedItem : itm.ItemNo;

                            int stockBalance = InventoryController.GetStockBalanceQtyByItem(ItemNo, por.GetInventoryLocationID(), out status).GetIntValue();

                            //decimal totalQtyApproved = dataPODetColl.Where(d => d.ItemNo == poDet.ItemNo).Sum(d => d.QtyApproved);
                            if ((od.QtyApproved * divider) > stockBalance)
                            {
                                od.Status = "Rejected";
                                status = "Item " + od.Item.ItemName +  " have stock balance lower than Approved Qty ";
                                return false;
                            }
                            else
                            {
                                od.Status = "Approved";
                                col.Add(od.GetUpdateCommand(UserInfo.username));
                            }
                        }
                    }
                    //DataService.ExecuteTransaction(col);
                    col.AddRange(por.UpdateHeaderStatusToQueryCommand(out poHdrStatus, out status, "SYSTEM"));
                        //isTransactionSuccess = false;

                    if ((poType.ToLower() == "replenish" || poType.ToLower() == "back order" || poType.ToLower() == "special order") && poHdr.Status == "Approved")
                    {
                        poHdr.IsAutoStockIn = autoStockIn;
                        col.Add(poHdr.GetUpdateCommand("SYSTEM"));

                        int invLocID = new InventoryLocation("InventoryLocationName", ConfigurationManager.AppSettings["WarehouseName"]).InventoryLocationID;
                        QueryCommandCollection stockOutQcc = new QueryCommandCollection();
                        QueryCommandCollection stockInQcc = new QueryCommandCollection();
                        if (!StockOutFromPurchaseOrderHeaderRefNo(por.GetPurchaseOrderHeaderRefNo(), UserInfo.username, 0, invLocID, false, false, "", out stockOutQcc))
                        {
                            isTransactionSuccess = false;
                        }
                        if (autoStockIn && isTransactionSuccess)
                        {
                            if (!StockInFromPurchaseOrderHeaderRefNo(por.GetPurchaseOrderHeaderRefNo(), UserInfo.username, 0, por.GetInventoryLocationID(), false, false, "", out stockInQcc))
                                isTransactionSuccess = false;
                        }
                        //combine qmc 
                        
                        col.AddRange(stockOutQcc);
                        col.AddRange(stockInQcc);
                    }
                    else if (poType == "RETURN" && poHdr.Status.ToUpper() == "APPROVED")
                    {
                        int invLocID = new InventoryLocation("InventoryLocationName", ConfigurationManager.AppSettings["WarehouseName"]).InventoryLocationID;
                        ArrayList list = new ArrayList();
                        foreach (var poDet in por.GetPODetail())
                        {
                            if (poDet.QtyApproved > 0 && poDet.Status == "Approved")
                                list.Add(new { ItemNo = poDet.ItemNo, Quantity = poDet.QtyApproved });
                        }
                        if (invLocID != 0 && list.Count > 0)
                        {
                            // Do Stock In for warehouse
                            //string stockInRefNo = "";
                            string dataStockIn = new JavaScriptSerializer().Serialize(list);
                            QueryCommandCollection stockincmd = new QueryCommandCollection();
                            if (StockInFromPurchaseOrderHeader(por, "SYSTEM", 0, invLocID, false, true, "Order Approval: " + poHdr.PurchaseOrderHeaderRefNo, out stockincmd))
                            {
                                col.AddRange(stockincmd);
                            }

                            //Update Header Ref No
                            //por.UpdateStockOutInRefNo(null, stockInRefNo);
                        }
                    }

                    DataService.ExecuteTransaction(col);
                    
                //}
                if (!isTransactionSuccess)
                    return false;
                return true; 
            }
            catch (Exception ex) { 
                Logger.writeLog(ex.Message); 
                status = ex.Message; BackOrderNo = ""; 
                return false; }
        }

        public bool IsSerialNoValid(List<string> listInvDet, out string message)
        {
            bool isValid = true;
            message = "";

            if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.EnableProductSerialNo), false))
                return true;

            try
            {
                foreach (var det in InvDet)
                {
                    if (!listInvDet.Contains(det.PurchaseOrderDetailRefNo))
                        continue;

                    var item = det.Item;
                    var serialNo = det.SerialNo;
                    if (item.IsUseSerialNo && String.IsNullOrEmpty(serialNo))
                    {
                        isValid = false;
                        message += string.Format("Item {0} requires to enter serial no", item.ItemName);
                        message += Environment.NewLine;
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                Logger.writeLog(ex);
            }

            return isValid;
        }

        public bool IsSerialNoExist(string itemNo, List<string> _serialNo, out string message)
        {
            bool isValid = true;
            message = "";

            if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.EnableProductSerialNo), false))
                return true;

            try
            {
                foreach (var det in InvDet)
                {
                    if (itemNo != det.ItemNo)
                        continue;

                    foreach (string s in _serialNo)
                    {
                        if (!det.SerialNo.Contains(s))
                        {
                            message = "Serial No not found in Order";
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                Logger.writeLog(ex);
            }

            return true;
        }

        public static bool PurchaseOrderApprovalXY(PurchaseOrderController por, string PurchaseOrderHeaderRefNo, bool autoStockIn, out string status, out string BackOrderNo)
        {
            try
            {
                QueryCommandCollection col = new QueryCommandCollection();
                BackOrderNo = "";
                //PurchaseOrderController por = new PurchaseOrderController(PurchaseOrderHeaderRefNo);
                bool isTransactionSuccess = true;
                //init Back Order 
                PurchaseOrderController backOrder = new PurchaseOrderController();

                //check for existing BackOrder for this inventorylocation
                PurchaseOrderHeaderCollection bCol = new PurchaseOrderHeaderCollection();
                bCol.Where(PurchaseOrderHeader.Columns.Userfld1, "Submitted");
                bCol.Where(PurchaseOrderHeader.Columns.Userfld2, "Back Order");
                bCol.Where(PurchaseOrderHeader.Columns.InventoryLocationID, por.GetInventoryLocationID());
                bCol.Load();

                if (por.InvHdr.POType == "Back Order")
                {
                    backOrder.InvHdr.PurchaseOrderHeaderRefNo = getNewBackOrderPurchaseOrderRefNo(por.GetInventoryLocationID());
                    backOrder.InvHdr.PurchaseOrderDate = DateTime.Now;
                    backOrder.InvHdr.DateNeededBy = DateTime.Now.AddDays(1);
                    backOrder.InvHdr.POType = "Back Order";
                    backOrder.InvHdr.Status = "Submitted";
                    backOrder.InvHdr.Discount = 0;
                    backOrder.InvHdr.InventoryLocationID = por.GetInventoryLocationID();
                    backOrder.InvHdr.RequestedBy = por.GetPOHeader().RequestedBy;
                    backOrder.InvHdr.Userfld10 = por.GetPOHeader().Userfld10;
                }
                else
                {
                    if (bCol.Count > 0)
                    {
                        backOrder = new PurchaseOrderController(bCol[0].PurchaseOrderHeaderRefNo);
                    }
                    else
                    {
                        backOrder.InvHdr.PurchaseOrderHeaderRefNo = getNewBackOrderPurchaseOrderRefNo(por.GetInventoryLocationID());
                        backOrder.InvHdr.PurchaseOrderDate = DateTime.Now;
                        backOrder.InvHdr.DateNeededBy = DateTime.Now.AddDays(1);
                        backOrder.InvHdr.POType = "Back Order";
                        backOrder.InvHdr.Status = "Submitted";
                        backOrder.InvHdr.Discount = 0;
                        backOrder.InvHdr.InventoryLocationID = por.GetInventoryLocationID();
                        backOrder.InvHdr.RequestedBy = por.GetPOHeader().RequestedBy;
                        backOrder.InvHdr.Userfld10 = por.GetPOHeader().Userfld10;
                    }
                }


                if (por == null || por.GetPurchaseOrderHeaderRefNo() == "")
                {
                    status = "Purchase Order Not Exist";
                    return false;
                }
                //string status = "";
                string poHdrStatus = "";

                if (InventoryLocationController.IsDeleted(por.GetInventoryLocationID()))
                {
                    status = ERR_CLINIC_DELETED;
                    return false;
                }

                if (StockTakeController.IsInventoryLocationFrozen(por.GetInventoryLocationID()))
                {
                    status = ERR_CLINIC_FROZEN;
                    return false;
                }

                PurchaseOrderHeader poHdr = por.GetPOHeader();
                if (poHdr.Status != "Submitted")
                {
                    status = "Cannot change this document's status because it is already " + poHdr.Status;
                    return false;
                }

                string poType = por.GetPOHeader().POType.ToUpper();

                //using (var ts = new System.Transactions.TransactionScope())
                //{

                bool AllowDeductInvQtyNotSuffice = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.AllowDeductInvQtyNotSufficient), false);
                List<string> listInvDet = new List<string>();
                foreach (PurchaseOrderDetail od in por.GetPODetail())
                {
                    listInvDet.Add(od.PurchaseOrderDetailRefNo);
                    od.GSTRule = GetGSTRuleDefaultWebOrdering();
                    //exclusive gst
                    decimal amount = (od.Quantity.GetValueOrDefault(0) * od.FactoryPrice.GetValueOrDefault(0));
                    if (od.GSTRule == 1)
                    {
                        od.GSTAmount = amount * (decimal)0.07;
                    }
                    else if (od.GSTRule == 2)
                    {
                        // inclusive
                        od.GSTAmount = amount * (decimal)0.07 / (decimal)1.07;
                    }
                    else
                    {
                        // no gst
                        od.GSTAmount = 0;
                    }
                    

                    if (poType.ToLower() == "replenish" || poType.ToLower() == "back order" || poType.ToLower() == "special order")
                    {
                        // if not from warehouse then we ignore the warehouse balance by setting it to maxvalue
                        if (!AllowDeductInvQtyNotSuffice)
                        {
                            decimal whBal = (por.GetPOHeader().WarehouseID == 0) ? decimal.MaxValue : GetWarehouseBalanceByLocID(por.GetPOHeader().WarehouseID, od.ItemNo, DateTime.Now, out status);

                            PurchaseOrderDetail pod = new PurchaseOrderDetail();
                            if (od.Quantity <= whBal)
                            {// add all quantity into replenish purchase order
                                od.QtyApproved = od.Quantity ?? 0;

                                if (od.RejectQty > 0)
                                {
                                    if (backOrder.AddItemIntoPurchaseOrder(od.ItemNo, od.RejectQty, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd HH:mm:ss"), od.PurchaseOrderDetailRefNo, out status, out pod))
                                    {
                                        col.AddRange(SalesOrderMappingController.DistributeBackOrderSalesMappingToQueryCommand(od.PurchaseOrderDetailRefNo, pod.PurchaseOrderDetailRefNo, od.RejectQty));
                                    }
                                }
                                else
                                {
                                    QueryCommandCollection orderMappingCol = new QueryCommandCollection();
                                    if (!SalesOrderMappingController.ApproveAllSalesOrderMappingToQueryCommand(od.PurchaseOrderDetailRefNo, out orderMappingCol))
                                        isTransactionSuccess = false;

                                    col.AddRange(orderMappingCol);
                                }
                            }
                            else
                            {   //If Ordered Qty > Warehouse Balance 
                                // Check if WH <= 0 then set all to backorder  
                                if (whBal <= 0)
                                {
                                    od.QtyApproved = 0;
                                    if (backOrder.AddItemIntoPurchaseOrder(od.ItemNo, (od.Quantity ?? 0) + od.RejectQty, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd HH:mm:ss"), od.PurchaseOrderDetailRefNo, out status, out pod))
                                    {
                                        col.AddRange(SalesOrderMappingController.DistributeBackOrderSalesMappingToQueryCommand(od.PurchaseOrderDetailRefNo, pod.PurchaseOrderDetailRefNo, (od.Quantity ?? 0) + od.RejectQty));
                                    }
                                }
                                else
                                {
                                    // Check if WH <= 0 then set all to backorder  
                                    od.QtyApproved = whBal;

                                    if (backOrder.AddItemIntoPurchaseOrder(od.ItemNo, (od.Quantity ?? 0) - whBal + od.RejectQty, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd HH:mm:ss"), od.PurchaseOrderDetailRefNo, out status, out pod))
                                    {
                                        col.AddRange(SalesOrderMappingController.DistributeBackOrderSalesMappingToQueryCommand(od.PurchaseOrderDetailRefNo, pod.PurchaseOrderDetailRefNo, (od.Quantity ?? 0) - whBal + od.RejectQty));
                                    }
                                }
                                BackOrderNo = backOrder.GetPurchaseOrderHeaderRefNo();

                            }
                            if (od.QtyApproved > 0)
                                od.Status = "Approved";
                            else
                                od.Status = "Rejected";
                        }
                        else {
                            if (od.Status != "Rejected")
                            {
                                PurchaseOrderDetail pod = new PurchaseOrderDetail();
                                od.QtyApproved = od.Quantity.GetValueOrDefault(0);

                                if (od.RejectQty > 0)
                                {
                                    if (backOrder.AddItemIntoPurchaseOrder(od.ItemNo, od.RejectQty, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd HH:mm:ss"), od.PurchaseOrderDetailRefNo, out status, out pod))
                                    {
                                        col.AddRange(SalesOrderMappingController.DistributeBackOrderSalesMappingToQueryCommand(od.PurchaseOrderDetailRefNo, pod.PurchaseOrderDetailRefNo, od.RejectQty));
                                    }
                                }
                                else
                                {
                                    QueryCommandCollection orderMappingCol = new QueryCommandCollection();
                                    if (!SalesOrderMappingController.ApproveAllSalesOrderMappingToQueryCommand(od.PurchaseOrderDetailRefNo, out orderMappingCol))
                                        isTransactionSuccess = false;

                                    col.AddRange(orderMappingCol);
                                }

                                od.Status = "Approved";
                            }
                        }
                        
                        col.Add(od.GetUpdateCommand(UserInfo.username));
                    }
                    else if (poType == "RETURN" || poType == "ADJUSTMENT OUT")
                    {
                        Item itm = new Item(od.ItemNo);
                        decimal divider = 1;
                        if (itm.NonInventoryProduct)
                        {
                            divider = (itm.DeductConvType ? (1 / itm.DeductConvRate == 0 ? 1 : itm.DeductConvRate) : (itm.DeductConvRate));
                        }

                        string ItemNo = itm.NonInventoryProduct ? itm.DeductedItem : itm.ItemNo;


                        int stockBalance = InventoryController.GetStockBalanceQtyByItemSummaryByDate(ItemNo, por.GetInventoryLocationID(), DateTime.Now, out status).GetIntValue();
                        
                        //decimal totalQtyApproved = dataPODetColl.Where(d => d.ItemNo == poDet.ItemNo).Sum(d => d.QtyApproved);
                        if (!AllowDeductInvQtyNotSuffice && (od.QtyApproved * divider) > stockBalance)
                        {
                            od.Status = "Rejected";
                            status = "Item " + od.Item.ItemName + " have stock balance lower than Approved Qty ";
                            return false;
                        }
                        else
                        {
                            od.Status = "Approved";
                            col.Add(od.GetUpdateCommand(UserInfo.username));
                        }
                    }
                }
                //DataService.ExecuteTransaction(col);
                col.AddRange(por.UpdateHeaderStatusToQueryCommand(out poHdrStatus, out status, "SYSTEM"));
                //isTransactionSuccess = false;
                string msgSerialNo = "";
                if (!por.IsSerialNoValid(listInvDet, out msgSerialNo))
                {
                    status = "Cannot Approve Goods Ordering. " + msgSerialNo;
                    return false;
                }



                if ((poType.ToLower() == "replenish" || poType.ToLower() == "back order" || poType.ToLower() == "special order") && poHdr.Status == "Approved" )
                {
                    col.AddRange(por.SetAutoGenerateInvoiceNo("SYSTEM", out status));

                    poHdr.IsAutoStockIn = autoStockIn;
                    col.Add(poHdr.GetUpdateCommand("SYSTEM"));

                    int invLocID = new InventoryLocation("InventoryLocationName", ConfigurationManager.AppSettings["WarehouseName"]).InventoryLocationID;
                    QueryCommandCollection stockOutQcc = new QueryCommandCollection();
                    QueryCommandCollection stockInQcc = new QueryCommandCollection();
                    
                    if (poHdr.WarehouseID != 0)
                    {
                        if (!StockOutFromPurchaseOrderHeader(por, UserInfo.username, 0, poHdr.WarehouseID, false, false, "", out stockOutQcc))
                        {
                            isTransactionSuccess = false;
                        }
                    }
                    if (stockOutQcc.Count == 0)
                        throw new Exception("Failed to stock out from warehouse");

                    if (autoStockIn && isTransactionSuccess)
                    {
                        Logger.writeLog("Calling StockInFromPurchaseOrderHeader, autoStockIn: " + autoStockIn.ToString());
                        if (!StockInFromPurchaseOrderHeader(por, UserInfo.username, 0, por.GetInventoryLocationID(), false, false, "", out stockInQcc))
                            isTransactionSuccess = false;
                    }
                    //combine qmc 
                     if (stockInQcc.Count == 0)
                            throw new Exception("Failed to do auto stock in");

                    col.AddRange(stockOutQcc);
                    col.AddRange(stockInQcc);
                }
                else if (poType == "RETURN" && poHdr.Status.ToUpper() == "APPROVED")
                {
                    #region *) OBSOLETE
                    //int invLocID = new InventoryLocation("InventoryLocationName", ConfigurationManager.AppSettings["WarehouseName"]).InventoryLocationID;
                    //ArrayList list = new ArrayList();
                    //foreach (var poDet in por.GetPODetail())
                    //{
                    //    if (poDet.QtyApproved > 0 && poDet.Status == "Approved")
                    //        list.Add(new { ItemNo = poDet.ItemNo, Quantity = poDet.QtyApproved });
                    //}

                    //if (invLocID != 0 && list.Count > 0)
                    //{
                    //    // Do Stock In for warehouse
                    //    //string stockInRefNo = "";
                    //    string dataStockIn = new JavaScriptSerializer().Serialize(list);
                    //    QueryCommandCollection stockincmd = new QueryCommandCollection();
                    //    if (StockInFromPurchaseOrderHeader(por,"SYSTEM", 0, invLocID, false, true, "Order Approval: " + poHdr.PurchaseOrderHeaderRefNo, out stockincmd))
                    //    {
                    //        col.AddRange(stockincmd);
                    //    }
                        
                    //    //Update Header Ref No
                    //    //por.UpdateStockOutInRefNo(null, stockInRefNo);
                    //}
                    #endregion

                    //if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.StockReturnWillReturnStockToWarehouse), false))
                    //{
                    //    // Do Stock Return In for the warehouse

                    //    PowerPOS.Container.CostingMethods CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
                    //    string strCostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
                    //    if (strCostingMethod.ToLower() == "fifo")
                    //        CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
                    //    else if (strCostingMethod.ToLower() == "fixed avg")
                    //        CostingMethod = PowerPOS.Container.CostingMethods.FixedAvg;
                    //    else
                    //        CostingMethod = PowerPOS.Container.CostingMethods.FIFO;

                    //    foreach (PurchaseOrderDetail poDet in por.GetPODetail())
                    //    {
                    //        int? supplierID = ItemSupplierMapController.GetPreferredSupplier(poDet.ItemNo);
                    //        if (!supplierID.HasValue) continue;

                    //        Supplier s = new Supplier(supplierID.Value);
                    //        if (!s.IsWarehouse.GetValueOrDefault(false) || !s.WarehouseID.HasValue) continue;

                    //        int invLocID = s.WarehouseID.Value;

                    //        InventoryController ctrl = new InventoryController(CostingMethod);
                    //        ctrl.SetInventoryHeaderInfo(PurchaseOrderHeaderRefNo, "", "Order Approval: " + poHdr.PurchaseOrderHeaderRefNo, 0, 0, 0);

                    //        if (!(poDet.QtyApproved > 0 && poDet.Status == "Approved")) continue;
                    //        ctrl.AddItemIntoInventoryStockReturn(poDet.ItemNo, poDet.Quantity.GetValueOrDefault(0), poDet.FactoryPrice.GetValueOrDefault(0), false, out status);
                            
                    //        if (status == "")
                    //        {
                    //            col.AddRange(ctrl.CreateReturnInQueryCommand(UserInfo.username, invLocID, false, true));
                    //        }
                    //    }
                    //}
                }

                //#region *) Save Price Level in the Inventory Location for next transaction
                //if (!string.IsNullOrEmpty(poHdr.PriceLevel) && poHdr.InventoryLocationID.HasValue)
                //{
                //    InventoryLocation iloc = new InventoryLocation(poHdr.InventoryLocationID.Value);
                //    if (iloc != null && iloc.InventoryLocationID == poHdr.InventoryLocationID.Value)
                //    {
                //        iloc.DefaultPriceLevel = poHdr.PriceLevel;
                //        col.Add(iloc.GetUpdateCommand(UserInfo.username));
                //    }
                //}
                //#endregion

                DataService.ExecuteTransaction(col);
                //}
                if (!isTransactionSuccess)
                    return false;


                #region *) Auto Create Supplier PO
                if ((poType.ToLower() == "replenish") && (poHdrStatus.ToUpper() == "APPROVED") 
                    && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.AutoCreateSupplierPOUponOutletOrderApproval), false))
                {
                    if (poHdr.SupplierID.GetValueOrDefault(0) == 0)
                    {
                        // Supplier = ALL, then create PO based on Item Supplier Map for each supplier

                        List<PurchaseOdrController> invCtrlList = new List<PurchaseOdrController>();

                        List<PurchaseOrderDetail> detColl = por.GetPODetail().Where(d => d.Status.ToUpper() == "APPROVED").ToList();
                        foreach (PurchaseOrderDetail poDet in detColl)
                        {
                            int? supplierID = ItemSupplierMapController.GetPreferredSupplier(poDet.ItemNo);

                            if (supplierID.HasValue)
                            {
                                bool invCtrlExists = false;

                                //PurchaseOdrController invCtrl = invCtrlList.Where(po => po.InvHdr.Supplier == supplierID.ToString()).First();
                                PurchaseOdrController invCtrl = new PurchaseOdrController();
                                for (int i = 0; i < invCtrlList.Count; i++)
                                {
                                    if (invCtrlList[i].InvHdr.Supplier == supplierID.Value.ToString())
                                    {
                                        invCtrl = invCtrlList[i];
                                        invCtrlExists = true;
                                        break;
                                    }
                                }

                                if (!invCtrlExists)
                                {
                                    Supplier s = new Supplier(supplierID.Value);

                                    invCtrl = new PurchaseOdrController();
                                    invCtrl.SetInventoryLocation(poHdr.InventoryLocationID.GetValueOrDefault(0));
                                    invCtrl.SetPurchaseOrderHeaderInfo(supplierID.Value.ToString(), "", 0, 1, 0);
                                    invCtrl.InvHdr.GSTType = string.IsNullOrEmpty(s.GSTRule) ? "0" : s.GSTRule;
                                    invCtrlList.Add(invCtrl);
                                }

                                string packingSizeName = "";
                                decimal costPrice = 0;

                                Item theItem = new Item(poDet.ItemNo);
                                if (theItem != null && theItem.ItemNo == poDet.ItemNo)
                                    packingSizeName = theItem.UOM;

                                //Query qr = new Query("ItemSupplierMap");
                                //qr.AddWhere(ItemSupplierMap.Columns.ItemNo, poDet.ItemNo);
                                //qr.AddWhere(ItemSupplierMap.Columns.SupplierID, supplierID.Value);
                                //ItemSupplierMap ism = new ItemSupplierMapController().FetchByQuery(qr).FirstOrDefault();
                                //if (ism != null)
                                //    costPrice = ism.CostPrice;
                                //else
                                //    costPrice = theItem.FactoryPrice;

                                costPrice = poDet.FactoryPrice == null ? theItem.FactoryPrice : poDet.FactoryPrice.GetValueOrDefault(0);

                                if (!invCtrl.AddItemToPurchaseOrderByPackingSize(poDet.ItemNo, poDet.QtyApproved, packingSizeName, 1, costPrice, invCtrl.InvHdr.GSTType.GetIntValue(), out status))
                                {
                                    throw new Exception("Error: " + status);
                                }
                            }
                        }

                        foreach (PurchaseOdrController invCtrl in invCtrlList)
                        {
                            invCtrl.InvHdr.CustomRefNo = ""; // Empty it so it will regenerate

                            col = new QueryCommandCollection();
                            col.AddRange(invCtrl.getInsertCommand(UserInfo.username, poHdr.InventoryLocationID.GetValueOrDefault(0), false));

                            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.AutoApproveSupplierPO), false))
                            {
                                if (invCtrl.GetTotalCost() > 0)
                                {
                                    bool updateCostPrice = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.AutoUpdateCostPriceOnSupplierPOApproval), false);
                                    QueryCommandCollection approveCmd;
                                    invCtrl.GetApprovePOAndUpdateCostPriceCommand(updateCostPrice, out status, out approveCmd);
                                    if (approveCmd != null && approveCmd.Count > 0)
                                        col.AddRange(approveCmd);
                                }
                            }

                            DataService.ExecuteTransaction(col);

                            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.UseCustomNo), false))
                            {
                                PurchaseOdrController.CustomRefNoUpdate();
                            }
                        }
                    }
                    else
                    {
                        // Supplier is selected

                        if (poHdr.WarehouseID == 0)
                        {
                            // Not a warehouse, then create PO for this supplier
                            Supplier s = new Supplier(poHdr.SupplierID.GetValueOrDefault(0));

                            PurchaseOdrController invCtrl = new PurchaseOdrController();
                            invCtrl.SetInventoryLocation(poHdr.InventoryLocationID.GetValueOrDefault(0));
                            invCtrl.SetPurchaseOrderHeaderInfo(poHdr.SupplierID.GetValueOrDefault(0).ToString(), "", 0, 1, 0);
                            invCtrl.InvHdr.GSTType = string.IsNullOrEmpty(s.GSTRule) ? "0" : s.GSTRule;
                            invCtrl.SetGoodOrderRefNo(poHdr.PurchaseOrderHeaderRefNo);

                            foreach (PurchaseOrderDetail poDet in por.GetPODetail())
                            {
                                if (poDet.Status.ToUpper() == "APPROVED")
                                {
                                    string packingSizeName = "";
                                    decimal costPrice = 0;

                                    Item theItem = new Item(poDet.ItemNo);
                                    if (theItem != null && theItem.ItemNo == poDet.ItemNo)
                                        packingSizeName = theItem.UOM;

                                    //Query qr = new Query("ItemSupplierMap");
                                    //qr.AddWhere(ItemSupplierMap.Columns.ItemNo, poDet.ItemNo);
                                    //qr.AddWhere(ItemSupplierMap.Columns.SupplierID, poHdr.SupplierID.GetValueOrDefault(0));
                                    //ItemSupplierMap ism = new ItemSupplierMapController().FetchByQuery(qr).FirstOrDefault();
                                    //if (ism != null)
                                    //    costPrice = ism.CostPrice;
                                    //else
                                    //    costPrice = theItem.FactoryPrice;
                                    costPrice = poDet.FactoryPrice == null ? theItem.FactoryPrice : poDet.FactoryPrice.GetValueOrDefault(0);

                                    if (!invCtrl.AddItemToPurchaseOrderByPackingSize(poDet.ItemNo, poDet.QtyApproved, packingSizeName, 1, costPrice, invCtrl.InvHdr.GSTType.GetIntValue(), out status))
                                    {
                                        throw new Exception("Error: " + status);
                                    }
                                }
                            }

                            col = new QueryCommandCollection();
                            col.AddRange(invCtrl.getInsertCommand(UserInfo.username, poHdr.InventoryLocationID.GetValueOrDefault(0), false));

                            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.AutoApproveSupplierPO), false))
                            {
                                if (invCtrl.GetTotalCost() > 0)
                                {
                                    bool updateCostPrice = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.AutoUpdateCostPriceOnSupplierPOApproval), false);
                                    QueryCommandCollection approveCmd;
                                    invCtrl.GetApprovePOAndUpdateCostPriceCommand(updateCostPrice, out status, out approveCmd);
                                    if (approveCmd != null && approveCmd.Count > 0)
                                        col.AddRange(approveCmd);
                                }
                            }

                            DataService.ExecuteTransaction(col);

                            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.UseCustomNo), false))
                            {
                                PurchaseOdrController.CustomRefNoUpdate();
                            }
                        }
                        else
                        {
                            // Warehouse is selected, then no need to create anything
                        }
                    }
                }
                #endregion


                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex.Message);
                status = ex.Message; BackOrderNo = "";
                return false;
            }
        }

        public static bool PurchaseOrderApprovalWithOrderFrom(PurchaseOrderController por, string PurchaseOrderHeaderRefNo, bool autoStockIn, out string status, out string BackOrderNo)
        {
            try
            {
                QueryCommandCollection col = new QueryCommandCollection();
                BackOrderNo = "";
                //PurchaseOrderController por = new PurchaseOrderController(PurchaseOrderHeaderRefNo);
                bool isTransactionSuccess = true;
                //init Back Order 
                PurchaseOrderController backOrder = new PurchaseOrderController();

                string pricelevel = por.GetPOHeader().PriceLevel;
                if (string.IsNullOrEmpty(pricelevel))
                {
                    InventoryLocation inLoc = new InventoryLocation(por.GetInventoryLocationID());

                    pricelevel = inLoc.DefaultPriceLevel;
                }

                //check for existing BackOrder for this inventorylocation
                PurchaseOrderHeaderCollection bCol = new PurchaseOrderHeaderCollection();
                bCol.Where(PurchaseOrderHeader.Columns.Userfld1, "Submitted");
                bCol.Where(PurchaseOrderHeader.Columns.Userfld2, "Back Order");
                bCol.Where(PurchaseOrderHeader.Columns.InventoryLocationID, por.GetInventoryLocationID());
                bCol.Where(PurchaseOrderHeader.Columns.SupplierID, por.getSupplierID());
                bCol.Where(PurchaseOrderHeader.UserColumns.WarehouseID, por.getWarehouseID());
                bCol.Load();

                if (por.InvHdr.POType == "Back Order")
                {
                    backOrder.InvHdr.PurchaseOrderHeaderRefNo = getNewBackOrderPurchaseOrderRefNo(por.GetInventoryLocationID());
                    backOrder.InvHdr.PurchaseOrderDate = DateTime.Now;
                    backOrder.InvHdr.DateNeededBy = DateTime.Now.AddDays(1);
                    backOrder.InvHdr.POType = "Back Order";
                    backOrder.InvHdr.Status = "Submitted";
                    backOrder.InvHdr.Discount = 0;
                    backOrder.InvHdr.InventoryLocationID = por.GetInventoryLocationID();
                    backOrder.InvHdr.RequestedBy = por.GetPOHeader().RequestedBy;
                    backOrder.InvHdr.Userfld10 = por.GetPOHeader().Userfld10;
                    backOrder.InvHdr.SupplierID = por.getSupplierID();
                    backOrder.InvHdr.WarehouseID = por.getWarehouseID();
                    if(string.IsNullOrEmpty(backOrder.InvHdr.PriceLevel))
                        backOrder.InvHdr.PriceLevel = pricelevel;
                }   
                else
                {
                    if (bCol.Count > 0)
                    {
                        backOrder = new PurchaseOrderController(bCol[0].PurchaseOrderHeaderRefNo);

                        if (!string.IsNullOrEmpty(backOrder.InvHdr.PriceLevel))
                            pricelevel = backOrder.InvHdr.PriceLevel;                        
                    }
                    else
                    {
                        backOrder.InvHdr.PurchaseOrderHeaderRefNo = getNewBackOrderPurchaseOrderRefNo(por.GetInventoryLocationID());
                        backOrder.InvHdr.PurchaseOrderDate = DateTime.Now;
                        backOrder.InvHdr.DateNeededBy = DateTime.Now.AddDays(1);
                        backOrder.InvHdr.POType = "Back Order";
                        backOrder.InvHdr.Status = "Submitted";
                        backOrder.InvHdr.Discount = 0;
                        backOrder.InvHdr.InventoryLocationID = por.GetInventoryLocationID();
                        backOrder.InvHdr.RequestedBy = por.GetPOHeader().RequestedBy;
                        backOrder.InvHdr.Userfld10 = por.GetPOHeader().Userfld10;
                        backOrder.InvHdr.SupplierID = por.getSupplierID();
                        backOrder.InvHdr.WarehouseID = por.getWarehouseID();
                        backOrder.InvHdr.PriceLevel = pricelevel;
                    }
                }


                if (por == null || por.GetPurchaseOrderHeaderRefNo() == "")
                {
                    status = "Purchase Order Not Exist";
                    return false;
                }
                //string status = "";
                string poHdrStatus = "";

                if (InventoryLocationController.IsDeleted(por.GetInventoryLocationID()))
                {
                    status = ERR_CLINIC_DELETED;
                    return false;
                }

                if (StockTakeController.IsInventoryLocationFrozen(por.GetInventoryLocationID()))
                {
                    status = ERR_CLINIC_FROZEN;
                    return false;
                }

                PurchaseOrderHeader poHdr = por.GetPOHeader();
                if (poHdr.Status != "Submitted")
                {
                    status = "Cannot change this document's status because it is already " + poHdr.Status;
                    return false;
                }

                string poType = por.GetPOHeader().POType.ToUpper();

                //using (var ts = new System.Transactions.TransactionScope())
                //{

                bool AllowDeductInvQtyNotSuffice = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.AllowDeductInvQtyNotSufficient), false);
                List<string> listInvDet = new List<string>();
                foreach (PurchaseOrderDetail od in por.GetPODetail())
                {
                    listInvDet.Add(od.PurchaseOrderDetailRefNo);

                    if (od.GSTAmount == null)
                    {
                        od.GSTRule = GetGSTRuleDefaultWebOrdering();
                        //exclusive gst
                        decimal amount = (od.Quantity.GetValueOrDefault(0) * od.FactoryPrice.GetValueOrDefault(0));
                        if (od.GSTRule == 1)
                        {                           
                            od.GSTAmount = amount  * (decimal)0.07;
                        }
                        else if (od.GSTRule == 2)
                        {
                            // inclusive
                            od.GSTAmount = amount * (decimal) 0.07 / (decimal) 1.07;
                        }
                        else { 
                            // no gst
                            od.GSTAmount = 0;
                        }
                    }
                    
                    if (poType.ToLower() == "replenish" || poType.ToLower() == "back order" || poType.ToLower() == "special order")
                    {
                        // if not from warehouse then we ignore the warehouse balance by setting it to maxvalue
                        if (!AllowDeductInvQtyNotSuffice)
                        {
                            if (od.Status == "Rejected")
                                continue;                            

                            decimal whBal = (por.GetPOHeader().WarehouseID == 0) ? decimal.MaxValue : GetWarehouseBalanceByLocID(por.GetPOHeader().WarehouseID, od.ItemNo, DateTime.Now, out status);

                            PurchaseOrderDetail pod = new PurchaseOrderDetail();
                            if (od.QtyApproved > od.Quantity)
                            {
                                if (od.QtyApproved <= whBal)
                                {// add all quantity into replenish purchase order

                                    if (od.RejectQty > 0)
                                    {
                                        if (backOrder.AddItemIntoPurchaseOrderWithPriceLevel(od.ItemNo, od.RejectQty, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd HH:mm:ss"), od.PurchaseOrderDetailRefNo, pricelevel, out status, out pod))
                                        {
                                            col.AddRange(SalesOrderMappingController.DistributeBackOrderSalesMappingToQueryCommand(od.PurchaseOrderDetailRefNo, pod.PurchaseOrderDetailRefNo, od.RejectQty));
                                        }
                                    }
                                    else
                                    {
                                        QueryCommandCollection orderMappingCol = new QueryCommandCollection();
                                        if (!SalesOrderMappingController.ApproveAllSalesOrderMappingToQueryCommand(od.PurchaseOrderDetailRefNo, out orderMappingCol))
                                            isTransactionSuccess = false;

                                        col.AddRange(orderMappingCol);
                                    }
                                }
                                else
                                {   //If Ordered Qty > Warehouse Balance 
                                    // Check if WH <= 0 then set all to backorder  
                                    if (whBal <= 0)
                                    {
                                        
                                        if (backOrder.AddItemIntoPurchaseOrderWithPriceLevel(od.ItemNo, od.QtyApproved, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd HH:mm:ss"), od.PurchaseOrderDetailRefNo, pricelevel, out status, out pod))
                                        {
                                            col.AddRange(SalesOrderMappingController.DistributeBackOrderSalesMappingToQueryCommand(od.PurchaseOrderDetailRefNo, pod.PurchaseOrderDetailRefNo, (od.Quantity ?? 0) + od.RejectQty));
                                            od.QtyApproved = 0;
                                        }
                                    }
                                    else
                                    {
                                        // Check if WH <= 0 then set all to backorder  
                                        if (backOrder.AddItemIntoPurchaseOrderWithPriceLevel(od.ItemNo, od.QtyApproved - whBal + od.RejectQty, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd HH:mm:ss"), od.PurchaseOrderDetailRefNo, pricelevel, out status, out pod))
                                        {
                                            col.AddRange(SalesOrderMappingController.DistributeBackOrderSalesMappingToQueryCommand(od.PurchaseOrderDetailRefNo, pod.PurchaseOrderDetailRefNo, (od.Quantity ?? 0) - whBal + od.RejectQty));
                                            od.QtyApproved = whBal;
                                        }
                                    }
                                    BackOrderNo = backOrder.GetPurchaseOrderHeaderRefNo();

                                }
                            }
                            else
                            {

                                if (od.Quantity <= whBal)
                                {// add all quantity into replenish purchase order

                                    //od.QtyApproved = od.Quantity ?? 0;

                                    if (od.RejectQty > 0)
                                    {
                                        if (backOrder.AddItemIntoPurchaseOrderWithPriceLevel(od.ItemNo, od.RejectQty, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd HH:mm:ss"), od.PurchaseOrderDetailRefNo, pricelevel, out status, out pod))
                                        {
                                            col.AddRange(SalesOrderMappingController.DistributeBackOrderSalesMappingToQueryCommand(od.PurchaseOrderDetailRefNo, pod.PurchaseOrderDetailRefNo, od.RejectQty));
                                        }
                                    }
                                    else
                                    {
                                        QueryCommandCollection orderMappingCol = new QueryCommandCollection();
                                        if (!SalesOrderMappingController.ApproveAllSalesOrderMappingToQueryCommand(od.PurchaseOrderDetailRefNo, out orderMappingCol))
                                            isTransactionSuccess = false;

                                        col.AddRange(orderMappingCol);
                                    }
                                }
                                else
                                {   //If Ordered Qty > Warehouse Balance 
                                    // Check if WH <= 0 then set all to backorder  
                                    if (whBal <= 0)
                                    {
                                        od.QtyApproved = 0;
                                        if (backOrder.AddItemIntoPurchaseOrderWithPriceLevel(od.ItemNo, (od.Quantity ?? 0) + od.RejectQty, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd HH:mm:ss"), od.PurchaseOrderDetailRefNo, pricelevel, out status, out pod))
                                        {
                                            col.AddRange(SalesOrderMappingController.DistributeBackOrderSalesMappingToQueryCommand(od.PurchaseOrderDetailRefNo, pod.PurchaseOrderDetailRefNo, (od.Quantity ?? 0) + od.RejectQty));
                                        }
                                    }
                                    else
                                    {
                                        // Check if WH <= 0 then set all to backorder  
                                        od.QtyApproved = whBal;

                                        if (backOrder.AddItemIntoPurchaseOrderWithPriceLevel(od.ItemNo, (od.Quantity ?? 0) - whBal + od.RejectQty, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd HH:mm:ss"), od.PurchaseOrderDetailRefNo, pricelevel, out status, out pod))
                                        {
                                            col.AddRange(SalesOrderMappingController.DistributeBackOrderSalesMappingToQueryCommand(od.PurchaseOrderDetailRefNo, pod.PurchaseOrderDetailRefNo, (od.Quantity ?? 0) - whBal + od.RejectQty));
                                        }
                                    }
                                    BackOrderNo = backOrder.GetPurchaseOrderHeaderRefNo();

                                }
                            }
                            if (od.QtyApproved > 0)
                                od.Status = "Approved";
                            else
                                od.Status = "Rejected";
                        }
                        else
                        {
                            if (od.Status != "Rejected")
                            {
                                PurchaseOrderDetail pod = new PurchaseOrderDetail();
                                
                                if (od.RejectQty > 0)
                                {
                                    if (backOrder.AddItemIntoPurchaseOrderWithPriceLevel(od.ItemNo, od.RejectQty, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd HH:mm:ss"), od.PurchaseOrderDetailRefNo, pricelevel, out status, out pod))
                                    {
                                        col.AddRange(SalesOrderMappingController.DistributeBackOrderSalesMappingToQueryCommand(od.PurchaseOrderDetailRefNo, pod.PurchaseOrderDetailRefNo, od.RejectQty));
                                    }
                                }
                                else
                                {
                                    QueryCommandCollection orderMappingCol = new QueryCommandCollection();
                                    if (!SalesOrderMappingController.ApproveAllSalesOrderMappingToQueryCommand(od.PurchaseOrderDetailRefNo, out orderMappingCol))
                                        isTransactionSuccess = false;

                                    col.AddRange(orderMappingCol);
                                }

                                od.Status = "Approved";
                            }
                        }
                        col.Add(od.GetUpdateCommand(UserInfo.username));
                    }
                    else if (poType == "RETURN" || poType == "ADJUSTMENT OUT")
                    {
                        Item itm = new Item(od.ItemNo);
                        string itemNo = itm.NonInventoryProduct ? itm.DeductedItem : itm.ItemNo;
                        decimal divider = 1;
                        if (itm.NonInventoryProduct)
                        {
                            divider = (itm.DeductConvType ? (1 / itm.DeductConvRate == 0 ? 1 : itm.DeductConvRate) : (itm.DeductConvRate));
                            
                        }

                        int stockBalance = InventoryController.GetStockBalanceQtyByItemSummaryByDate(itemNo, por.GetInventoryLocationID(), DateTime.Now, out status).GetIntValue();

                        //decimal totalQtyApproved = dataPODetColl.Where(d => d.ItemNo == poDet.ItemNo).Sum(d => d.QtyApproved);
                        if (!AllowDeductInvQtyNotSuffice && (od.QtyApproved * divider) > stockBalance)
                        {
                            od.Status = "Rejected";
                            status = "Item " + od.Item.ItemName + " have stock balance lower than Approved Qty ";
                            return false;
                        }
                        else
                        {
                            od.Status = "Approved";
                            col.Add(od.GetUpdateCommand(UserInfo.username));
                        }
                    }
                }
                //DataService.ExecuteTransaction(col);

                string msgSerialNo = "";
                if (!por.IsSerialNoValid(listInvDet, out msgSerialNo))
                {
                    status = "Cannot Approve Goods Ordering. " + msgSerialNo;
                    return false;
                }
                col.AddRange(por.UpdateHeaderStatusToQueryCommand(out poHdrStatus, out status, "SYSTEM"));
                //isTransactionSuccess = false;

                if ((poType.ToLower() == "replenish" || poType.ToLower() == "back order" || poType.ToLower() == "special order") && poHdr.Status == "Approved")
                {
                    if (string.IsNullOrEmpty(por.GetPOHeader().ShipVia) && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.AutoGenerateInvoiceNo), false))
                    {
                        QueryCommandCollection comd = new QueryCommandCollection();
                        string newInvoiceNo= "";
                        if(!StockTransferController.GenerateInvoiceCreditNoteNo(por.GetInventoryLocationID(),false, "SYSTEM", out comd, out newInvoiceNo, out status))
                        {
                            throw new Exception(status);
                        }
                        col.AddRange(comd);
                        col.AddRange(por.SetInvoiceCreditNoteNo("SYSTEM", newInvoiceNo, out status));
                    }

                    poHdr.IsAutoStockIn = autoStockIn;
                    col.Add(poHdr.GetUpdateCommand("SYSTEM"));

                    int invLocID = new InventoryLocation("InventoryLocationName", ConfigurationManager.AppSettings["WarehouseName"]).InventoryLocationID;
                    QueryCommandCollection stockOutQcc = new QueryCommandCollection();
                    QueryCommandCollection stockInQcc = new QueryCommandCollection();

                    if (poHdr.WarehouseID != 0)
                    {
                        if (!StockOutFromPurchaseOrderHeader(por, UserInfo.username, 0, poHdr.WarehouseID, false, false, "", out stockOutQcc))
                        {
                            isTransactionSuccess = false;
                        } 
                    }

                    if (stockOutQcc.Count == 0)
                        throw new Exception("Failed to stock out from warehouse");

                    if (autoStockIn && isTransactionSuccess)
                    {
                        Logger.writeLog("Calling StockInFromPurchaseOrderHeader, autoStockIn: " + autoStockIn.ToString());
                        if (!StockInFromPurchaseOrderHeader(por, UserInfo.username, 0, por.GetInventoryLocationID(), false, false, "", out stockInQcc))
                            isTransactionSuccess = false;
                    }
                    //combine qmc 

                    if (stockInQcc.Count == 0)
                        throw new Exception("Failed to do auto stock in");

                    col.AddRange(stockOutQcc);
                    col.AddRange(stockInQcc);
                }
                else if (poType == "RETURN" && poHdr.Status.ToUpper() == "APPROVED")
                {
                    //set creditnote no
                    if (string.IsNullOrEmpty(por.GetPOHeader().ShipVia) && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.AutoGenerateInvoiceNo), false))
                    {
                        QueryCommandCollection comd = new QueryCommandCollection();
                        string newInvoiceNo = "";
                        if (!StockTransferController.GenerateInvoiceCreditNoteNo(por.GetInventoryLocationID(), true, "SYSTEM", out comd, out newInvoiceNo, out status))
                        {
                            throw new Exception(status);
                        }
                        col.AddRange(comd);
                        col.AddRange(por.SetInvoiceCreditNoteNo("SYSTEM", newInvoiceNo, out status));
                    }
                }

                DataService.ExecuteTransaction(col);
                //}
                if (!isTransactionSuccess)
                    return false;


                #region *) Auto Create Supplier PO
                if ((poType.ToLower() == "replenish") && (poHdrStatus.ToUpper() == "APPROVED")
                    && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.AutoCreateSupplierPOUponOutletOrderApproval), false))
                {
                    if (poHdr.SupplierID.GetValueOrDefault(0) == 0)
                    {
                        // Supplier = ALL, then create PO based on Item Supplier Map for each supplier

                        List<PurchaseOdrController> invCtrlList = new List<PurchaseOdrController>();

                        List<PurchaseOrderDetail> detColl = por.GetPODetail().Where(d => d.Status.ToUpper() == "APPROVED").ToList();
                        foreach (PurchaseOrderDetail poDet in detColl)
                        {
                            int? supplierID = ItemSupplierMapController.GetPreferredSupplier(poDet.ItemNo);

                            if (supplierID.HasValue)
                            {
                                bool invCtrlExists = false;

                                //PurchaseOdrController invCtrl = invCtrlList.Where(po => po.InvHdr.Supplier == supplierID.ToString()).First();
                                PurchaseOdrController invCtrl = new PurchaseOdrController();
                                for (int i = 0; i < invCtrlList.Count; i++)
                                {
                                    if (invCtrlList[i].InvHdr.Supplier == supplierID.Value.ToString())
                                    {
                                        invCtrl = invCtrlList[i];
                                        invCtrlExists = true;
                                        break;
                                    }
                                }

                                if (!invCtrlExists)
                                {
                                    Supplier s = new Supplier(supplierID.Value);

                                    invCtrl = new PurchaseOdrController();
                                    invCtrl.SetInventoryLocation(poHdr.InventoryLocationID.GetValueOrDefault(0));
                                    invCtrl.SetPurchaseOrderHeaderInfo(supplierID.Value.ToString(), "", 0, 1, 0);
                                    invCtrl.InvHdr.GSTType = string.IsNullOrEmpty(s.GSTRule) ? "0" : s.GSTRule;
                                    invCtrlList.Add(invCtrl);
                                }

                                string packingSizeName = "";
                                decimal costPrice = 0;

                                Item theItem = new Item(poDet.ItemNo);
                                if (theItem != null && theItem.ItemNo == poDet.ItemNo)
                                    packingSizeName = theItem.UOM;

                                //Query qr = new Query("ItemSupplierMap");
                                //qr.AddWhere(ItemSupplierMap.Columns.ItemNo, poDet.ItemNo);
                                //qr.AddWhere(ItemSupplierMap.Columns.SupplierID, supplierID.Value);
                                //ItemSupplierMap ism = new ItemSupplierMapController().FetchByQuery(qr).FirstOrDefault();
                                //if (ism != null)
                                //    costPrice = ism.CostPrice;
                                //else
                                //    costPrice = theItem.FactoryPrice;

                                costPrice = poDet.FactoryPrice == null ? theItem.FactoryPrice : poDet.FactoryPrice.GetValueOrDefault(0);

                                if (!invCtrl.AddItemToPurchaseOrderByPackingSize(poDet.ItemNo, poDet.QtyApproved, packingSizeName, 1, costPrice, invCtrl.InvHdr.GSTType.GetIntValue(), out status))
                                {
                                    throw new Exception("Error: " + status);
                                }
                            }
                        }

                        foreach (PurchaseOdrController invCtrl in invCtrlList)
                        {
                            invCtrl.InvHdr.CustomRefNo = ""; // Empty it so it will regenerate

                            col = new QueryCommandCollection();
                            col.AddRange(invCtrl.getInsertCommand(UserInfo.username, poHdr.InventoryLocationID.GetValueOrDefault(0), false));

                            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.AutoApproveSupplierPO), false))
                            {
                                if (invCtrl.GetTotalCost() > 0)
                                {
                                    bool updateCostPrice = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.AutoUpdateCostPriceOnSupplierPOApproval), false);
                                    QueryCommandCollection approveCmd;
                                    invCtrl.GetApprovePOAndUpdateCostPriceCommand(updateCostPrice, out status, out approveCmd);
                                    if (approveCmd != null && approveCmd.Count > 0)
                                        col.AddRange(approveCmd);
                                }
                            }

                            DataService.ExecuteTransaction(col);

                            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.UseCustomNo), false))
                            {
                                PurchaseOdrController.CustomRefNoUpdate();
                            }
                        }
                    }
                    else
                    {
                        // Supplier is selected

                        if (poHdr.WarehouseID == 0)
                        {
                            // Not a warehouse, then create PO for this supplier
                            Supplier s = new Supplier(poHdr.SupplierID.GetValueOrDefault(0));

                            PurchaseOdrController invCtrl = new PurchaseOdrController();
                            invCtrl.SetInventoryLocation(poHdr.InventoryLocationID.GetValueOrDefault(0));
                            invCtrl.SetPurchaseOrderHeaderInfo(poHdr.SupplierID.GetValueOrDefault(0).ToString(), "", 0, 1, 0);
                            invCtrl.InvHdr.GSTType = string.IsNullOrEmpty(s.GSTRule) ? "0" : s.GSTRule;
                            invCtrl.SetGoodOrderRefNo(poHdr.PurchaseOrderHeaderRefNo);

                            foreach (PurchaseOrderDetail poDet in por.GetPODetail())
                            {
                                if (poDet.Status.ToUpper() == "APPROVED")
                                {
                                    string packingSizeName = "";
                                    decimal costPrice = 0;

                                    Item theItem = new Item(poDet.ItemNo);
                                    if (theItem != null && theItem.ItemNo == poDet.ItemNo)
                                        packingSizeName = theItem.UOM;

                                    //Query qr = new Query("ItemSupplierMap");
                                    //qr.AddWhere(ItemSupplierMap.Columns.ItemNo, poDet.ItemNo);
                                    //qr.AddWhere(ItemSupplierMap.Columns.SupplierID, poHdr.SupplierID.GetValueOrDefault(0));
                                    //ItemSupplierMap ism = new ItemSupplierMapController().FetchByQuery(qr).FirstOrDefault();
                                    //if (ism != null)
                                    //    costPrice = ism.CostPrice;
                                    //else
                                    //    costPrice = theItem.FactoryPrice;
                                    costPrice = poDet.FactoryPrice == null ? theItem.FactoryPrice : poDet.FactoryPrice.GetValueOrDefault(0);

                                    if (!invCtrl.AddItemToPurchaseOrderByPackingSize(poDet.ItemNo, poDet.QtyApproved, packingSizeName, 1, costPrice, invCtrl.InvHdr.GSTType.GetIntValue(), out status))
                                    {
                                        throw new Exception("Error: " + status);
                                    }
                                }
                            }

                            col = new QueryCommandCollection();
                            col.AddRange(invCtrl.getInsertCommand(UserInfo.username, poHdr.InventoryLocationID.GetValueOrDefault(0), false));

                            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.AutoApproveSupplierPO), false))
                            {
                                if (invCtrl.GetTotalCost() > 0)
                                {
                                    bool updateCostPrice = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.AutoUpdateCostPriceOnSupplierPOApproval), false);
                                    QueryCommandCollection approveCmd;
                                    invCtrl.GetApprovePOAndUpdateCostPriceCommand(updateCostPrice, out status, out approveCmd);
                                    if (approveCmd != null && approveCmd.Count > 0)
                                        col.AddRange(approveCmd);
                                }
                            }

                            DataService.ExecuteTransaction(col);

                            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.UseCustomNo), false))
                            {
                                PurchaseOdrController.CustomRefNoUpdate();
                            }
                        }
                        else
                        {
                            // Warehouse is selected, then no need to create anything
                        }
                    }
                }
                #endregion


                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex.Message);
                status = ex.Message; BackOrderNo = "";
                return false;
            }
        }

        public static int GetGSTRuleDefaultWebOrdering() 
        {
            int GSTRuleIndex = 3;
            try
            {
                if (AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.InvoiceGSTRule) != "")
                {
                    String[] GSTRules = new String[] { "", "Exclusive GST", "Inclusive GST", "Non GST" };
                    for (int i = 0; i < GSTRules.Count(); i++)
                    {
                        if (GSTRules[i] == AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.InvoiceGSTRule))
                        {
                            GSTRuleIndex = i;
                            break;
                        }
                    }                    
                }
            }
            catch (Exception ex)
            {
                GSTRuleIndex = 3;
            }

            return GSTRuleIndex;
        }

        // * change approved qty for approved PO then make adjustment /

        public bool ChangeApprovedPOApprovedQty(string PurchaseOrderDetRefNo, decimal newQtyApproved, out string status)
        {
            status = "";
            try
            {
                PurchaseOrderDetail myTmpDet;

                myTmpDet = (PowerPOS.PurchaseOrderDetail)InvDet.Find(PurchaseOrderDetRefNo);

                if (myTmpDet == null)
                {
                    throw new Exception("Inventory detail are not created yet");
                }

                decimal oldQty = myTmpDet.QtyApproved;
                decimal diff = newQtyApproved - oldQty;

                int invLocID = new InventoryLocation("InventoryLocationName", ConfigurationManager.AppSettings["WarehouseName"]).InventoryLocationID;

                if (diff > 0)
                {
                    // create adjustment in outlet stock
                    StockInFromPurchaseOrderDetailRefNoWithoutUpdStatus(myTmpDet.PurchaseOrderHeaderRefNo, myTmpDet.PurchaseOrderDetailRefNo, diff,
                                                        "", 0, myTmpDet.PurchaseOrderHeader.InventoryLocationID ?? 0, true, true,
                                                        "Change Approved Qty for" + myTmpDet.PurchaseOrderDetailRefNo);

                    if (invLocID != 0 && invLocID != myTmpDet.PurchaseOrderHeader.InventoryLocationID)
                    {
                        // create adjustment out hq stock
                        StockOutFromPurchaseOrderDetailRefNoWithoutUpdStatus(myTmpDet.PurchaseOrderHeaderRefNo, myTmpDet.PurchaseOrderDetailRefNo, diff,
                                                            "", 0, invLocID, true, true,
                                                            "Change Approved Qty for" + myTmpDet.PurchaseOrderDetailRefNo);
                    }
                }
                else if (diff < 0)
                { 
                    // create adjustment out outlet stock
                    StockOutFromPurchaseOrderDetailRefNoWithoutUpdStatus(myTmpDet.PurchaseOrderHeaderRefNo, myTmpDet.PurchaseOrderDetailRefNo, Math.Abs(diff),
                                                        "", 0, myTmpDet.PurchaseOrderHeader.InventoryLocationID ?? 0, true, true,
                                                        "Change Approved Qty for" + myTmpDet.PurchaseOrderDetailRefNo);

                    if (invLocID != 0 && invLocID != myTmpDet.PurchaseOrderHeader.InventoryLocationID)
                    {
                        // create adjustment in outlet stock
                        StockInFromPurchaseOrderDetailRefNoWithoutUpdStatus(myTmpDet.PurchaseOrderHeaderRefNo, myTmpDet.PurchaseOrderDetailRefNo, Math.Abs(diff),
                                                            "", 0, invLocID, true, true,
                                                            "Change Approved Qty for" + myTmpDet.PurchaseOrderDetailRefNo);
                    }
                }


                // search for back order and update quantity and quantity approved
                string query = "select d.* from purchaseorderdetail d inner join purchaseorderheader h on d.PurchaseOrderHeaderRefNo = h.PurchaseOrderHeaderRefNo " + 
                                "where h.Userfld2 = 'Back Order' and d.userfld6 = '" + myTmpDet.PurchaseOrderDetailRefNo + "' and ISNULL(d.userfld1,'Approved') = 'Approved' order by d.createdon desc";
                DataTable ds = DataService.GetDataSet(new QueryCommand(query)).Tables[0];

                PurchaseOrderDetailCollection col = new PurchaseOrderDetailCollection();
                col.Load(ds);

                if (col.Count > 0)
                {
                    col[0].Quantity -= diff;
                    if (col[0].Status == "Approved")
                    {
                        decimal oldq = col[0].QtyApproved;

                        col[0].QtyApproved -= diff;
                        col[0].Amount = col[0].Amount / (oldq <= 0 ? 1 : oldq)* col[0].QtyApproved;
                        col[0].GSTAmount = col[0].GSTAmount / (oldq <= 0 ? 1 : oldq) * col[0].QtyApproved;
                    }                    
                    col[0].Save();
                }

                myTmpDet.QtyApproved = newQtyApproved;
                if (myTmpDet.Amount > 0)
                {
                    myTmpDet.Amount = myTmpDet.Amount / (oldQty <= 0 ? 1 : oldQty) * myTmpDet.QtyApproved;
                    myTmpDet.GSTAmount = myTmpDet.GSTAmount / (oldQty <= 0 ? 1 : oldQty) * myTmpDet.QtyApproved;
                }
                InvDet.SaveAll();
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error change Approved Qty" + ex.Message);
                status = ex.Message;
                return false;
            }
        }

        public bool ChangeApprovedPOApprovedQtyWithOrderFrom(string PurchaseOrderDetRefNo, decimal newQtyApproved, out string status)
        {
            status = "";
            try
            {
                PurchaseOrderDetail myTmpDet;

                myTmpDet = (PowerPOS.PurchaseOrderDetail)InvDet.Find(PurchaseOrderDetRefNo);

                

                if (myTmpDet == null)
                {
                    throw new Exception("Inventory detail are not created yet");
                }

                decimal oldQty = myTmpDet.QtyApproved;
                decimal diff = newQtyApproved - oldQty;

                int invLocID = new InventoryLocation("InventoryLocationName", ConfigurationManager.AppSettings["WarehouseName"]).InventoryLocationID;

                if (diff > 0)
                {
                    if (InvHdr.WarehouseID != 0)
                    {
                        //stock out from orderfrom
                        StockOutFromPurchaseOrderDetailRefNoWithoutUpdStatus(myTmpDet.PurchaseOrderHeaderRefNo, myTmpDet.PurchaseOrderDetailRefNo, diff,
                                                                "", 0, InvHdr.WarehouseID, true, true,
                                                                "Change Approved Qty for" + myTmpDet.PurchaseOrderDetailRefNo);


                        //if already received or posted then stock in
                        if(InvHdr.Status == "Received" || InvHdr.Status == "Posted")
                            StockInFromPurchaseOrderDetailRefNoWithoutUpdStatus(myTmpDet.PurchaseOrderHeaderRefNo, myTmpDet.PurchaseOrderDetailRefNo, diff,
                                                                "", 0, myTmpDet.PurchaseOrderHeader.InventoryLocationID ?? 0, true, true,
                                                                "Change Approved Qty for" + myTmpDet.PurchaseOrderDetailRefNo);
                    }
                    else
                    {
                        // create adjustment in outlet stock
                        StockInFromPurchaseOrderDetailRefNoWithoutUpdStatus(myTmpDet.PurchaseOrderHeaderRefNo, myTmpDet.PurchaseOrderDetailRefNo, diff,
                                                            "", 0, myTmpDet.PurchaseOrderHeader.InventoryLocationID ?? 0, true, true,
                                                            "Change Approved Qty for" + myTmpDet.PurchaseOrderDetailRefNo);

                        if (invLocID != 0 && invLocID != myTmpDet.PurchaseOrderHeader.InventoryLocationID)
                        {
                            // create adjustment out hq stock
                            StockOutFromPurchaseOrderDetailRefNoWithoutUpdStatus(myTmpDet.PurchaseOrderHeaderRefNo, myTmpDet.PurchaseOrderDetailRefNo, diff,
                                                                "", 0, invLocID, true, true,
                                                                "Change Approved Qty for" + myTmpDet.PurchaseOrderDetailRefNo);
                        }
                    }
                }
                else if (diff < 0)
                {
                    if (InvHdr.WarehouseID != 0)
                    {

                        // create adjustment in outlet stock
                        StockInFromPurchaseOrderDetailRefNoWithoutUpdStatus(myTmpDet.PurchaseOrderHeaderRefNo, myTmpDet.PurchaseOrderDetailRefNo, Math.Abs(diff),
                                                            "", 0, InvHdr.WarehouseID, true, true,
                                                            "Change Approved Qty for" + myTmpDet.PurchaseOrderDetailRefNo);

                        // create adjustment out outlet stock
                        if (InvHdr.Status == "Received" || InvHdr.Status == "Posted")
                            StockOutFromPurchaseOrderDetailRefNoWithoutUpdStatus(myTmpDet.PurchaseOrderHeaderRefNo, myTmpDet.PurchaseOrderDetailRefNo, Math.Abs(diff),
                                                                "", 0, myTmpDet.PurchaseOrderHeader.InventoryLocationID ?? 0, true, true,
                                                                "Change Approved Qty for" + myTmpDet.PurchaseOrderDetailRefNo);
                    }
                    else
                    {
                        // create adjustment out outlet stock
                        StockOutFromPurchaseOrderDetailRefNoWithoutUpdStatus(myTmpDet.PurchaseOrderHeaderRefNo, myTmpDet.PurchaseOrderDetailRefNo, Math.Abs(diff),
                                                            "", 0, myTmpDet.PurchaseOrderHeader.InventoryLocationID ?? 0, true, true,
                                                            "Change Approved Qty for" + myTmpDet.PurchaseOrderDetailRefNo);

                        if (invLocID != 0 && invLocID != myTmpDet.PurchaseOrderHeader.InventoryLocationID)
                        {
                            // create adjustment in outlet stock
                            StockInFromPurchaseOrderDetailRefNoWithoutUpdStatus(myTmpDet.PurchaseOrderHeaderRefNo, myTmpDet.PurchaseOrderDetailRefNo, Math.Abs(diff),
                                                                "", 0, invLocID, true, true,
                                                                "Change Approved Qty for" + myTmpDet.PurchaseOrderDetailRefNo);
                        }
                    }
                }


                // search for back order and update quantity and quantity approved
                string query = "select d.* from purchaseorderdetail d inner join purchaseorderheader h on d.PurchaseOrderHeaderRefNo = h.PurchaseOrderHeaderRefNo " +
                                "where h.Userfld2 = 'Back Order' and d.userfld6 = '" + myTmpDet.PurchaseOrderDetailRefNo + "' and ISNULL(d.userfld1,'Approved') = 'Approved' order by d.createdon desc";
                DataTable ds = DataService.GetDataSet(new QueryCommand(query)).Tables[0];

                PurchaseOrderDetailCollection col = new PurchaseOrderDetailCollection();
                col.Load(ds);

                if (col.Count > 0)
                {
                    col[0].Quantity -= diff;
                    if (col[0].Status == "Approved")
                    {
                        decimal oldq = col[0].QtyApproved;

                        col[0].QtyApproved -= diff;
                        col[0].Amount = col[0].Amount / (oldq <= 0 ? 1 : oldq) * col[0].QtyApproved;
                        col[0].GSTAmount = col[0].GSTAmount / (oldq <= 0 ? 1 : oldq) * col[0].QtyApproved;
                    }
                    col[0].Save();
                }

                myTmpDet.QtyApproved = newQtyApproved;
                if (myTmpDet.Amount > 0)
                {
                    myTmpDet.Amount = myTmpDet.Amount / (oldQty <= 0 ? 1 : oldQty) * myTmpDet.QtyApproved;
                    myTmpDet.GSTAmount = myTmpDet.GSTAmount / (oldQty <= 0 ? 1 : oldQty) * myTmpDet.QtyApproved;
                }
                InvDet.SaveAll();
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error change Approved Qty" + ex.Message);
                status = ex.Message;
                return false;
            }
        }

        // * change approved qty for approved PO then make adjustment /

        
        #region Create Purchase Order From Sales

        public static DataTable FetchSalesDetailDatabyDate(DateTime startDate, DateTime endDate, int PointOfSaleID)
        {
            return FetchSalesDetailDatabyDate(startDate, endDate, PointOfSaleID, 0);
        }

        public static DataTable FetchSalesDetailDatabyDate(DateTime startDate, DateTime endDate, int PointOfSaleID, int supplierID)
        {
            DataTable dt = null;
            try
            {
                string sqlString = "Select * from OrderDet inner join Item on OrderDet.ItemNo = Item.ItemNo and Item.Deleted = 0 where OrderDet.OrderHdrID in " +
                    "(Select OrderHdrID from OrderHdr where isVoided = 0 and PointOfSaleID = " + PointOfSaleID.ToString() + " and OrderDate Between '" +
                    startDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + endDate.ToString("yyyy-MM-dd HH:mm:ss") + "')" +
                    " and ISNULL(OrderDet.userfld9,'') = '' and Item.IsInInventory = 1";

                if (supplierID != 0)
                {
                    sqlString += " and Item.ItemNo in (select ItemNo from ItemSupplierMap where SupplierID = " + supplierID.ToString() + ")";
                }

                dt = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
                return dt;
            }
            catch (Exception ex) { Logger.writeLog(ex.Message); return dt; }
        }

        public static DataTable FetchSalesDetailDatabyDateSupplierPortal(DateTime startDate, DateTime endDate, int PointOfSaleID, int supplierID, string username)
        {
            DataTable dt = null;
            try
            {
                string queryW = "Select count(*) from Supplier where ISNULL(Userflag1,0) = 1 and ISNULL(Deleted,0) = 0";

                int countWarehouse = (int)DataService.ExecuteScalar(new QueryCommand(queryW));

                string sqlString = "Select * from OrderDet inner join Item on OrderDet.ItemNo = Item.ItemNo and Item.Deleted = 0 where OrderDet.OrderHdrID in " +
                    "(Select OrderHdrID from OrderHdr where isVoided = 0 and PointOfSaleID = " + PointOfSaleID.ToString() + " and OrderDate Between '" +
                    startDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + endDate.ToString("yyyy-MM-dd HH:mm:ss") + "')" +
                    " and ISNULL(OrderDet.userfld9,'') = '' and Item.IsInInventory = 1 and OrderDet.Quantity > 0 and OrderDet.Amount > 0";

                bool isUseSupplierPortal = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.User.UseSupplierPortal), false);


                if (supplierID != 0 && countWarehouse > 1)
                {
                    sqlString += " and Item.ItemNo in (select ItemNo from ItemSupplierMap where SupplierID = " + supplierID.ToString() + ")";
                }
                else
                {
                    List<string> supID = new List<string>();
                    string supp = "";
                    UserMst us = new UserMst(UserMst.Columns.UserName, username);
                    if (isUseSupplierPortal && us != null && us.IsSupplier && us.IsRestrictedSupplierList && supplierID == 0)
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

                        sqlString += " and Item.ItemNo in (select ItemNo from ItemSupplierMap where SupplierID IN (" + supp + ") )";
                    }
                }

                dt = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
                return dt;
            }
            catch (Exception ex) { Logger.writeLog(ex.Message); return dt; }
        }

        public static string FetchSalesPersonByDate(DateTime startDate, DateTime endDate, int PointOfSaleID)
        {
            DataTable dt = null;
            string username = "";
            try
            {
                string sqlString = "Select s.* from OrderDet od " +
                    "inner join Item on od.ItemNo = Item.ItemNo and Item.Deleted = 0 " +
                    "left join SalesCommissionRecord S ON s.OrderHdrID = od.OrderHdrID " +
                    "where od.OrderHdrID in  " +
                    "(Select OrderHdrID from OrderHdr where isVoided = 0 and PointOfSaleID = " + PointOfSaleID.ToString() + " and OrderDate Between '" +
                    startDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + endDate.ToString("yyyy-MM-dd HH:mm:ss") + "')" +
                    " and ISNULL(od.userfld9,'') = '' and Item.IsInInventory = 1";
                dt = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];

                if (dt.Rows.Count > 0)
                {
                    username = dt.Rows[0]["SalesPersonID"].ToString();
                }
                return username;
            }
            catch (Exception ex) { Logger.writeLog(ex.Message); return username; }
        }

        public static bool createPurchaseOrderFromSales(POSController pos, bool fromWeb, bool autoApprove, out string status)
        {
            status = "";
            try
            {
                // init replenish 
                PurchaseOrderController por = new PurchaseOrderController();
                por.InvHdr.PurchaseOrderHeaderRefNo = getNewPurchaseOrderRefNo((int)pos.myOrderHdr.PointOfSale.Outlet.InventoryLocationID);
                por.InvHdr.PurchaseOrderDate = DateTime.Now;
                por.InvHdr.DateNeededBy = DateTime.Now.AddDays(1);
                por.InvHdr.POType = "Replenish";
                por.InvHdr.Status = "Submitted";
                por.InvHdr.Discount = 0;
                por.InvHdr.InventoryLocationID = (int)pos.myOrderHdr.PointOfSale.Outlet.InventoryLocationID;
                por.InvHdr.RequestedBy = pos.myOrderHdr.CashierID;
                por.InvHdr.Userfld10 = pos.myOrderHdr.OrderHdrID;

                foreach (OrderDet od in pos.myOrderDet)
                {
                    if (!od.IsVoided)
                    {
                        PurchaseOrderDetail pod = new PurchaseOrderDetail();
                        por.AddItemIntoPurchaseOrder(od.ItemNo, od.Quantity.GetValueOrDefault(0).GetIntValue(), od.OrderDetDate.AddYears(100).ToString("yyyy-MM-dd HH:mm:ss"), od.OrderDetID, out status, out pod);
                    }
                }

                string newPurchaseOrderHdrRefNo;
                //string status;

                if (!fromWeb)
                {
                    SyncClientController.Load_WS_URL();
                    PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                    ws.Timeout = 100000;
                    ws.Url = SyncClientController.WS_URL;
                    DataSet myDataSet = new DataSet();
                    myDataSet.Tables.Add(por.InvHdrToDataTable());
                    myDataSet.Tables.Add(por.InvDetToDataTable());
                    byte[] data = SyncClientController.CompressDataSetToByteArray(myDataSet);

                    if (ws.PurchaseOrderCompressed
                        (data,
                        UserInfo.username,
                        (int)pos.myOrderHdr.PointOfSale.Outlet.InventoryLocationID,
                        false, out newPurchaseOrderHdrRefNo, out status))
                    {
                        //download inventoryhdr and inventorydet                        
                        if (SyncClientController.GetCurrentPurchaseOrder())
                        {
                            //isSuccess = true;
                            status = "";
                            return true;
                        }
                        else
                        {
                            Logger.writeLog("Unable to download data from server: " + status);
                            return false;
                        }
                    }


                }
                else
                {
                    bool res = true;

                    if (por.GetPODetail().Count > 0)
                    {
                        if (!por.CreateOrder(UserInfo.username,
                        (int)pos.myOrderHdr.PointOfSale.Outlet.InventoryLocationID,
                        out status))
                        {

                            res = false;
                        }
                        else
                        {
                            if (autoApprove)
                            {
                                string backOrderNo = "";
                                if (PurchaseOrderApprovalXY(por.GetPurchaseOrderHeaderRefNo(), true, out status, out backOrderNo))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex) { Logger.writeLog(ex.Message); status = ex.Message; return false; }
        }

        #endregion
        public static bool StockInFromPurchaseOrderHeaderRefNo(string PurchaseOrderHeaderRefNo,
                string username, int StockInReasonID, int InventoryLocationID,
                bool IsAdjustment, bool CalculateCOGS, string Remark, out QueryCommandCollection qmc)
        {
            string status = "";
            string newRefNo = "";

            qmc = new QueryCommandCollection();

            try
            {
                InventoryLocation invLoc = new InventoryLocation(InventoryLocationID);
                if (invLoc == null || invLoc.InventoryLocationID != InventoryLocationID)
                {
                    status = "Inventory Location not found.";
                    return false;
                }

                if (InventoryLocationController.IsDeleted(InventoryLocationID))
                {
                    status = "Inventory Location is Deleted";
                    return false;
                }

                if (StockTakeController.IsInventoryLocationFrozen(InventoryLocationID))
                {
                    status = "Inventory Location is Frozen";
                    return false;
                }

                PurchaseOrderHeader poHdr = new PurchaseOrderHeader(PurchaseOrderHeaderRefNo);
                if (poHdr == null || poHdr.PurchaseOrderHeaderRefNo != PurchaseOrderHeaderRefNo)
                {
                    status = "Document Number not found.";
                    return false;
                }
                else
                {
                    if ((poHdr.POType.ToUpper() == "ORDER" || poHdr.POType.ToUpper() == "REPLENISH") && poHdr.Status != "Approved")
                    {
                        status = "Cannot do Stock In. Please check the document's status.";
                        return false;
                    }
                }

                PurchaseOrderDetailCollection podColl = new PurchaseOrderDetailCollection();
                podColl.Where(PurchaseOrderDetail.Columns.PurchaseOrderHeaderRefNo, PurchaseOrderHeaderRefNo);
                podColl.Load();

                PowerPOS.Container.CostingMethods CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
                string strCostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
                if (strCostingMethod.ToLower() == "fifo")
                    CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
                else if (strCostingMethod.ToLower() == "fixed avg")
                    CostingMethod = PowerPOS.Container.CostingMethods.FixedAvg;
                else
                    CostingMethod = PowerPOS.Container.CostingMethods.FIFO;

                InventoryController ctrl = new InventoryController(CostingMethod);
                ctrl.SetInventoryHeaderInfo(PurchaseOrderHeaderRefNo, "", Remark, 0, 0, 0);
                ctrl.SetVendorInvoiceNo(poHdr.ShipVia);

                // Stock In Reason is stored in StockOutReasonID column
                if (StockInReasonID > -1) ctrl.setInventoryStockOutReasonID(StockInReasonID);

                foreach (var det in podColl)
                {
                    if (det.QtyApproved > 0)
                    {
                        ctrl.AddItemIntoInventory(det.ItemNo, det.QtyApproved, out status);
                    }
                }

                if (status == "")
                {
                    ctrl.InvHdr.InventoryHdrRefNo = InventoryController.getNewInventoryRefNo(InventoryLocationID);
                    qmc = ctrl.CreateStockInQueryCommand(username, InventoryLocationID, IsAdjustment,
                        CalculateCOGS);
                    if (qmc.Count < 0)
                    {
                        newRefNo = ctrl.GetInvHdrRefNo();

                        if (poHdr != null && (poHdr.POType.ToUpper() == "ORDER" || poHdr.POType.ToUpper() == "REPLENISH"))
                        {
                            // If "Goods Receive" then update the Status to "Posted"
                            poHdr.Status = "Posted";
                            qmc.Add(poHdr.GetUpdateCommand(username));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                newRefNo = "";
            }

            var result = new
            {
                newRefNo = newRefNo,
                status = status
            };

            return true;
        }

        public static bool StockInFromPurchaseOrderHeader(PurchaseOrderController poController,
                string username, int StockInReasonID, int InventoryLocationID,
                bool IsAdjustment, bool CalculateCOGS, string Remark, out QueryCommandCollection qmc)
        {
            string status = "";
            string newRefNo = "";

            qmc = new QueryCommandCollection();

            try
            {
                InventoryLocation invLoc = new InventoryLocation(InventoryLocationID);
                if (invLoc == null || invLoc.InventoryLocationID != InventoryLocationID)
                {
                    status = "Inventory Location not found.";
                    return false;
                }

                if (InventoryLocationController.IsDeleted(InventoryLocationID))
                {
                    status = "Inventory Location is Deleted";
                    return false;
                }

                if (StockTakeController.IsInventoryLocationFrozen(InventoryLocationID))
                {
                    status = "Inventory Location is Frozen";
                    return false;
                }

                PurchaseOrderHeader poHdr = poController.InvHdr;
                //if (poHdr == null || poHdr.PurchaseOrderHeaderRefNo != PurchaseOrderHeaderRefNo)
                //{
                //    status = "Document Number not found.";
                //    return false;
                //}
                //else
                //{
                    if ((poHdr.POType.ToUpper() == "ORDER" || poHdr.POType.ToUpper() == "REPLENISH") && poHdr.Status != "Approved")
                    {
                        status = "Cannot do Stock In. Please check the document's status.";
                        return false;
                    }
                //}

                PurchaseOrderDetailCollection podColl = poController.GetPODetail();
                //podColl.Where(PurchaseOrderDetail.Columns.PurchaseOrderHeaderRefNo, PurchaseOrderHeaderRefNo);
                //podColl.Load();

                PowerPOS.Container.CostingMethods CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
                string strCostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
                if (strCostingMethod.ToLower() == "fifo")
                    CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
                else if (strCostingMethod.ToLower() == "fixed avg")
                    CostingMethod = PowerPOS.Container.CostingMethods.FixedAvg;
                else
                    CostingMethod = PowerPOS.Container.CostingMethods.FIFO;

                InventoryController ctrl = new InventoryController(CostingMethod);
                ctrl.SetInventoryHeaderInfo(poController.GetPurchaseOrderHeaderRefNo(), "", Remark, 0, 0, 0);
                ctrl.SetVendorInvoiceNo(poController.GetInvoiceNo());

                // Stock In Reason is stored in StockOutReasonID column
                if (StockInReasonID > -1) ctrl.setInventoryStockOutReasonID(StockInReasonID);

                foreach (var det in podColl)
                {
                    if (det.Status == "Rejected" && det.QtyApproved > 0)
                        det.QtyApproved = 0;

                    if (det.QtyApproved > 0)
                    {
                        ctrl.AddItemIntoInventory(det.ItemNo, det.QtyApproved, out status);
                    }
                }

                if (status == "")
                {
                    ctrl.InvHdr.InventoryHdrRefNo = InventoryController.getNewInventoryRefNo(InventoryLocationID);
                    ctrl.InvHdr.VendorInvoiceNo = poController.GetInvoiceNo();
                    qmc = ctrl.CreateStockInQueryCommand(username, InventoryLocationID, IsAdjustment,
                        CalculateCOGS);
                    if (qmc.Count > 0)
                    {
                        newRefNo = ctrl.GetInvHdrRefNo();

                        if (poHdr != null && (poHdr.POType.ToUpper() == "ORDER" || poHdr.POType.ToUpper() == "REPLENISH"))
                        {
                            // If "Goods Receive" then update the Status to "Posted"
                            poHdr.Status = "Posted";
                            qmc.Add(poHdr.GetUpdateCommand(username));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                newRefNo = "";
            }

            var result = new
            {
                newRefNo = newRefNo,
                status = status
            };

            return true;
        }

        public static bool StockOutFromPurchaseOrderHeaderRefNo(string PurchaseOrderHeaderRefNo,
                string username, int StockInReasonID, int InventoryLocationID,
                bool IsAdjustment, bool CalculateCOGS, string Remark, out QueryCommandCollection qmc)
        {
            string status = "";
            string newRefNo = "";

            qmc = new QueryCommandCollection();
            try
            {
                InventoryLocation invLoc = new InventoryLocation(InventoryLocationID);
                if (invLoc == null || invLoc.InventoryLocationID != InventoryLocationID)
                {
                    status = "Inventory Location not found.";
                    return false;
                }

                if (InventoryLocationController.IsDeleted(InventoryLocationID))
                {
                    status = "Inventory Location is Deleted";
                    return false;
                }

                if (StockTakeController.IsInventoryLocationFrozen(InventoryLocationID))
                {
                    status = "Inventory Location is Frozen";
                    return false;
                }

                PurchaseOrderHeader poHdr = new PurchaseOrderHeader(PurchaseOrderHeaderRefNo);
                if (poHdr == null || poHdr.PurchaseOrderHeaderRefNo != PurchaseOrderHeaderRefNo)
                {
                    status = "Document Number not found.";
                    return false;
                }
                else
                {
                    if ((poHdr.POType.ToUpper() == "ORDER" || poHdr.POType.ToUpper() == "REPLENISH") && poHdr.Status != "Approved")
                    {
                        status = "Cannot do Stock In. Please check the document's status.";
                        return false;
                    }
                }

                PurchaseOrderDetailCollection podColl = new PurchaseOrderDetailCollection();
                podColl.Where(PurchaseOrderDetail.Columns.PurchaseOrderHeaderRefNo, PurchaseOrderHeaderRefNo);
                podColl.Load();

                PowerPOS.Container.CostingMethods CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
                string strCostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
                if (strCostingMethod.ToLower() == "fifo")
                    CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
                else if (strCostingMethod.ToLower() == "fixed avg")
                    CostingMethod = PowerPOS.Container.CostingMethods.FixedAvg;
                else
                    CostingMethod = PowerPOS.Container.CostingMethods.FIFO;

                InventoryController ctrl = new InventoryController(CostingMethod);
                ctrl.SetInventoryHeaderInfo(PurchaseOrderHeaderRefNo, "", Remark, 0, 0, 0);

                // Stock In Reason is stored in StockOutReasonID column
                if (StockInReasonID > -1) ctrl.setInventoryStockOutReasonID(StockInReasonID);

                foreach (var det in podColl)
                {
                    ctrl.AddItemIntoInventory(det.ItemNo, det.QtyApproved, out status);
                }

                qmc = new QueryCommandCollection();
                if (status == "")
                {
                    ctrl.InvHdr.InventoryHdrRefNo = InventoryController.getNewInventoryRefNo(InventoryLocationID);
                    
                    if (ctrl.CreateStockOutQueryCommand(username, StockInReasonID, InventoryLocationID, IsAdjustment,
                        true, out status, out qmc))
                    {
                        newRefNo = ctrl.GetInvHdrRefNo();

                        if (poHdr != null && (poHdr.POType.ToUpper() == "ORDER" || poHdr.POType.ToUpper() == "REPLENISH"))
                        {
                            // If "Goods Receive" then update the Status to "Posted"
                            poHdr.Status = "Posted";
                            qmc.Add(poHdr.GetUpdateCommand(username));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                newRefNo = "";

                var result1 = new
                {
                    newRefNo = newRefNo,
                    status = status
                };

                return false;
            }

            var result = new
            {
                newRefNo = newRefNo,
                status = status
            };

            return true;
        }

        public static bool StockOutFromPurchaseOrderHeader(PurchaseOrderController poController,
                string username, int StockInReasonID, int InventoryLocationID,
                bool IsAdjustment, bool CalculateCOGS, string Remark, out QueryCommandCollection qmc)
        {
            string status = "";
            string newRefNo = "";

            qmc = new QueryCommandCollection();
            try
            {
                InventoryLocation invLoc = new InventoryLocation(InventoryLocationID);
                if (invLoc == null || invLoc.InventoryLocationID != InventoryLocationID)
                {
                    status = "Inventory Location not found.";
                    return false;
                }

                if (InventoryLocationController.IsDeleted(InventoryLocationID))
                {
                    status = "Inventory Location is Deleted";
                    return false;
                }

                if (StockTakeController.IsInventoryLocationFrozen(InventoryLocationID))
                {
                    status = "Inventory Location is Frozen";
                    return false;
                }

                PurchaseOrderHeader poHdr = poController.GetPOHeader();
                /*if (poHdr == null || poHdr.PurchaseOrderHeaderRefNo != PurchaseOrderHeaderRefNo)
                {
                    status = "Document Number not found.";
                    return false;
                }
                else
                {*/
                    if ((poHdr.POType.ToUpper() == "ORDER" || poHdr.POType.ToUpper() == "REPLENISH") && poHdr.Status != "Approved")
                    {
                        status = "Cannot do Stock In. Please check the document's status.";
                        return false;
                    }
                //}

                PurchaseOrderDetailCollection podColl = poController.GetPODetail();
                //podColl.Where(PurchaseOrderDetail.Columns.PurchaseOrderHeaderRefNo, PurchaseOrderHeaderRefNo);
                //podColl.Load();

                PowerPOS.Container.CostingMethods CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
                string strCostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
                if (strCostingMethod.ToLower() == "fifo")
                    CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
                else if (strCostingMethod.ToLower() == "fixed avg")
                    CostingMethod = PowerPOS.Container.CostingMethods.FixedAvg;
                else
                    CostingMethod = PowerPOS.Container.CostingMethods.FIFO;

                InventoryController ctrl = new InventoryController(CostingMethod);
                ctrl.SetInventoryHeaderInfo(poController.GetPurchaseOrderHeaderRefNo(), "", Remark, 0, 0, 0);

                // Stock In Reason is stored in StockOutReasonID column
                if (StockInReasonID > -1) ctrl.setInventoryStockOutReasonID(StockInReasonID);

                List<string> listInvDet = new List<string>();

                foreach (var det in podColl)
                {
                    if (det.Status == "Rejected" && det.QtyApproved > 0)
                        det.QtyApproved = 0;

                    ctrl.AddItemIntoInventory(det.ItemNo, det.QtyApproved, out status);

                    if (!string.IsNullOrEmpty(det.SerialNo))
                    {
                        //Split into string
                        string tmp = det.SerialNo;
                        List<String> _serialNo = tmp.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        ctrl.InvDet[ctrl.InvDet.Count - 1].SerialNo = _serialNo;

                    }
                }

                

                qmc = new QueryCommandCollection();
                if (status == "")
                {
                    ctrl.InvHdr.InventoryHdrRefNo = InventoryController.getNewInventoryRefNo(InventoryLocationID);

                    if (ctrl.CreateStockOutQueryCommand(username, StockInReasonID, InventoryLocationID, IsAdjustment,
                        true, out status, out qmc))
                    {
                        newRefNo = ctrl.GetInvHdrRefNo();

                        //if (poHdr != null && (poHdr.POType.ToUpper() == "ORDER" || poHdr.POType.ToUpper() == "REPLENISH"))
                        //{
                        //    // If "Goods Receive" then update the Status to "Posted"
                        //    poHdr.Status = "Posted";
                        //    qmc.Add(poHdr.GetUpdateCommand(username));
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                newRefNo = "";

                var result1 = new
                {
                    newRefNo = newRefNo,
                    status = status
                };

                return false;
            }

            var result = new
            {
                newRefNo = newRefNo,
                status = status
            };

            return true;
        }

        public static bool StockInFromPurchaseOrderDetailRefNo(string PurchaseOrderHeaderRefNo,
                string PurchaseOrderDetailRefNo, decimal Quantity,
                string username, int StockInReasonID, int InventoryLocationID,
                bool IsAdjustment, bool CalculateCOGS, string Remark)
        {
            string status = "";
            string newRefNo = "";


            try
            {
                InventoryLocation invLoc = new InventoryLocation(InventoryLocationID);
                if (invLoc == null || invLoc.InventoryLocationID != InventoryLocationID)
                {
                    status = "Inventory Location not found.";
                    return false;
                }

                if (InventoryLocationController.IsDeleted(InventoryLocationID))
                {
                    status = "Inventory Location is Deleted";
                    return false;
                }

                if (StockTakeController.IsInventoryLocationFrozen(InventoryLocationID))
                {
                    status = "Inventory Location is Frozen";
                    return false;
                }

                PurchaseOrderHeader poHdr = new PurchaseOrderHeader(PurchaseOrderHeaderRefNo);
                if (poHdr == null || poHdr.PurchaseOrderHeaderRefNo != PurchaseOrderHeaderRefNo)
                {
                    status = "Document Number not found.";
                    return false;
                }
                else
                {
                    //if (poHdr.POType.ToUpper() == "ORDER" && poHdr.Status != "Approved")
                    if (poHdr.Status != "Approved")
                    {
                        status = "Cannot do Stock In. Please check the document's status.";
                        return false;
                    }
                }

                PurchaseOrderDetailCollection podColl = new PurchaseOrderDetailCollection();
                podColl.Where(PurchaseOrderDetail.Columns.PurchaseOrderHeaderRefNo, PurchaseOrderHeaderRefNo);
                podColl.Where(PurchaseOrderDetail.Columns.PurchaseOrderDetailRefNo, PurchaseOrderDetailRefNo);
                podColl.Load();

                PowerPOS.Container.CostingMethods CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
                string strCostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
                if (strCostingMethod.ToLower() == "fifo")
                    CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
                else if (strCostingMethod.ToLower() == "fixed avg")
                    CostingMethod = PowerPOS.Container.CostingMethods.FixedAvg;
                else
                    CostingMethod = PowerPOS.Container.CostingMethods.FIFO;

                InventoryController ctrl = new InventoryController(CostingMethod);
                ctrl.SetInventoryHeaderInfo(PurchaseOrderHeaderRefNo, "", Remark, 0, 0, 0);

                // Stock In Reason is stored in StockOutReasonID column
                if (StockInReasonID > -1) ctrl.setInventoryStockOutReasonID(StockInReasonID);

                foreach (var det in podColl)
                {
                    if (det.QtyApproved > 0)
                    {
                        ctrl.AddItemIntoInventory(det.ItemNo, Quantity, out status);
                    }
                }

                if (status == "")
                {
                    if (ctrl.StockIn(username, InventoryLocationID, IsAdjustment,
                        CalculateCOGS, out status))
                    {
                        newRefNo = ctrl.GetInvHdrRefNo();

                        if (poHdr != null && (poHdr.POType.ToUpper() == "ORDER" || poHdr.POType.ToUpper() == "REPLENISH"))
                        {
                            // If "Goods Receive" then update the Status to "Posted"
                            poHdr.Status = "Posted";
                            poHdr.Save();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                newRefNo = "";

                var result1 = new
                {
                    newRefNo = newRefNo,
                    status = status
                };

                return false;
            }

            var result = new
            {
                newRefNo = newRefNo,
                status = status
            };

            return true;
        }

        public static bool StockOutFromPurchaseOrderDetailRefNo(string PurchaseOrderHeaderRefNo,
                string PurchaseOrderDetailRefNo, decimal Quantity,
                string username, int StockInReasonID, int InventoryLocationID,
                bool IsAdjustment, bool CalculateCOGS, string Remark)
        {
            string status = "";
            string newRefNo = "";


            try
            {
                InventoryLocation invLoc = new InventoryLocation(InventoryLocationID);
                if (invLoc == null || invLoc.InventoryLocationID != InventoryLocationID)
                {
                    status = "Inventory Location not found.";
                    return false;
                }

                if (InventoryLocationController.IsDeleted(InventoryLocationID))
                {
                    status = "Inventory Location is Deleted";
                    return false;
                }

                if (StockTakeController.IsInventoryLocationFrozen(InventoryLocationID))
                {
                    status = "Inventory Location is Frozen";
                    return false;
                }

                PurchaseOrderHeader poHdr = new PurchaseOrderHeader(PurchaseOrderHeaderRefNo);
                if (poHdr == null || poHdr.PurchaseOrderHeaderRefNo != PurchaseOrderHeaderRefNo)
                {
                    status = "Document Number not found.";
                    return false;
                }
                else
                {
                    if (poHdr.Status != "Approved")
                    {
                        status = "Cannot do Stock In. Please check the document's status.";
                        return false;
                    }
                }

                PurchaseOrderDetailCollection podColl = new PurchaseOrderDetailCollection();
                podColl.Where(PurchaseOrderDetail.Columns.PurchaseOrderHeaderRefNo, PurchaseOrderHeaderRefNo);
                podColl.Where(PurchaseOrderDetail.Columns.PurchaseOrderDetailRefNo, PurchaseOrderDetailRefNo);
                podColl.Load();

                PowerPOS.Container.CostingMethods CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
                string strCostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
                if (strCostingMethod.ToLower() == "fifo")
                    CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
                else if (strCostingMethod.ToLower() == "fixed avg")
                    CostingMethod = PowerPOS.Container.CostingMethods.FixedAvg;
                else
                    CostingMethod = PowerPOS.Container.CostingMethods.FIFO;

                InventoryController ctrl = new InventoryController(CostingMethod);
                ctrl.SetInventoryHeaderInfo(PurchaseOrderHeaderRefNo, "", Remark, 0, 0, 0);

                // Stock In Reason is stored in StockOutReasonID column
                if (StockInReasonID > -1) ctrl.setInventoryStockOutReasonID(StockInReasonID);

                foreach (var det in podColl)
                {
                    ctrl.AddItemIntoInventory(det.ItemNo, Quantity, out status);
                }

                if (status == "")
                {
                    if (ctrl.StockOut(username, StockInReasonID, InventoryLocationID, IsAdjustment,
                        true, out status))
                    {
                        newRefNo = ctrl.GetInvHdrRefNo();

                        if (poHdr != null && (poHdr.POType.ToUpper() == "ORDER" || poHdr.POType.ToUpper() == "REPLENISH"))
                        {
                            // If "Goods Receive" then update the Status to "Posted"
                            poHdr.Status = "Posted";
                            poHdr.Save();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                newRefNo = "";
            }

            var result = new
            {
                newRefNo = newRefNo,
                status = status
            };

            return true;
        }

        public static bool StockInFromPurchaseOrderDetailRefNoWithoutUpdStatus(string PurchaseOrderHeaderRefNo,
                string PurchaseOrderDetailRefNo, decimal Quantity,
                string username, int StockInReasonID, int InventoryLocationID,
                bool IsAdjustment, bool CalculateCOGS, string Remark)
        {
            string status = "";
            string newRefNo = "";


            try
            {
                InventoryLocation invLoc = new InventoryLocation(InventoryLocationID);
                if (invLoc == null || invLoc.InventoryLocationID != InventoryLocationID)
                {
                    status = "Inventory Location not found.";
                    return false;
                }

                if (InventoryLocationController.IsDeleted(InventoryLocationID))
                {
                    status = "Inventory Location is Deleted";
                    return false;
                }

                if (StockTakeController.IsInventoryLocationFrozen(InventoryLocationID))
                {
                    status = "Inventory Location is Frozen";
                    return false;
                }

                PurchaseOrderHeader poHdr = new PurchaseOrderHeader(PurchaseOrderHeaderRefNo);
                if (poHdr == null || poHdr.PurchaseOrderHeaderRefNo != PurchaseOrderHeaderRefNo)
                {
                    status = "Document Number not found.";
                    return false;
                }
                else
                {
                    //if (poHdr.POType.ToUpper() == "ORDER" && poHdr.Status != "Approved")
                    if (poHdr.Status != "Approved")
                    {
                        status = "Cannot do Stock In. Please check the document's status.";
                        return false;
                    }
                }

                PurchaseOrderDetailCollection podColl = new PurchaseOrderDetailCollection();
                podColl.Where(PurchaseOrderDetail.Columns.PurchaseOrderHeaderRefNo, PurchaseOrderHeaderRefNo);
                podColl.Where(PurchaseOrderDetail.Columns.PurchaseOrderDetailRefNo, PurchaseOrderDetailRefNo);
                podColl.Load();

                PowerPOS.Container.CostingMethods CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
                string strCostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
                if (strCostingMethod.ToLower() == "fifo")
                    CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
                else if (strCostingMethod.ToLower() == "fixed avg")
                    CostingMethod = PowerPOS.Container.CostingMethods.FixedAvg;
                else
                    CostingMethod = PowerPOS.Container.CostingMethods.FIFO;

                InventoryController ctrl = new InventoryController(CostingMethod);
                ctrl.SetInventoryHeaderInfo(PurchaseOrderHeaderRefNo, "", Remark, 0, 0, 0);

                // Stock In Reason is stored in StockOutReasonID column
                if (StockInReasonID > -1) ctrl.setInventoryStockOutReasonID(StockInReasonID);

                foreach (var det in podColl)
                {
                    if (det.QtyApproved > 0)
                    {
                        ctrl.AddItemIntoInventory(det.ItemNo, Quantity, out status);
                    }
                }

                if (status == "")
                {
                    if (ctrl.StockIn(username, InventoryLocationID, IsAdjustment,
                        CalculateCOGS, out status))
                    {
                        newRefNo = ctrl.GetInvHdrRefNo();                        
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                newRefNo = "";

                var result1 = new
                {
                    newRefNo = newRefNo,
                    status = status
                };

                return false;
            }

            var result = new
            {
                newRefNo = newRefNo,
                status = status
            };

            return true;
        }

        public static bool StockOutFromPurchaseOrderDetailRefNoWithoutUpdStatus(string PurchaseOrderHeaderRefNo,
                string PurchaseOrderDetailRefNo, decimal Quantity,
                string username, int StockInReasonID, int InventoryLocationID,
                bool IsAdjustment, bool CalculateCOGS, string Remark)
        {
            string status = "";
            string newRefNo = "";


            try
            {
                InventoryLocation invLoc = new InventoryLocation(InventoryLocationID);
                if (invLoc == null || invLoc.InventoryLocationID != InventoryLocationID)
                {
                    status = "Inventory Location not found.";
                    return false;
                }

                if (InventoryLocationController.IsDeleted(InventoryLocationID))
                {
                    status = "Inventory Location is Deleted";
                    return false;
                }

                if (StockTakeController.IsInventoryLocationFrozen(InventoryLocationID))
                {
                    status = "Inventory Location is Frozen";
                    return false;
                }

                PurchaseOrderHeader poHdr = new PurchaseOrderHeader(PurchaseOrderHeaderRefNo);
                if (poHdr == null || poHdr.PurchaseOrderHeaderRefNo != PurchaseOrderHeaderRefNo)
                {
                    status = "Document Number not found.";
                    return false;
                }
                else
                {
                    if (poHdr.Status != "Approved")
                    {
                        status = "Cannot do Stock In. Please check the document's status.";
                        return false;
                    }
                }

                PurchaseOrderDetailCollection podColl = new PurchaseOrderDetailCollection();
                podColl.Where(PurchaseOrderDetail.Columns.PurchaseOrderHeaderRefNo, PurchaseOrderHeaderRefNo);
                podColl.Where(PurchaseOrderDetail.Columns.PurchaseOrderDetailRefNo, PurchaseOrderDetailRefNo);
                podColl.Load();

                PowerPOS.Container.CostingMethods CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
                string strCostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
                if (strCostingMethod.ToLower() == "fifo")
                    CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
                else if (strCostingMethod.ToLower() == "fixed avg")
                    CostingMethod = PowerPOS.Container.CostingMethods.FixedAvg;
                else
                    CostingMethod = PowerPOS.Container.CostingMethods.FIFO;

                InventoryController ctrl = new InventoryController(CostingMethod);
                ctrl.SetInventoryHeaderInfo(PurchaseOrderHeaderRefNo, "", Remark, 0, 0, 0);

                // Stock In Reason is stored in StockOutReasonID column
                if (StockInReasonID > -1) ctrl.setInventoryStockOutReasonID(StockInReasonID);

                foreach (var det in podColl)
                {
                    ctrl.AddItemIntoInventory(det.ItemNo, Quantity, out status);
                }

                if (status == "")
                {
                    if (ctrl.StockOut(username, StockInReasonID, InventoryLocationID, IsAdjustment,
                        true, out status))
                    {
                        newRefNo = ctrl.GetInvHdrRefNo();                        
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                newRefNo = "";
            }

            var result = new
            {
                newRefNo = newRefNo,
                status = status
            };

            return true;
        }

        public static decimal GetWarehouseBalance(string itemNo, DateTime date, out string status)
        {
            status = "";
            decimal WHBal = 0;

            try
            {
                int invLocID = new InventoryLocation("InventoryLocationName", ConfigurationManager.AppSettings["WarehouseName"]).InventoryLocationID;
                if (invLocID == 0)
                {
                    status = "Warehouse not found.";
                }
                if (status == "")
                {
                    WHBal = InventoryController.GetStockBalanceQtyByItemSummaryByDate(itemNo, invLocID, date, out status);
                }
                return WHBal;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return 0;
            }
        }

        public static decimal GetWarehouseBalanceByLocID(int locationID, string itemNo, DateTime date, out string status)
        {
            status = "";
            decimal WHBal = 0;

            try
            {
                int invLocID = new InventoryLocation(locationID).InventoryLocationID;
                if (invLocID == 0)
                {
                    status = "Warehouse not found.";
                }
                if (status == "")
                {
                    WHBal = InventoryController.GetStockBalanceQtyByItemSummaryByDate(itemNo, invLocID, date, out status);
                }
                return WHBal;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return 0;
            }
        }

        public bool GenerateInvoiceNo(int InventoryLocationID, string username, out QueryCommandCollection tmp, out string newInvoiceNo, out string status)
        {
            status = "";
            newInvoiceNo = "";
            tmp = new QueryCommandCollection();
            try
            {
                int runningNumber = 0;
                string displayedName = "";
                InvoiceSequenceNoCollection col = new InvoiceSequenceNoCollection();
                Query qu = new Query("InvoiceSequenceNo");
                qu.AddWhere(InvoiceSequenceNo.Columns.InventoryLocationID, InventoryLocationID);
                qu.AddWhere(InvoiceSequenceNo.Columns.Deleted, false);

                col.LoadAndCloseReader(qu.ExecuteReader());

                InventoryLocation invLoc = new InventoryLocation(InventoryLocationID);
                if (invLoc.IsNew)
                    throw new Exception("Inventory doesn't exist");

                InvoiceSequenceNo no;

                if (col.Count() == 0)
                {
                    
                    no = new InvoiceSequenceNo();
                    no.CurrentNo = runningNumber;
                    no.InventoryLocationID = InventoryLocationID;
                    no.InventoryLocationCode = string.IsNullOrEmpty(invLoc.DisplayedName) ? invLoc.InventoryLocationName.Substring(0, 3) : invLoc.DisplayedName;
                    no.Deleted = false;
                    displayedName = no.InventoryLocationCode;
                }
                else
                {
                    no = col[0];
                    no.Deleted = false;
                    col[0].InventoryLocationCode = string.IsNullOrEmpty(invLoc.DisplayedName) ? invLoc.InventoryLocationName.Substring(0, 3) : invLoc.DisplayedName;
                    runningNumber = col[0].CurrentNo.GetValueOrDefault(0);
                    displayedName = col[0].InventoryLocationCode;
                }

                string prefix = AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.CustomPrefix);
                string suffix = AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.CustomSuffix);

                int numberLength;
                if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.NumberLength), out numberLength))
                {
                    numberLength = 6;
                }

                string dateFormat = AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.CustomNoDateFormat);
                if (string.IsNullOrEmpty(dateFormat)) dateFormat = "yyMMdd";
                prefix = prefix.Replace("[DATE]", DateTime.Now.ToString(dateFormat));
                suffix = suffix.Replace("[DATE]", DateTime.Now.ToString(dateFormat));

                #region *) Check whether need to reset the runningNo or not
                string resetEvery = AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ResetNumberEvery);
                if (!string.IsNullOrEmpty(resetEvery) && resetEvery != Constants.ResetEvery.Never)
                {
                    bool needReset = false;
                    string lastReset = no.Userfld1;
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
                            if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.CurrentNo), out tmpNo))
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
                        no.Userfld1 = DateTime.Now.ToString("yyMMdd");
                    }

                    if (needReset)
                    {
                        runningNumber = 1;
                        no.CurrentNo = runningNumber;
                        no.Userfld1 = DateTime.Now.ToString("yyMMdd");
                    }
                    else{
                        runningNumber += 1;
                        no.CurrentNo = runningNumber;
                    }
                }
                #endregion

                newInvoiceNo = displayedName + prefix + runningNumber.ToString().PadLeft(numberLength, '0') + suffix;

                if(no.IsNew)
                    tmp.Add(no.GetInsertCommand(username));
                else
                    tmp.Add(no.GetUpdateCommand(username));

                return true;

            }
            catch (Exception ex)
            {
                Logger.writeLog("Error Generate Invoice No" + ex.Message);
                status = "Error Generate Invoice No" + ex.Message;

                return false;
            }
        }
        
        public QueryCommandCollection SetAutoGenerateInvoiceNo(string username, out string status)
        {
            status = "";
            QueryCommandCollection tmp = new QueryCommandCollection();
            try
            {
                if (string.IsNullOrEmpty(this.InvHdr.ShipVia) && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.AutoGenerateInvoiceNo),false))
                {
                    QueryCommandCollection tm = new QueryCommandCollection();
                    string newInvoiceNo = "";

                    if (!GenerateInvoiceNo(this.InvHdr.InventoryLocationID.GetValueOrDefault(0), username,  out tm, out newInvoiceNo, out status))
                        throw new Exception(status);

                    tmp.AddRange(tm);
                    this.InvHdr.ShipVia = newInvoiceNo;
                    tmp.Add(InvHdr.GetUpdateCommand(username));
                }

                return tmp;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error Generate Invoice No" + ex.Message);
                status = "Error Generate Invoice No" + ex.Message;

                return tmp;
            }
        }

        public QueryCommandCollection SetInvoiceCreditNoteNo(string username, string newInvoiceNo, out string status)
        {
            status = "";
            QueryCommandCollection tmp = new QueryCommandCollection();
            try
            {                
                QueryCommandCollection tm = new QueryCommandCollection();
                
                tmp.AddRange(tm);
                this.InvHdr.ShipVia = newInvoiceNo;
                tmp.Add(InvHdr.GetUpdateCommand(username));
                
                return tmp;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error Set Invoice No" + ex.Message);
                status = "Error Set Invoice No" + ex.Message;

                return tmp;
            }
        }
    }
}
