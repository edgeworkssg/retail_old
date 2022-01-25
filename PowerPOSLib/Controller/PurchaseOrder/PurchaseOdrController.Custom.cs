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
using System.Drawing;
using System.IO;

namespace PowerPOS
{
    
    [Serializable]
    public partial class PurchaseOdrController
    {
 		#region "PurchaseOrder Movement Type Constant"
        public const string PurchaseOrderMovementType_StockIn = "Stock In";
        public const string PurchaseOrderMovementType_StockOut = "Stock Out";
        public const string PurchaseOrderMovementType_Sales = "Sales";
        public const string PurchaseOrderMovementType_TransferOut = "Transfer Out";
        public const string PurchaseOrderMovementType_TransferIn = "Transfer In";
        public const string PurchaseOrderMovementType_Adjustment = "Adjustment";
        #endregion
        public PurchaseOrderHdr InvHdr; //Save the header information of the Inventory Document
        public PurchaseOrderDetCollection InvDet; //Save the detail information (Item by Item) of the inventory document

        public const string ERR_CLINIC_FROZEN = "This Inventory Location IS frozen. No changes can be made to database.";
        public const string ERR_CLINIC_NOT_FROZEN = "This Inventory Location is NOT frozen. No stock take activity can be made.";
        public const string ERR_CLINIC_DELETED = "This Inventory Location IS deleted.";

        #region "Constructor"
        public PurchaseOdrController()
        {
            //CostingType -> Do not this variable anymore
            //AssignCostingType(CostingType);

            //Create a new inventory objects
            InvHdr = new PurchaseOrderHdr();
            InvHdr.UniqueID = Guid.NewGuid(); //assign a uniqueid
            InvHdr.Userfld8 = AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.PurchaseOrderRole);
            InvHdr.Userfld9 = AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.PurchaseOrderCompany);
            //Create the new default values
            InvHdr.PurchaseOrderDate = DateTime.Now; //date of the documents
            InvHdr.UserName = UserInfo.username; //the name of the user doing it
            InvHdr.Remark = ""; //remark
            InvHdr.FreightCharge = 0; //freight
            InvHdr.Deleted = false; //deleted

            bool useCustomNo = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.UseCustomNo), false);
            bool generateNoInServer = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.GenerateCustomNoInServer), false);
            if (useCustomNo && !generateNoInServer)
                InvHdr.CustomRefNo = CreateNewCustomRefNo(); // custom ref no
            
            InvDet = new PurchaseOrderDetCollection();

        }

        //Load existing saved inventory reference number
        public PurchaseOdrController(string PurchaseOrderHdrRefNo)
        {
            //AssignCostingType(CostingType);

            InvHdr = new PurchaseOrderHdr(PurchaseOrderHdrRefNo);

            //create new inventory reference no....                                 
            InvDet = new PurchaseOrderDetCollection();
            Query qr = PurchaseOrderDet.CreateQuery();
            InvDet.Load(qr.WHERE(PurchaseOrderDet.Columns.PurchaseOrderHdrRefNo, PurchaseOrderHdrRefNo).
                ExecuteDataSet().Tables[0]);
        }

        
        #endregion

        #region "Get ID"

        //Get the maximum Inventory Line ID
        private string GetPurchaseOrderDetMaxID(out string status)
        {
            status = "";
            try
            {
                if (InvDet != null)
                {
                    if (InvDet.Count == 0) return "1";
                    return (int.Parse(InvDet[InvDet.Count - 1].PurchaseOrderDetRefNo.Split('.')[1]) + 1).ToString();
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
        public string GetPurchaseOrderHdrRefNo()
        {
            return InvHdr.PurchaseOrderHdrRefNo;
        }

        public DateTime GetPurchaseOrderDate()
        {
            return InvHdr.PurchaseOrderDate;
        }

        //Get the custom ref no
        public string GetCustomRefNo()
        {
            return InvHdr.CustomRefNo;
        }

        public bool isApproved()
        {
            if (InvHdr.Userfld7 != null && (InvHdr.Userfld7 == "Approved" || InvHdr.Status == "Partially Received"))
                return true;
            else


            return false;
        }

        public String GetRemark()
        {
            return InvHdr.Remark;
        }
        public PurchaseOrderDet GetPurchaseOrderDet(string _PurchaseOrderNo)
        {
            PurchaseOrderDet temp = new PurchaseOrderDet();
            foreach (PurchaseOrderDet tmp1 in InvDet)
            {
                if (tmp1.PurchaseOrderDetRefNo == _PurchaseOrderNo)
                {
                    temp = tmp1;
                }
            }
            return temp;
        }

        #endregion

        #region "Fetch PurchaseOrder Detail Info"

        public bool LoadPurchaseOrder(string RefNo, out string status)
        {
            status = "";
            if (PointOfSaleInfo.IntegrateWithInventory)
            {
                return LoadConfirmedPurchaseOrderControllerForStockIn(RefNo, out status);
            }
            else
            {
                return LoadConfirmedPurchaseOrderControllerFromServer(RefNo, out status);
            }
        }

        public bool LoadConfirmedPurchaseOrderController(string RefNo)
        {
            bool found = false;
            string PurchaseOrderHdrRefNo;
            InvHdr = new PurchaseOrderHdr("CustomRefNo", RefNo);
            if (!InvHdr.IsNew && InvHdr.IsLoaded)
            {
                found = true;
            }
            else
            {
                InvHdr = new PurchaseOrderHdr(RefNo);
                if (!InvHdr.IsNew && InvHdr.IsLoaded)
                {
                    found = true;
                }
            }

            if (found)
            {
                PurchaseOrderHdrRefNo = InvHdr.PurchaseOrderHdrRefNo;
                Query qry = PurchaseOrderDet.CreateQuery();

                InvDet = new PurchaseOrderDetCollection();
                InvDet.Load(qry.WHERE(PurchaseOrderDet.Columns.PurchaseOrderHdrRefNo, PurchaseOrderHdrRefNo).ExecuteDataSet().Tables[0]);
                return true;
            }
                
                
            return false;
        }

        public bool LoadConfirmedPurchaseOrderControllerForStockIn(string RefNo, out string status)
        {
            //bool found = false;
            string PurchaseOrderHdrRefNo;
            status = "";
            //InvHdr = new PurchaseOrderHdr("CustomRefNo", RefNo);

            if (LoadConfirmedPurchaseOrderController(RefNo))
            {
                PurchaseOrderHdrRefNo = InvHdr.PurchaseOrderHdrRefNo;

                if (InvHdr.Status != null && InvHdr.Status.ToUpper() == "POSTED")
                {
                    status = "All of the item in Purchase Order No " + RefNo + " already received.";
                    return false;
                }
                if (InvHdr.Status != null && InvHdr.Status.ToUpper() == "CANCELED")
                {
                    status = "Purchase Order No " + RefNo + " is canceled.";
                    return false;
                }

                Query qry = PurchaseOrderDet.CreateQuery();

                InvDet = new PurchaseOrderDetCollection();
                InvDet.Load(qry.WHERE(PurchaseOrderDet.Columns.PurchaseOrderHdrRefNo, PurchaseOrderHdrRefNo).ExecuteDataSet().Tables[0]);
                ArrayList purchaseOrderDetToDelete = new ArrayList();
                if (InvHdr.Status != null && InvHdr.Status.ToUpper() == "PARTIALLY RECEIVED")
                {
                    foreach (PurchaseOrderDet pod in InvDet)
                    {
                        decimal StockedIn = InventoryController.getQuantityStockedInForPurchaseOrder(pod.PurchaseOrderDetRefNo);
                        if (pod.Quantity <= StockedIn)
                        {
                            purchaseOrderDetToDelete.Add(pod.PurchaseOrderDetRefNo);
                        }
                        else
                        {
                            if (StockedIn > 0)
                            {
                                pod.Quantity = pod.Quantity - StockedIn;
                                status = "Some items in the PO #" + RefNo + " have been received.";
                            }
                        }
                    }
                }
                if (purchaseOrderDetToDelete.Count > 0)
                {
                    foreach (string toDelete in purchaseOrderDetToDelete)
                    {
                        PurchaseOrderDet pod = (PurchaseOrderDet)InvDet.Find(toDelete);
                        InvDet.Remove(pod);
                    }
                }

                InvDet = InvDet.SortByRefNo(); 
                
                return true;
            }
            else
            {
                status = "Purchase Order " + RefNo + " Not Found";
                return false;
            }


            //return false;
        }

        public bool LoadConfirmedPurchaseOrderControllerFromServer(string RefNo, out string status)
        {
            bool found = false;
            string PurchaseOrderHdrRefNo;
            SyncClientController.Load_WS_URL();
            PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
            ws.Timeout = 100000;
            ws.Url = SyncClientController.WS_URL;

            byte[] data = ws.LoadConfirmedPurchaseOrderControllerForStockIn(RefNo, out status);
            if (data != null)
            {
                DataSet ds = SyncClientController.DecompressDataSetFromByteArray(data);
                if (ds.Tables.Count == 2)
                    if (LoadFromDataTable(ds.Tables[0], ds.Tables[1]))
                        found = true;
            }
            if (found)
            {
                
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

        public DataTable FetchUnSavedPurchaseOrderItems(out string status)
        {
            status = "";
            DataTable dt = new DataTable();
            dt.Columns.Add("PurchaseOrderHdrRefNo", typeof(string));
            dt.Columns.Add("PurchaseOrderDetRefNo", typeof(string));
            dt.Columns.Add("ItemNo", typeof(string));
            dt.Columns.Add("ItemName", typeof(string));
            dt.Columns.Add("PackingQuantity", typeof(decimal));
            dt.Columns.Add("PackingSizeName", typeof(string));
            dt.Columns.Add("PackingSizeCost", typeof(decimal));
            dt.Columns.Add("PackingSizeUOM", typeof(decimal));
            dt.Columns.Add("Quantity", typeof(decimal));
            dt.Columns.Add("OnHand", typeof(decimal));
            dt.Columns.Add("SuggestedQty", typeof(decimal));
            dt.Columns.Add("UOM", typeof(string));
            dt.Columns.Add("FactoryPrice", typeof(decimal));
            dt.Columns.Add("RetailPrice", typeof(decimal));
            dt.Columns.Add("CostOfGoods", typeof(decimal));
            dt.Columns.Add("GSTAmount", typeof(decimal));
            dt.Columns.Add("GSTRule", typeof(int));
            dt.Columns.Add("GST", typeof(string));
            dt.Columns.Add("Currency", typeof(string));
            dt.Columns.Add("Remark", typeof(string));
            dt.Columns.Add("Deleted", typeof(bool));
            dt.Columns.Add("PackingSizeF", typeof(string));
            dt.Columns.Add("ItemDescription", typeof(string));
            dt.Columns.Add("ItemImage", typeof(Byte[]));
            dt.Columns.Add("Attributes1", typeof(string));
            dt.Columns.Add("Attributes2", typeof(string));
            dt.Columns.Add("Attributes3", typeof(string));
            dt.Columns.Add("Attributes4", typeof(string));
            dt.Columns.Add("Attributes5", typeof(string));
            dt.Columns.Add("Attributes6", typeof(string));
            dt.Columns.Add("Attributes7", typeof(string));
            dt.Columns.Add("Attributes8", typeof(string));
            dt.Columns.Add("IsDetailDeleted", typeof(bool));
            try
            {
                for (int i = InvDet.Count - 1; i > -1; i--)
                {
                    var newRow = dt.NewRow();
                    Item theItem = new Item(InvDet[i].ItemNo);
                    newRow["PurchaseOrderHdrRefNo"] = InvDet[i].PurchaseOrderHdrRefNo;
                    newRow["PurchaseOrderDetRefNo"] = InvDet[i].PurchaseOrderDetRefNo;
                    newRow["ItemNo"] = theItem.ItemNo;
                    newRow["ItemName"] = theItem.ItemName;
                    //newRow["PackingQuantity"] = InvDet[i].PackingQuantity.GetStringValue("N2");
                    newRow["PackingQuantity"] = InvDet[i].PackingQuantity;
                    newRow["PackingSizeName"] = InvDet[i].PackingSizeName;
                    newRow["PackingSizeCost"] = InvDet[i].PackingSizeCost;
                    newRow["PackingSizeUOM"] = InvDet[i].PackingSizeUOM;
                    newRow["Quantity"] = InvDet[i].Quantity;
                    decimal onHand = ItemSummaryController.FetchStockBalanceByItemNo(InvDet[i].ItemNo,
                        InvHdr.InventoryLocationID.GetValueOrDefault(0));
                    newRow["OnHand"] = onHand;
                    newRow["SuggestedQty"] = 0;
                    ItemQuantityTrigger iq = new ItemQuantityTrigger("ItemNo", InvDet[i].ItemNo);
                    if (iq != null && iq.ItemNo != "")
                        newRow["SuggestedQty"] = (Convert.ToDecimal(iq.TriggerQuantity) - onHand);
                    newRow["UOM"] = theItem.UOM;
                    newRow["FactoryPrice"] = InvDet[i].FactoryPrice;
                    newRow["RetailPrice"] = InvDet[i].RetailPrice;
                    newRow["CostOfGoods"] = InvDet[i].CostOfGoods;
                    newRow["GSTRule"] = InvDet[i].GSTRule;
                    if (InvDet[i].GSTRule == 0)
                        newRow["GST"] = "Non GST";
                    if (InvDet[i].GSTRule == 1)
                        newRow["GST"] = "Exclusive";
                    if (InvDet[i].GSTRule == 2)
                        newRow["GST"] = "Inclusive";
                    newRow["GSTAmount"] = InvDet[i].GSTAmount;
                    if (this.InvHdr.Currency != null)
                        newRow["Currency"] = this.InvHdr.Currency.CurrencyCode;
                    newRow["Remark"] = InvDet[i].Remark;
                    newRow["Deleted"] = InvDet[i].Deleted.GetValueOrDefault(false);
                    newRow["IsDetailDeleted"] = InvDet[i].IsDetailDeleted;

                    string packingF = string.Format("{0} {1}/{2} {3}/${4}",
                        InvDet[i].PackingQuantity.ToString("0.####"),
                        InvDet[i].PackingSizeName,
                        InvDet[i].PackingSizeUOM.ToString("N0"),
                        theItem.UOM,
                        InvDet[i].PackingSizeCost.ToString("N2"));
                    newRow["PackingSizeF"] = packingF;
                    newRow["ItemDescription"] = theItem.ItemDesc == null ? "" : theItem.ItemDesc;

                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.AddItemPicture), false))
                    {
                        //string[] extensions = { "jpg", "png", "bmp", "jpeg" };

                        string itemNo = theItem.ItemNo;
                        var myItem = new Item(Item.Columns.ItemNo, itemNo);
                        string[] extensions = new string[] { "jpg", "gif", "png", "bmp", "jpeg" };

                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.UseImageLocal), false))
                        {
                            string ImagePhotosFolder = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ImageLocalPath);
                            if (!string.IsNullOrEmpty(ImagePhotosFolder) && Directory.Exists(ImagePhotosFolder))
                            {

                                foreach (string ext in extensions)
                                {
                                    string ImagePath = ImagePhotosFolder + itemNo.Trim() + "." + ext;
                                    if (System.IO.File.Exists(ImagePath))
                                    {
                                        newRow["ItemImage"] = ItemController.ResizeImageBiteToArray(Image.FromFile(ImagePath), new Size(40, 40));                                  
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (myItem.ItemImage != null)
                            {
                                newRow["ItemImage"] = myItem.ItemImage;
                            }
                        }
                        
                    }

                    newRow["Attributes1"] = theItem.Attributes1;
                    newRow["Attributes2"] = theItem.Attributes2;
                    newRow["Attributes3"] = theItem.Attributes3;
                    newRow["Attributes4"] = theItem.Attributes4;
                    newRow["Attributes5"] = theItem.Attributes5;
                    newRow["Attributes6"] = theItem.Attributes6;
                    newRow["Attributes7"] = theItem.Attributes7;
                    newRow["Attributes8"] = theItem.Attributes8;
 
                    dt.Rows.Add(newRow);
                }

                status = "";
            }
            catch (Exception ex)
            {
                status = "Error : " + ex.Message;
                Logger.writeLog(ex);
            }

            return dt;
        }

        public DataTable FetchUnSavedPurchaseOrderItemsWithDetailDeleted(out string status)
        {
            status = "";
            DataTable dt = FetchUnSavedPurchaseOrderItems(out status);
            DataTable dtD = new DataTable();

            var dR = dt.Select("IsDetailDeleted = false");
            if (dR.Any())
                dtD = dR.CopyToDataTable();

            return dtD;
        }

        public DataTable FetchUnSavedPurchaseOrderItemsWithDetailDeletedSupplierPortal(string username, string rangeDay, out string status)
        {
            status = "";
            DataTable dt = FetchUnSavedPurchaseOrderItems(out status);
            DataTable dtD = new DataTable();

            var dR = dt.Select("IsDetailDeleted = false");
            if (dR.Any())
                dtD = dR.CopyToDataTable();

            dtD.Columns.Add("Amount", typeof(decimal));
            dtD.Columns.Add("SalesPeriod1", typeof(decimal));
            dtD.Columns.Add("SalesPeriod2", typeof(decimal));
            dtD.Columns.Add("SalesPeriod3", typeof(decimal));

            

            foreach (DataRow dRow in dtD.Rows)
            {
                dRow["Amount"] = dRow["PackingQuantity"].ToString().GetDecimalValue() * dRow["PackingSizeCost"].ToString().GetDecimalValue();
                dRow["SalesPeriod1"] = InventoryController.InventoryFetchItemSales(dRow["ItemNo"].ToString(), this.InvHdr.Supplier.GetIntValue(), "1",
                            rangeDay, InvHdr.InventoryLocationID.GetValueOrDefault(0), username, out status);
                dRow["SalesPeriod2"] = InventoryController.InventoryFetchItemSales(dRow["ItemNo"].ToString(), this.InvHdr.Supplier.GetIntValue(), "2",
                    rangeDay, InvHdr.InventoryLocationID.GetValueOrDefault(0), username, out status);
                dRow["SalesPeriod3"] = InventoryController.InventoryFetchItemSales(dRow["ItemNo"].ToString(), this.InvHdr.Supplier.GetIntValue(), "3",
                    rangeDay, InvHdr.InventoryLocationID.GetValueOrDefault(0), username, out status);
            }

            


            return dtD;
        }

        public DataTable FetchUnSavedInventoryItems(bool displayOnHand, bool displayCostPrice, 
            bool displayPacking, bool displayUOM, bool displayCurrency, out string status)
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
                dTable.Columns.Add("PurchaseOrderDetRefNo");
                dTable.Columns.Add("Deleted", System.Type.GetType("System.Boolean"));
                dTable.Columns.Add("RetailPrice", System.Type.GetType("System.Decimal"));
                dTable.Columns.Add("GSTAmount", System.Type.GetType("System.Decimal"));
                dTable.Columns.Add("GSTRule", typeof(Int32));
                if (displayCostPrice)
                {
                    dTable.Columns.Add("FactoryPrice", System.Type.GetType("System.Decimal"));
                    dTable.Columns.Add("CostOfGoods", System.Type.GetType("System.Decimal"));
                }

                    dTable.Columns.Add("UOM", typeof(string)); 
                    dTable.Columns.Add("Packing", typeof(string));
                    dTable.Columns.Add("Currency", typeof(string)); 
                    dTable.Columns.Add("GST", typeof(string));
                    dTable.Columns.Add("CostPerPackingSize", typeof(decimal));
                    dTable.Columns.Add("PackingSizeUOM", typeof(decimal));

                Item myItem;
                for (int i = InvDet.Count - 1; i > -1; i--)
                {
                    dr = dTable.NewRow();
                    myItem = new Item(InvDet[i].ItemNo);
                    if (displayUOM)
                        dr["UOM"] = myItem.UOM;
                    if (displayPacking)
                        dr["Packing"] = InvDet[i].PackingSizeName;
                    if (displayCurrency)
                        if (this.InvHdr.Currency != null)
                            dr["Currency"] = this.InvHdr.Currency.CurrencyCode;  //InvDet[i].Userfld2;
                    if (InvDet[i].GSTRule == 0)
                        dr["GST"] = "Non GST";
                    if (InvDet[i].GSTRule == 1)
                        dr["GST"] = "Exclusive";
                    if (InvDet[i].GSTRule == 2)
                        dr["GST"] = "Inclusive";
                    dr["GSTAmount"] = InvDet[i].GSTAmount;
                    dr["ItemNo"] = InvDet[i].ItemNo;
                    dr["ItemName"] = myItem.ItemName;
                    dr["Quantity"] = InvDet[i].Quantity;
                    dr["GSTRule"] = InvDet[i].GSTRule;
                    if (displayCostPrice)
                    {
                        dr["FactoryPrice"] = InvDet[i].FactoryPrice;
                        dr["CostOfGoods"] = InvDet[i].CostOfGoods;
                        if (InvDet[i].PackingSizeName != null && InvDet[i].PackingSizeName != "")
                        {
                            dr["CostPerPackingSize"] = InvDet[i].PackingSizeCost;
                            dr["PackingSizeUOM"] = InvDet[i].PackingSizeUOM;
                        }
                        else
                        {
                            dr["CostPerPackingSize"] = 0;
                            dr["PackingSizeUOM"] = 1;
                        }

                    }
                    dr["RetailPrice"] = InvDet[i].RetailPrice;
                    dr["Remark"] = InvDet[i].Remark;
                    dr["PurchaseOrderDetRefNo"] = InvDet[i].PurchaseOrderDetRefNo;
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

                    if (InvDet[i].Deleted.HasValue)
                    {
                        dr["Deleted"] = InvDet[i].Deleted.Value;
                    }
                    else
                    {
                        dr["Deleted"] = false;
                    }

                    dTable.Rows.Add(dr);
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
                dTable.Columns.Add("PurchaseOrderDetRefNo");
                dTable.Columns.Add("Deleted", System.Type.GetType("System.Boolean"));              
                dTable.Columns.Add("RetailPrice", System.Type.GetType("System.Decimal"));
                if (displayCostPrice)
                {
                    dTable.Columns.Add("FactoryPrice", System.Type.GetType("System.Decimal"));
                    dTable.Columns.Add("CostOfGoods", System.Type.GetType("System.Decimal"));
                }
                dTable.Columns.Add("UOM");
                dTable.Columns.Add("Packing Size");
                dTable.Columns.Add("GST", System.Type.GetType("System.Decimal"));
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
                        dr["RetailPrice"] = InvDet[i].RetailPrice;
                        //dr["CostOfGoods"] = InvDet[i].CostOfGoods;
                        dr["Remark"] = InvDet[i].Remark;
                        dr["PurchaseOrderDetRefNo"] = InvDet[i].PurchaseOrderDetRefNo;
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
                        
                        if (InvDet[i].Deleted.HasValue)
                        {
                            dr["Deleted"] = InvDet[i].Deleted.Value;
                        }
                        else
                        {
                            dr["Deleted"] = false;
                        }
                        
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

        public PurchaseOrderDetCollection GetPODetail()
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
        public bool AddItemIntoInventory(string ItemID, int Qty, decimal COGS, out string status)
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
                    PurchaseOrderDet tmp;
                    tmp = new PurchaseOrderDet();
                    tmp.CostOfGoods = COGS;
                    tmp.Discount = 0;
                    tmp.Quantity = Qty;
                    tmp.PurchaseOrderHdrRefNo = InvHdr.PurchaseOrderHdrRefNo;
                    tmp.PurchaseOrderDetRefNo = tmp.PurchaseOrderHdrRefNo + "." + GetPurchaseOrderDetMaxID(out status);

                    if (status != "")
                    {
                        return false;
                    }
                    tmp.ItemNo = ItemID;
                    //tmp.FactoryPrice = tmp.Item.FactoryPrice;
                    if (InvHdr.CurrencyId.HasValue)
                    {
                        tmp.FactoryPrice = ItemController.FetchCostPrice(InvHdr.CurrencyId.Value, ItemID);
                        tmp.RetailPrice = tmp.Item.RetailPrice;
                    }
                    else
                    {
                        tmp.FactoryPrice = tmp.Item.FactoryPrice;
                        tmp.RetailPrice = tmp.Item.RetailPrice;
                    }
                    tmp.UniqueID = Guid.NewGuid();
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

        //Add item into inventory line with Quantity
        public bool AddItemToPurchaseOrderByPackingSize(string itemNo, decimal poQty,
            string packingSizeName, decimal packingSizeUOM, decimal packingSizeCostPrice, 
            int gstType, out string status)
        {
            status = "";
            bool isSuccess = false;
            try
            {
                Item tmpItem = new Item(itemNo);
                if (!tmpItem.IsLoaded)
                {
                    status = "Item with Item No " + itemNo + " does not exist in the system";
                    throw new Exception(status);
                }
                if (!tmpItem.IsInInventory)
                {
                    status = "Item with Item No " + itemNo + " is not an inventory item";
                    throw new Exception(status);
                }

                if (InvDet != null)
                {
                    //int existinvdet = InvDet.Find("ItemNo", tmpItem.ItemNo);
                    int existinvdet = InvDet.ToList<PurchaseOrderDet>().FindIndex(c => c.ItemNo == tmpItem.ItemNo && c.IsDetailDeleted == false);

                    bool allowSameItem = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.AllowSameItemMultipleTimes), false);
                    if (existinvdet != -1 && !allowSameItem && !InvDet[existinvdet].IsDetailDeleted)
                    {
                        InvDet[existinvdet].Quantity = InvDet[existinvdet].Quantity + (poQty * packingSizeUOM.ToString("N0").GetIntValue());
                    }
                    else
                    {
                        PurchaseOrderDet tmp = new PurchaseOrderDet();
                        tmp.PurchaseOrderHdrRefNo = InvHdr.PurchaseOrderHdrRefNo;
                        tmp.PurchaseOrderDetRefNo = tmp.PurchaseOrderHdrRefNo + "." + GetPurchaseOrderDetMaxID(out status);
                        if (status != "")
                            throw new Exception("Unable get PurchaseOrderDetRefNo : " + status);
                        tmp.ItemNo = tmpItem.ItemNo;
                        tmp.PackingQuantity = poQty;
                        tmp.PackingSizeName = packingSizeName;
                        tmp.PackingSizeUOM = packingSizeUOM;
                        tmp.PackingSizeCost = packingSizeCostPrice;
                        tmp.Quantity = poQty * packingSizeUOM.ToString("N0").GetIntValue();
                        if (packingSizeUOM > 0)
                            tmp.FactoryPrice = packingSizeCostPrice / packingSizeUOM;
                        else
                            tmp.FactoryPrice = tmpItem.FactoryPrice;
                        tmp.RetailPrice = tmpItem.RetailPrice;
                        tmp.CostOfGoods = tmp.Quantity.GetValueOrDefault(0) * tmp.FactoryPrice;
                        tmp.GSTRule = gstType;
                        if (tmp.GSTRule == 1)
                            tmp.GSTAmount = (tmp.FactoryPrice * tmp.Quantity.GetValueOrDefault(0)) * (0.07m);
                        else if (tmp.GSTRule == 2)
                            tmp.GSTAmount = ((tmp.FactoryPrice * tmp.Quantity.GetValueOrDefault(0)) / 1.07m) * 0.07m;
                        else
                            tmp.GSTAmount = 0;
                        tmp.Deleted = true;
                        tmp.IsDetailDeleted = false;
                        tmp.UniqueID = Guid.NewGuid();
                        InvDet.Add(tmp);
                    }
                    status = "";
                    isSuccess = true;
                }
                else
                {
                    status = "Inventory Detail has not been initialized.";
                    throw new Exception(status);
                }
                isSuccess = true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = "Error : "+ ex.Message;
                isSuccess = false;
            }
            return isSuccess;
        }

        //Add item into inventory line with Quantity
        public bool AddItemIntoInventorySupplier(string ItemID, int Qty, string SupplierName, int gstType, out string status)
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
                    //int existinvdet = InvDet.Find("ItemNo", tmpItem.ItemNo);
                    int existinvdet = InvDet.ToList<PurchaseOrderDet>().FindIndex(c => c.ItemNo == tmpItem.ItemNo && c.IsDetailDeleted == false);

                    bool allowSameItem = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.AllowSameItemMultipleTimes), false);
                    if (existinvdet != -1 && !allowSameItem && !InvDet[existinvdet].IsDetailDeleted)
                    {
                        InvDet[existinvdet].Quantity = InvDet[existinvdet].Quantity + Qty;
                    }
                    else
                    {
                        PurchaseOrderDet tmp;
                        tmp = new PurchaseOrderDet();
                        tmp.CostOfGoods = 0;
                        tmp.Discount = 0;
                        tmp.Quantity = Qty;
                        tmp.PurchaseOrderHdrRefNo = InvHdr.PurchaseOrderHdrRefNo;
                        tmp.PurchaseOrderDetRefNo = tmp.PurchaseOrderHdrRefNo + "." + GetPurchaseOrderDetMaxID(out status);
                        if (status != "")
                        {
                            return false;
                        }
                        tmp.ItemNo = ItemID;
                        string currency = "";
                        tmp.FactoryPrice = costprice;
                        tmp.GSTRule = tmpItem.GSTRule.GetValueOrDefault(0);
                        if (coll.Count > 0)
                        {
                            tmp.Currency = coll[0].Currency;
                            tmp.PackingSizeName = "";
                            if (coll[0].CostPrice != null)
                            {
                                tmp.FactoryPrice = coll[0].CostPrice;
                            }
                            else
                            {
                                tmp.FactoryPrice = tmp.Item.FactoryPrice;
                            }
                            //tmp.Userfloat3 = coll[0].CostPrice;
                            tmp.GSTRule = coll[0].GSTRule.GetValueOrDefault(0);
                            
                        }
                        if (tmp.GSTRule == 1)
                            tmp.GSTAmount = (tmp.FactoryPrice * tmp.Quantity.GetValueOrDefault(0)) * (0.07m);
                        else if (tmp.GSTRule == 2)
                            tmp.GSTAmount = ((tmp.FactoryPrice * tmp.Quantity.GetValueOrDefault(0)) / 1.07m) * 0.07m;
                        else
                            tmp.GSTAmount = 0;
                        tmp.RetailPrice = tmp.Item.RetailPrice;
                        tmp.Deleted = true;
                        tmp.IsDetailDeleted = false;
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
        public bool AddItemIntoInventory(string ItemID, int Qty, out string status)
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
                if (InvHdr.Supplier == null || InvHdr.Supplier == "")
                {
                    status = "Please Set Supplier";
                    return false;
                }
                ItemSupplierMapCollection ism = new ItemSupplierMapCollection();
                ism.Where(ItemSupplierMap.Columns.ItemNo, ItemID);
                ism.Where(ItemSupplierMap.Columns.SupplierID, InvHdr.Supplier);
                ism.Load();

                //Add into InventoryDet -- Check if already existance and non promo            
                if (InvDet != null)
                {
                    PurchaseOrderDet tmp;
                    tmp = new PurchaseOrderDet();
                    tmp.CostOfGoods = 0;
                    tmp.Discount = 0;
                    tmp.Quantity = Qty;
                    tmp.PurchaseOrderHdrRefNo = InvHdr.PurchaseOrderHdrRefNo;
                    tmp.PurchaseOrderDetRefNo = tmp.PurchaseOrderHdrRefNo + "." + GetPurchaseOrderDetMaxID(out status);
                    if (status != "")
                    {
                        return false;
                    }
                    tmp.ItemNo = ItemID;
                    if (ism.Count > 0)
                    {
                        tmp.Currency = ism[0].Currency;
                        tmp.PackingSizeName = "";
                        if (ism[0].CostPrice != null)
                        {
                            tmp.FactoryPrice = ism[0].CostPrice;
                        }
                        else
                        {
                            tmp.FactoryPrice = tmp.Item.FactoryPrice;
                        }
                        
                    }
                    string currency = "";
                    /*if (InvHdr.CurrencyId.HasValue)
                    { 
                        tmp.FactoryPrice = ItemController.FetchCostPrice(InvHdr.CurrencyId.Value, ItemID);
                    }
                    else
                    {
                        tmp.FactoryPrice = tmp.Item.FactoryPrice;
                    }*/
                    tmp.RetailPrice = tmp.Item.RetailPrice;
                    tmp.UniqueID = Guid.NewGuid();
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

        //Add item into inventory line with Quantity
        public bool AddItemIntoInventory(string ItemID, int Qty, string supplierID, int gstType,out string status)
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
                if (InvHdr.Supplier == null || InvHdr.Supplier == "")
                {
                    status = "Please Set Supplier";
                    return false;
                }
                ItemSupplierMapCollection ism = new ItemSupplierMapCollection();
                ism.Where(ItemSupplierMap.Columns.ItemNo, ItemID);
                ism.Where(ItemSupplierMap.Columns.SupplierID, InvHdr.Supplier);
                ism.Load();

                //Add into InventoryDet -- Check if already existance and non promo            
                if (InvDet != null)
                {
                    PurchaseOrderDet tmp;
                    tmp = new PurchaseOrderDet();
                    tmp.CostOfGoods = 0;
                    tmp.Discount = 0;
                    tmp.Quantity = Qty;
                    tmp.PurchaseOrderHdrRefNo = InvHdr.PurchaseOrderHdrRefNo;
                    tmp.PurchaseOrderDetRefNo = tmp.PurchaseOrderHdrRefNo + "." + GetPurchaseOrderDetMaxID(out status);
                    if (status != "")
                    {
                        return false;
                    }
                    tmp.ItemNo = ItemID;
                    tmp.GSTRule = tmpItem.GSTRule.GetValueOrDefault(0);
                    if (ism.Count > 0)
                    {
                        tmp.Currency = ism[0].Currency;
                        tmp.PackingSizeName = "";
                        if (ism[0].CostPrice != null)
                        {
                            tmp.FactoryPrice = ism[0].CostPrice;
                        }
                        else
                        {
                            tmp.FactoryPrice = tmp.Item.FactoryPrice;
                        }
                        //tmp.Userfloat3 = ism[0].CostPrice;
                        tmp.GSTRule = ism[0].GSTRule.GetValueOrDefault(0);
                        
                    }
                    if (tmp.GSTRule == 1)
                        tmp.GSTAmount = (tmp.FactoryPrice * tmp.Quantity.GetValueOrDefault(0)) * (0.07m);
                    else if (tmp.GSTRule == 2)
                        tmp.GSTAmount = ((tmp.FactoryPrice * tmp.Quantity.GetValueOrDefault(0)) / 1.07m) * 0.07m;
                    else
                        tmp.GSTAmount = 0;
                    string currency = "";
                    /*if (InvHdr.CurrencyId.HasValue)
                    { 
                        tmp.FactoryPrice = ItemController.FetchCostPrice(InvHdr.CurrencyId.Value, ItemID);
                    }
                    else
                    {
                        tmp.FactoryPrice = tmp.Item.FactoryPrice;
                    }*/
                    tmp.RetailPrice = tmp.Item.RetailPrice;
                    tmp.UniqueID = Guid.NewGuid();
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
                    PurchaseOrderDet tmp;
                    tmp = new PurchaseOrderDet();
                    tmp.CostOfGoods = 0;
                    tmp.Discount = 0;
                    tmp.Quantity = Qty;
                    tmp.PurchaseOrderHdrRefNo = InvHdr.PurchaseOrderHdrRefNo;
                    tmp.PurchaseOrderDetRefNo = tmp.PurchaseOrderHdrRefNo + "." + GetPurchaseOrderDetMaxID(out status);
                    if (status != "")
                    {
                        return false;
                    }
                    tmp.ItemNo = ItemID;
                    string currency = "";
                    if (InvHdr.CurrencyId.HasValue)
                    {
                        tmp.FactoryPrice = CPrice;
                    }
                    else
                    {
                        tmp.FactoryPrice = CPrice;
                    }
                    tmp.RetailPrice = tmp.Item.RetailPrice;
                    tmp.UniqueID = Guid.NewGuid();
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
                if (InvHdr.CurrencyId.HasValue)
                {
                    tmp.FactoryPrice = ItemController.FetchCostPrice(InvHdr.CurrencyId.Value, ItemID);
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
                        if (InvHdr.CurrencyId.HasValue)
                        {
                            tmp.FactoryPrice = ItemController.FetchCostPrice(InvHdr.CurrencyId.Value, tmp.ItemNo);
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
                        if (InvHdr.CurrencyId.HasValue)
                        {
                            tmp.FactoryPrice = ItemController.FetchCostPrice(InvHdr.CurrencyId.Value, tmp.ItemNo);
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
        //Delete an item from inventory detail
        public bool DeleteFromPurchaseOrderDetail(string ID, out string status)
        {
            status = "";
            try
            {
                return InvDet.Remove((PowerPOS.PurchaseOrderDet)InvDet.Find(ID));
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

        public bool ChangePOPackingQty(string id, int newQty, out string status)
        {
            bool isSuccess = false;
            status = "";
            try
            {
                PurchaseOrderDet myTmpDet = (PowerPOS.PurchaseOrderDet)InvDet.Find(id);
                if (myTmpDet == null)
                    throw new Exception("Purchase Order Det Not Found!!");
                myTmpDet.PackingQuantity = newQty;
                myTmpDet.Quantity = (myTmpDet.PackingQuantity * myTmpDet.PackingSizeUOM).GetIntValue();
                myTmpDet.CostOfGoods = myTmpDet.FactoryPrice * myTmpDet.Quantity.GetValueOrDefault(0);
                if (myTmpDet.GSTRule == 1)
                    myTmpDet.GSTAmount = (myTmpDet.FactoryPrice * myTmpDet.Quantity.GetValueOrDefault(0)) * (0.07m);
                else if (myTmpDet.GSTRule == 2)
                    myTmpDet.GSTAmount = ((myTmpDet.FactoryPrice * myTmpDet.Quantity.GetValueOrDefault(0)) / 1.07m) * 0.07m;
                else
                    myTmpDet.GSTAmount = 0;
                isSuccess = true;
            }
            catch (Exception ex)
            {
                status = "Error :" + ex.Message;
                isSuccess = true;
                Logger.writeLog(ex);
            }
            return isSuccess;
        }

        public bool ChangePOPackingSize(string id, string packingSizeName, decimal packingSizeUOM, decimal packingSizeCost, out string status)
        {
            bool isSuccess = false;
            status = "";
            try
            {
                PurchaseOrderDet myTmpDet = (PowerPOS.PurchaseOrderDet)InvDet.Find(id);
                if (myTmpDet == null)
                    throw new Exception("Purchase Order Det Not Found!!");

                myTmpDet.PackingSizeName = packingSizeName;
                myTmpDet.PackingSizeUOM = packingSizeUOM;
                myTmpDet.PackingSizeCost = packingSizeCost;
                myTmpDet.Quantity = (myTmpDet.PackingSizeUOM * myTmpDet.PackingQuantity).GetIntValue();
                if (packingSizeUOM > 0)
                    myTmpDet.FactoryPrice = packingSizeCost / packingSizeUOM;
                else
                    myTmpDet.FactoryPrice = myTmpDet.Item.FactoryPrice;

                myTmpDet.CostOfGoods = myTmpDet.FactoryPrice * myTmpDet.Quantity.GetValueOrDefault(0);
                if (myTmpDet.GSTRule == 1)
                    myTmpDet.GSTAmount = (myTmpDet.FactoryPrice * myTmpDet.Quantity.GetValueOrDefault(0)) * (0.07m);
                else if (myTmpDet.GSTRule == 2)
                    myTmpDet.GSTAmount = ((myTmpDet.FactoryPrice * myTmpDet.Quantity.GetValueOrDefault(0)) / 1.07m) * 0.07m;
                else
                    myTmpDet.GSTAmount = 0;
                isSuccess = true;
            }
            catch (Exception ex)
            {
                status = "Error :" + ex.Message;
                isSuccess = true;
                Logger.writeLog(ex);
            }
            return isSuccess;
        }

        public bool ChangePOPackingCost(string id, decimal packingSizeCost, out string status)
        {
            bool isSuccess = false;
            status = "";
            try
            {
                PurchaseOrderDet myTmpDet = (PowerPOS.PurchaseOrderDet)InvDet.Find(id);
                if (myTmpDet == null)
                    throw new Exception("Purchase Order Det Not Found!!");
                myTmpDet.PackingSizeCost = packingSizeCost;
                //if (packingSizeCost > 0)
                    myTmpDet.FactoryPrice = packingSizeCost / myTmpDet.PackingSizeUOM;
                //else
                 /*   if (myTmpDet.Item.UOM == null || myTmpDet.Item.UOM == "")
                        myTmpDet.FactoryPrice = packingSizeCost; 
                    else
                        myTmpDet.FactoryPrice = myTmpDet.Item.FactoryPrice;*/
                myTmpDet.CostOfGoods = myTmpDet.FactoryPrice * myTmpDet.Quantity.GetValueOrDefault(0);
                if (myTmpDet.GSTRule == 1)
                    myTmpDet.GSTAmount = (myTmpDet.FactoryPrice * myTmpDet.Quantity.GetValueOrDefault(0)) * (0.07m);
                else if (myTmpDet.GSTRule == 2)
                    myTmpDet.GSTAmount = ((myTmpDet.FactoryPrice * myTmpDet.Quantity.GetValueOrDefault(0)) / 1.07m) * 0.07m;
                else
                    myTmpDet.GSTAmount = 0;
                isSuccess = true;
            }
            catch (Exception ex)
            {
                status = "Error :" + ex.Message;
                isSuccess = true;
                Logger.writeLog(ex);
            }
            return isSuccess;
        }

        public bool ChangePOQty(string id, int newQty, out string status)
        {
            bool isSuccess = false;
            status = "";
            try
            {
                PurchaseOrderDet myTmpDet = (PowerPOS.PurchaseOrderDet)InvDet.Find(id);
                if (myTmpDet == null)
                    throw new Exception("Purchase Order Det Not Found!!");
                myTmpDet.Quantity = newQty;
                myTmpDet.CostOfGoods = myTmpDet.FactoryPrice * myTmpDet.Quantity.GetValueOrDefault(0);
                if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowPackingSize), false))
                {
                    myTmpDet.PackingQuantity = newQty;
                }
                if (myTmpDet.GSTRule == 1)
                    myTmpDet.GSTAmount = (myTmpDet.FactoryPrice * myTmpDet.Quantity.GetValueOrDefault(0)) * (0.07m);
                else if (myTmpDet.GSTRule == 2)
                    myTmpDet.GSTAmount = ((myTmpDet.FactoryPrice * myTmpDet.Quantity.GetValueOrDefault(0)) / 1.07m) * 0.07m;
                else
                    myTmpDet.GSTAmount = 0;
                isSuccess = true;
            }
            catch (Exception ex)
            {
                status = "Error :" + ex.Message;
                isSuccess = true;
                Logger.writeLog(ex);
            }
            return isSuccess;
        }

        public bool ChangePOTotalCost(string id, decimal newQty, out string status)
        {
            bool isSuccess = false;
            status = "";
            try
            {
                PurchaseOrderDet myTmpDet = (PowerPOS.PurchaseOrderDet)InvDet.Find(id);
                if (myTmpDet == null)
                    throw new Exception("Purchase Order Det Not Found!!");

                myTmpDet.CostOfGoods = newQty;
                //myTmpDet.Quantity = newQty;
                myTmpDet.FactoryPrice = myTmpDet.CostOfGoods / myTmpDet.Quantity.GetValueOrDefault(1);
                if (myTmpDet.PackingQuantity != 0)
                    myTmpDet.PackingSizeCost = myTmpDet.CostOfGoods / myTmpDet.PackingQuantity;
                if (myTmpDet.GSTRule == 1)
                    myTmpDet.GSTAmount = (myTmpDet.FactoryPrice * myTmpDet.Quantity.GetValueOrDefault(0)) * (0.07m);
                else if (myTmpDet.GSTRule == 2)
                    myTmpDet.GSTAmount = ((myTmpDet.FactoryPrice * myTmpDet.Quantity.GetValueOrDefault(0)) / 1.07m) * 0.07m;
                else
                    myTmpDet.GSTAmount = 0;
                isSuccess = true;
            }
            catch (Exception ex)
            {
                status = "Error :" + ex.Message;
                isSuccess = true;
                Logger.writeLog(ex);
            }
            return isSuccess;
        }

        public bool ChangePOFactoryPrice(string id, decimal factoryPrice, out string status)
        {
            bool isSuccess = false;
            status = "";
            try
            {
                PurchaseOrderDet myTmpDet = (PowerPOS.PurchaseOrderDet)InvDet.Find(id);
                if (myTmpDet == null)
                    throw new Exception("Purchase Order Det Not Found!!");
                myTmpDet.FactoryPrice = factoryPrice;
                myTmpDet.CostOfGoods = myTmpDet.FactoryPrice * myTmpDet.Quantity.GetValueOrDefault(0);
                if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowPackingSize), false))
                {
                    myTmpDet.PackingSizeCost = factoryPrice;
                }
                if (myTmpDet.GSTRule == 1)
                    myTmpDet.GSTAmount = (myTmpDet.FactoryPrice * myTmpDet.Quantity.GetValueOrDefault(0)) * (0.07m);
                else if (myTmpDet.GSTRule == 2)
                    myTmpDet.GSTAmount = ((myTmpDet.FactoryPrice * myTmpDet.Quantity.GetValueOrDefault(0)) / 1.07m) * 0.07m;
                else
                    myTmpDet.GSTAmount = 0;
                isSuccess = true;
            }
            catch (Exception ex)
            {
                status = "Error :" + ex.Message;
                isSuccess = true;
                Logger.writeLog(ex);
            }
            return isSuccess;
        }

        public bool ChangeItemQty(string ID, int newQty, out string status)
        {

            try
            {
                status = "";
                PurchaseOrderDet myTmpDet;

                myTmpDet = (PowerPOS.PurchaseOrderDet)InvDet.Find(ID);

                if (myTmpDet == null)
                {
                    status = "Inventory detail has not been created";
                    return false;
                }

                myTmpDet.Quantity = newQty;
                UpdateGST(ID); 
                return true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return false;
            }
        }

        private void UpdateGST(string ID)
        {
            PurchaseOrderDet myTmpDet;

            myTmpDet = (PowerPOS.PurchaseOrderDet)InvDet.Find(ID);

            if (myTmpDet == null)
                return;
            int gstType = myTmpDet.GSTRule;
            if (myTmpDet.PackingSizeName != null && myTmpDet.PackingSizeName != "")
            {
                if (gstType == 1)
                    myTmpDet.GSTAmount = (myTmpDet.PackingSizeCost / myTmpDet.PackingSizeUOM * myTmpDet.Quantity.GetValueOrDefault(0)) * (0.07m);
                else if (gstType == 2)
                    myTmpDet.GSTAmount = ((myTmpDet.PackingSizeCost / myTmpDet.PackingSizeUOM * myTmpDet.Quantity.GetValueOrDefault(0)) / 1.07m) * 0.07m;
                else
                    myTmpDet.GSTAmount = 0;
            }
            else
            {
                if (gstType == 1)
                    myTmpDet.GSTAmount = (myTmpDet.FactoryPrice * myTmpDet.Quantity.GetValueOrDefault(0)) * (0.07m);
                else if (gstType == 2)
                    myTmpDet.GSTAmount = ((myTmpDet.FactoryPrice * myTmpDet.Quantity.GetValueOrDefault(0)) / 1.07m) * 0.07m;
                else
                    myTmpDet.GSTAmount = 0;
            }
        }

        public bool ChangePackageSize(string ID, string newPackage, out string status)
        {

            try
            {
                status = "";
                PurchaseOrderDet myTmpDet;

                myTmpDet = (PowerPOS.PurchaseOrderDet)InvDet.Find(ID);

                if (myTmpDet == null)
                {
                    status = "Inventory detail has not been created";
                    return false;
                }

                myTmpDet.PackingSizeName = newPackage;
                UpdateGST(ID);
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
        //Validate Quantity
        public bool ValidateQuantity(string ID)
        {

            try
            {
                PurchaseOrderDet myTmpDet;

                myTmpDet = (PowerPOS.PurchaseOrderDet)InvDet.Find(ID);

                if (myTmpDet == null)
                    return false;
                 decimal qty = 1;
                 if (myTmpDet.PackingSizeName == null || myTmpDet.PackingSizeName == "")
                     return true;

                 qty = myTmpDet.PackingSizeUOM;
                if (myTmpDet.Quantity % qty == 0)
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);

                return false;
            }
        }

        //Validate Quantity
        public bool ValidateQuantity(string ID, decimal qty)
        {
            
            try
            {
                PurchaseOrderDet myTmpDet;

                myTmpDet = (PowerPOS.PurchaseOrderDet)InvDet.Find(ID);
               
                if (myTmpDet == null)
                    return false;

                if (myTmpDet.Quantity % qty == 0)
                    return true;
                else
                    return false;
    
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
               
                return false;
            }
        }

        //Set Factory Price
        public bool ChangeFactoryPrice(string ID, decimal newFactoryPrice, int gstType, out string status)
        {
            status = "";
            try
            {
                PurchaseOrderDet myTmpDet;
                
                myTmpDet = (PowerPOS.PurchaseOrderDet)InvDet.Find(ID);
                /*gstType = 0;
                if (myTmpDet.Userint1 != null)
                {
                    gstType = (int)myTmpDet.Userint1;
                }*/
                
                if (myTmpDet == null)
                    return false;

                myTmpDet.FactoryPrice = newFactoryPrice;
                if (gstType == 1)
                    myTmpDet.GSTAmount = (myTmpDet.FactoryPrice * myTmpDet.Quantity.GetValueOrDefault(0)) * (0.07m);
                else if (gstType == 2)
                    myTmpDet.GSTAmount = ((myTmpDet.FactoryPrice * myTmpDet.Quantity.GetValueOrDefault(0)) / 1.07m) * 0.07m;
                else
                    myTmpDet.GSTAmount = 0;

                if (myTmpDet.PackingSizeName != null && myTmpDet.PackingSizeName != "")
                {
                    myTmpDet.PackingSizeCost = newFactoryPrice * myTmpDet.PackingSizeUOM;
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

        //Set Factory Price
        public bool ResetFactoryPrice(string ID,out string status)
        {
            status = "";
            try
            {
                PurchaseOrderDet myTmpDet;

                myTmpDet = (PowerPOS.PurchaseOrderDet)InvDet.Find(ID);


                int gstType = myTmpDet.GSTRule;

                if (myTmpDet == null)
                    return false;
                ItemSupplierMapCollection ism = new ItemSupplierMapCollection();
                ism.Where(ItemSupplierMap.Columns.ItemNo, myTmpDet.ItemNo);
                //ism.Where(ItemSupplierMap.Columns.SupplierID, supplier);
                ism.Where(ItemSupplierMap.Columns.Deleted,false);
                ism.Load();

                if (ism.Count > 0)
                {
                    myTmpDet.FactoryPrice = ism[0].CostPrice;
                }
                else
                {
                    Item tmpItem = new Item(myTmpDet.ItemNo);
                    myTmpDet.FactoryPrice = tmpItem.FactoryPrice;
                }
                
                /*if (gstType == 1)
                    myTmpDet.Userfloat1 = (myTmpDet.FactoryPrice * myTmpDet.Quantity) * (0.07m);
                else if (gstType == 2)
                    myTmpDet.Userfloat1 = ((myTmpDet.FactoryPrice * myTmpDet.Quantity) / 1.07m) * 0.07m;
                else
                    myTmpDet.Userfloat1 = 0;*/

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }

        //Set Factory Price
        public bool ChangePackingSizeUOM(string ID, decimal newPackingSizeUOM, out string status)
        {
            status = "";
            try
            {
                PurchaseOrderDet myTmpDet;

                myTmpDet = (PowerPOS.PurchaseOrderDet)InvDet.Find(ID);
                
                if (myTmpDet == null)
                    return false;

                myTmpDet.PackingSizeUOM = newPackingSizeUOM;

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }

        //Set Factory Price
        public bool ChangeCostPerPackingSize(string ID, decimal newCostPerPackingSize, out string status)
        {
            status = "";
            try
            {
                PurchaseOrderDet myTmpDet;

                myTmpDet = (PowerPOS.PurchaseOrderDet)InvDet.Find(ID);

                if (myTmpDet == null)
                    return false;

                myTmpDet.PackingSizeCost = newCostPerPackingSize;
                myTmpDet.FactoryPrice = newCostPerPackingSize / myTmpDet.PackingSizeUOM;

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }

        //Set Retail Price
        public bool ChangeRetailPrice(string ID, decimal newRetailPrice, out string status)
        {
            status = "";
            try
            {
                PurchaseOrderDet myTmpDet;

                myTmpDet = (PowerPOS.PurchaseOrderDet)InvDet.Find(ID);

                if (myTmpDet == null)
                    return false;

                myTmpDet.RetailPrice = newRetailPrice;

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
                PurchaseOrderDet myTmpDet;

                myTmpDet = (PowerPOS.PurchaseOrderDet)InvDet.Find(ID);
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

        public bool SetPOEmailStatusAndSave(bool isSent)
        {
            try
            {
                InvHdr.Userflag5 = isSent;
                InvHdr.Save();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
            return true;
        }

        public bool SetPurchaseOrderHeaderInfo(string Supplier,
            string Remark, decimal freightCharges, double ExchangeRate, decimal Discount)
        {

            //InvHdr.PurchaseOrderNo = PurchaseOrderNo;
            InvHdr.Supplier = Supplier;
            InvHdr.Remark = Remark;
            InvHdr.FreightCharge = freightCharges;
            InvHdr.ExchangeRate = ExchangeRate;
            InvHdr.Discount = Discount;
            return true;
        }

        public bool SetAdditionalHeaderInfo(
            DateTime deliveryDate, string deliveryAddress, string PaymentTerm, string receivingTime, string gstType, decimal minPurchase, decimal delCharge)
        {

            //InvHdr.PurchaseOrderNo = PurchaseOrderNo;
            
            InvHdr.Userfld3 = deliveryDate.ToString("yyyy-MM-dd");
            InvHdr.Userfld4 = deliveryAddress;
            InvHdr.Userfld5 = PaymentTerm;
            InvHdr.Userfld6 = receivingTime;
            InvHdr.Userfld10 = gstType;
            InvHdr.Userfloat1 = minPurchase;
            InvHdr.Userfloat2 = delCharge;
            return true;
        }

        public bool SetCurrency(
            int CurrencyID )
        {

            //InvHdr.PurchaseOrderNo = PurchaseOrderNo;
            InvHdr.CurrencyId = CurrencyID;
            
            return true;
        }

        public bool SetPurchaseOrder(string PurchaseOrderNo)
        {
            InvHdr.PurchaseOrderHdrRefNo = PurchaseOrderNo;

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
            InvHdr.Supplier = Remark;
            return true;
        }

        public bool SetFreightCharges(decimal FreightCharges)
        {
            InvHdr.FreightCharge = FreightCharges;

            return true;
        }

        public bool SetDiscount(decimal Discount)
        {
            InvHdr.Discount = Discount;

            return true;
        }

        public bool SetExchangeRate(double ExchangeRate)
        {
            InvHdr.ExchangeRate = ExchangeRate;

            return true;
        }

        public Guid getUniqueID()
        {
            return InvHdr.UniqueID;
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

        public string getSupplierName()
        {
            Supplier s = new Supplier(InvHdr.Supplier);
            if (s != null && s.IsLoaded)
            {
                return s.SupplierName;
            }
            return "";
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

        public void SetGoodOrderRefNo(string goodOrderRefNo)
        {
            InvHdr.GoodsOrderRefNo = goodOrderRefNo;
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
                InvHdr.CurrencyId = CurrencyID;

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
            if (InvHdr.CurrencyId.HasValue) return InvHdr.CurrencyId.Value;

            return 0;
        }

        public string getGSTType()
        {
            if (InvHdr.Userfld10 != null && InvHdr.Userfld10 != "") return InvHdr.Userfld10;

            return "3";
        }

        public bool hasGSTRule()
        {
            return (InvHdr.Userfld10 != null && InvHdr.Userfld10 != "");
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
                PurchaseOrderHdrCollection tmp = new PurchaseOrderHdrCollection();
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
            InvHdr.UniqueID = Guid.NewGuid();
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
            return InvHdr.InventoryLocation.InventoryLocationName;
        }

        public int GetInventoryLocationID()
        {
            return InvHdr.InventoryLocation.InventoryLocationID;
        }

        public string getSupplier()
        {
            return InvHdr.Supplier;
        }

        public decimal GetFreightCharges()
        {
            if (InvHdr.Userfloat4.HasValue)
                return InvHdr.Userfloat4.Value;
            return 0;
        }

        public string getDeliveryTime()
        {
            if (InvHdr.Userfld3 != null && InvHdr.Userfld3 != "")
                return InvHdr.Userfld3;
            return "";
        }

        public string getDeliveryTimeFormatted()
        {
            if (InvHdr.Userfld3 != null && InvHdr.Userfld3 != "")
            {
                DateTime tmp;
                if (!DateTime.TryParse(InvHdr.Userfld3, out tmp))
                {
                    return InvHdr.Userfld3;
                }
                else
                {
                    return tmp.ToString("dd MMM yyyy");
                }
            }
            return "";
        }

        public string getDeliveryAddress()
        {
            if (InvHdr.Userfld4 != null && InvHdr.Userfld4 != "")
                return InvHdr.Userfld4;
            return "";
        }

        public string getPaymentTerm()
        {
            if (InvHdr.Userfld5 != null && InvHdr.Userfld5 != "")
                return InvHdr.Userfld5;
            return "";
        }

        public string getReceivingTime()
        {
            if (InvHdr.Userfld6 != null && InvHdr.Userfld6 != "")
                return InvHdr.Userfld6;
            return "";
        }


        public decimal GetTotalCost()
        {
            decimal total = 0;
            for (int i = 0; i < InvDet.Count; i++)
            {
                total += InvDet[i].CostOfGoods;
            }
            return total;
        }

        public decimal getDiscount()
        {
            if (InvHdr.Discount.HasValue)
                return InvHdr.Discount.Value;
            return 0;
        }

        public double getExchangeRate()
        {
            return InvHdr.ExchangeRate;
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
            Query qr = PurchaseOrderHdr.CreateQuery();
            qr.AddWhere(PurchaseOrderHdr.Columns.PurchaseOrderHdrRefNo, Comparison.Like, header + "%");
            qr.QueryType = QueryType.Select;
            qr.Top = "1";
            qr.SetSelectList("PurchaseOrderHdrRefNo");
            qr.OrderBy = OrderBy.Desc("PurchaseOrderHdrRefNo");

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

        public static string CreateNewCustomRefNo()
        {
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
                string lastReset = AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.LastReset);
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
                    AppSetting.SetSetting(AppSetting.SettingsName.PurchaseOrder.LastReset, DateTime.Now.ToString("yyMMdd"));
                }

                if (needReset)
                {
                    AppSetting.SetSetting(AppSetting.SettingsName.PurchaseOrder.CurrentNo, "0");
                    AppSetting.SetSetting(AppSetting.SettingsName.PurchaseOrder.LastReset, DateTime.Now.ToString("yyMMdd"));
                }
            }
            #endregion

            int runningNo;
            if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.CurrentNo), out runningNo))
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
            if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.CurrentNo), out runningNo))
            {
                runningNo = 0;
            }
            runningNo += 1;
            AppSetting.SetSetting(AppSetting.SettingsName.PurchaseOrder.CurrentNo, runningNo.ToString());
        }

        public bool CreateOrder(string username, int InventoryLocationID, out string status)
        {
            try
            {
                SupplierItemMap.CreateSupplierItemMapTable();

                QueryCommandCollection cmd;
                cmd = getInsertCommand(username, InventoryLocationID, false);
                SubSonic.DataService.ExecuteTransaction(cmd);

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.UseCustomNo), false))
                {
                    PurchaseOdrController.CustomRefNoUpdate();
                }

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

        public bool CreateEditOrder(string username, int InventoryLocationID, bool IsEdit, out string status)
        {
            try
            {
                SupplierItemMap.CreateSupplierItemMapTable();

                QueryCommandCollection cmd;
                cmd = getInsertCommand(username, InventoryLocationID, IsEdit);
                SubSonic.DataService.ExecuteTransaction(cmd);

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.UseCustomNo), false))
                {
                    PurchaseOdrController.CustomRefNoUpdate();
                }

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


        public QueryCommandCollection getInsertCommand(string username, int InventoryLocationID, bool IsEdit)
        {
            if (IsEdit)
            {
                PurchaseOrderHdr tmpHdr = new PurchaseOrderHdr();
                InvHdr.CopyTo(tmpHdr);
                InvHdr = new PurchaseOrderHdr(tmpHdr.PurchaseOrderHdrRefNo);
                if (InvHdr == null || InvHdr.PurchaseOrderHdrRefNo != tmpHdr.PurchaseOrderHdrRefNo)
                    throw new Exception("(error)Purchase Order Number is not found.");

                foreach (TableSchema.TableColumn col in InvHdr.GetSchema().Columns)
                {
                    if (col.ColumnName != "ModifiedOn" && col.ColumnName != "ModifiedBy")
                    {
                        InvHdr.SetColumnValue(col.ColumnName, tmpHdr.GetColumnValue(col.ColumnName));
                    }
                }
            }

            bool isSellPriceEditable = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.IsSellingPriceEditable), false);
            #region *) Validation: Check whether it is a valid order
            if (InvHdr == null | InvDet == null) //Valid Order?
                throw new Exception("(error)Invalid Inventory data. Unable to receive Inventory.");
            #endregion

            for (int i = 0; i < InvDet.Count; i++)
                if (InvDet[i].Quantity <= 0)
                    throw new Exception("(error)Error: Quantity must be larger than zero.");

            #region *) Conditioning: Set header information
            
            if ((InvHdr.PurchaseOrderHdrRefNo == null || !InvHdr.PurchaseOrderHdrRefNo.StartsWith("ST") || InvHdr.UserName.ToLower() != "system") && !IsEdit)
                InvHdr.PurchaseOrderHdrRefNo = PurchaseOdrController.getNewPurchaseOrderRefNo(InventoryLocationID);
            #endregion
            InvHdr.UserName = username;
            InvHdr.InventoryLocationID = InventoryLocationID;
            InvHdr.Status = "";

            if (!InvHdr.FreightCharge.HasValue) InvHdr.FreightCharge = 0;

            if (string.IsNullOrEmpty(InvHdr.CustomRefNo))
            {
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.UseCustomNo), false))
                    InvHdr.CustomRefNo = CreateNewCustomRefNo();
                else
                    InvHdr.CustomRefNo = InvHdr.PurchaseOrderHdrRefNo;
            }

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

            decimal SumOfFactoryPriceAndQty = 0;
            #region *) Calculate: calculate sum of factory price * quantity for distributing Freight charges
            for (int i = 0; i < InvDet.Count; i++)
                SumOfFactoryPriceAndQty += (decimal)InvDet[i].FactoryPrice * InvDet[i].Quantity.GetValueOrDefault(0);

            if (SumOfFactoryPriceAndQty == 0)
                SumOfFactoryPriceAndQty = 0.0001M;
            #endregion

            int initialIndex = 0;

            string qCount = "select count(PurchaseOrderDetRefNo) as countRow from purchaseorderdet where purchaseorderhdrrefno = '" + InvHdr.PurchaseOrderHdrRefNo + "'";

            initialIndex = (int) DataService.ExecuteScalar(new QueryCommand(qCount));

            string queryUpdate = "Update PurchaseOrderDet set Userflag1 = 1, ModifiedOn = getdate() where PurchaseOrderHdrRefNo = '" + InvHdr.PurchaseOrderHdrRefNo + "'";
            cmd.Add(new QueryCommand(queryUpdate));

            for (int i = 0; i < InvDet.Count; i++)
            {
                #region *) Conditioning: Set detail information
                decimal weight = (InvDet[i].FactoryPrice * InvDet[i].Quantity.GetValueOrDefault(0)) / SumOfFactoryPriceAndQty;

                InvDet[i].PurchaseOrderHdrRefNo = InvHdr.PurchaseOrderHdrRefNo;
                InvDet[i].PurchaseOrderDetRefNo = InvHdr.PurchaseOrderHdrRefNo + "." + (initialIndex + i + 1).ToString();
                InvDet[i].RemainingQty = InvDet[i].Quantity.GetValueOrDefault(0);

                //int GetBalBeforeQty = GetStockBalanceQtyByItemByDate(InvDet[i].ItemNo, InvHdr.InventoryLocationID.Value, DateTime.Now, out status);
                //InvDet[i].BalanceBefore = GetBalBeforeQty;
                //InvDet[i].BalanceAfter = GetBalBeforeQty + InvDet[i].Quantity;

                if (InvDet[i].Quantity > 0)
                {
                    if (InvHdr.ExchangeRate > 0)
                        InvDet[i].FactoryPrice = InvDet[i].FactoryPrice * (decimal)InvHdr.ExchangeRate;

                    InvDet[i].FactoryPrice =
                        InvDet[i].FactoryPrice - ((weight * InvHdr.Discount.Value) / InvDet[i].Quantity.GetValueOrDefault(0));

                    InvDet[i].CostOfGoods = InvDet[i].FactoryPrice
                        + (decimal)((weight * InvHdr.FreightCharge.Value) / InvDet[i].Quantity);
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
                    PurchaseOrderDet pd = new PurchaseOrderDet(InvDet[i].PurchaseOrderDetRefNo);
                    pd.IsDetailDeleted = InvDet[i].IsDetailDeleted;
                    mycmd = pd.GetUpdateCommand(UserInfo.username);
                    cmd.Add(mycmd);
                }
                #endregion

                #region *) Update item supplier map -----  OBSOLETE, MOVED TO PO APPROVAL
                //ItemSupplierMapCollection ism = new ItemSupplierMapCollection();
                //ism.Where(ItemSupplierMap.Columns.SupplierID, int.Parse(InvHdr.Supplier));
                //ism.Where(ItemSupplierMap.Columns.ItemNo, InvDet[i].ItemNo);
                //ism.Load();

                //if (ism.Count > 0)
                //{
                //    bool upd = false;


                //    if (InvDet[i].PackingSizeName != null && InvDet[i].PackingSizeName != "")
                //    {
                //        if (InvDet[i].PackingSizeName == ism[0].PackingSize1)
                //        {
                //            ism[0].CostPrice1 = (decimal)InvDet[i].PackingSizeCost;
                //        }
                //        else if (InvDet[i].PackingSizeName == ism[0].PackingSize2)
                //        {
                //            ism[0].CostPrice2 = (decimal)InvDet[i].PackingSizeCost;
                //        }
                //        else if (InvDet[i].PackingSizeName == ism[0].PackingSize3)
                //        {
                //            ism[0].CostPrice3 = (decimal)InvDet[i].PackingSizeCost;
                //        }
                //        else if (InvDet[i].PackingSizeName == ism[0].PackingSize4)
                //        {
                //            ism[0].CostPrice4 = (decimal)InvDet[i].PackingSizeCost;
                //        }
                //        else if (InvDet[i].PackingSizeName == ism[0].PackingSize5)
                //        {
                //            ism[0].CostPrice5 = (decimal)InvDet[i].PackingSizeCost;
                //        }
                //        else if (InvDet[i].PackingSizeName == ism[0].PackingSize6)
                //        {
                //            ism[0].CostPrice6 = (decimal)InvDet[i].PackingSizeCost;
                //        }
                //        else if (InvDet[i].PackingSizeName == ism[0].PackingSize7)
                //        {
                //            ism[0].CostPrice7 = (decimal)InvDet[i].PackingSizeCost;
                //        }
                //        else if (InvDet[i].PackingSizeName == ism[0].PackingSize8)
                //        {
                //            ism[0].CostPrice8 = (decimal)InvDet[i].PackingSizeCost;
                //        }
                //        else if (InvDet[i].PackingSizeName == ism[0].PackingSize9)
                //        {
                //            ism[0].CostPrice9 = (decimal)InvDet[i].PackingSizeCost;
                //        }
                //        else if (InvDet[i].PackingSizeName == ism[0].PackingSize10)
                //        {
                //            ism[0].CostPrice10 = (decimal)InvDet[i].PackingSizeCost;
                //        }
                //        upd = true;
                //    }
                //    else
                //        if (InvDet[i].FactoryPrice > -1)
                //        {
                //            if ((InvDet[i].FactoryPrice == 0) && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.DontUpdateFactoryPriceIfZero), false))
                //            {
                //                // Don't update
                //            }
                //            else
                //            {
                //                ism[0].CostPrice = (decimal)InvDet[i].FactoryPrice;
                //                upd = true;
                //            }
                //        }

                //    if (upd)
                //        cmd.Add(ism[0].GetUpdateCommand(UserInfo.username));
                //}
                //else
                //{
                //    Item it = new Item(InvDet[i].ItemNo);
                //    if (InvDet[i].FactoryPrice > -1)
                //    {
                //        if ((InvDet[i].FactoryPrice == 0) && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.DontUpdateFactoryPriceIfZero), false))
                //        {
                //            // Don't update
                //        }
                //        else
                //        {
                //            it.FactoryPrice = (decimal)InvDet[i].FactoryPrice;
                //            cmd.Add(it.GetUpdateCommand(UserInfo.username));
                //        }
                //    }
                //}
                #endregion

                #region *) Update Item.RetailPrice
                if (isSellPriceEditable)
                {
                    Item theItem = new Item(InvDet[i].ItemNo);
                    theItem.RetailPrice = InvDet[i].RetailPrice;
                    cmd.Add(theItem.GetUpdateCommand(UserInfo.username));
                }
                #endregion

                #region *) Insert (If Not Exists) SupplierItemMap for this SupplierID and ItemNo

                if (InvHdr.Supplier != string.Empty) // only if Supplier is not string.empty (--select supplier-- means string empty)
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
                ((PowerPOS.PurchaseOrderDet)InvDet.Find(ID)).Deleted = value;
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
                PurchaseOrderDet myTmpDet;

                myTmpDet = (PowerPOS.PurchaseOrderDet)InvDet.Find(ID);

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

                PurchaseOrderHdr po = new PurchaseOrderHdr(InvHdr.PurchaseOrderHdrRefNo);
                if (!po.IsNew)
                    InvHdr.IsNew = false;
                
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
            PurchaseOrderDetCollection beforeModification = new PurchaseOrderDetCollection();
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

        public bool ImportFromDataTableWithPacking(DataTable message, int gstType, int SupplierID, out DataTable ErrorMessage)
        {
            ErrorMessage = CreateCSVImportErrorMessageDataTable();
            PurchaseOrderDetCollection beforeModification = new PurchaseOrderDetCollection();
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

                if (message.Columns.Contains("POQty"))
                { QtyColumnName = "POQty"; }
                else if (message.Columns.Contains("Qty"))
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

                    decimal Qty = 0;
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

                    if (!decimal.TryParse(sQty, out Qty))
                        AddNewImportErrorMessage(Counter, Barcode, ItemNo, sQty, "", "Cannot read Quantity value. Please check the file that you are trying to import.", ref ErrorDb);

                    //if (!decimal.TryParse(CPrice, out CostPrice))
                    //    AddNewImportErrorMessage(Counter, Barcode, ItemNo, CPrice, "", "Cannot read Cost Price value. Please check the file that you are trying to import.", ref ErrorDb);

                    string status;
                    //if (CostPriceColumnName != "")
                    //{
                    //    AddItemIntoInventory1(ItemNo, Qty, CostPrice, out status);
                    //}
                    //else
                    //{
                    //    AddItemIntoInventory(ItemNo, Qty, out status);
                    //}

                    string packingsizeName = Rw["POUOM"].ToString();
                    decimal packingSizeUOM = 0;
                    decimal packingSizeCostPrice = 0;

                    if (!string.IsNullOrEmpty(packingsizeName))
                    {
                        List<PackingSize> pack = PurchaseOrderController.FetchPackingSizeByItemNoAndSupplier(ItemNo, SupplierID);
                        PackingSize p = pack.Where(fn => fn.PackingSizeName.Equals(packingsizeName)).FirstOrDefault();

                        if (p != null)
                        {
                            packingsizeName = p.PackingSizeName;
                            packingSizeUOM = p.PackingSizeUOM;
                            packingSizeCostPrice = p.PackingSizeCostPrice;
                        }
                    }
                    else
                    {
                        Item itm = new Item(ItemNo);
                        packingsizeName = itm.UOM;
                        packingSizeUOM = 1;
                        packingSizeCostPrice = itm.FactoryPrice;
                    }

                    if (!AddItemToPurchaseOrderByPackingSize(ItemNo, Qty, packingsizeName, packingSizeUOM, packingSizeCostPrice, gstType, out status))
                    {
                        if (status != "")
                        {
                            AddNewImportErrorMessage(Counter, Barcode, ItemNo, sQty, "", "Unable to add item. " + status, ref ErrorDb);
                            Logger.writeLog(Counter + " " + BarcodeColumnName + " " + ItemNoColumnName + " " + sQty + " " + "" + " Unable to add item: " + status);
                        }
                    }


                    //if (status != "")
                    //{
                    //    AddNewImportErrorMessage(Counter, Barcode, ItemNo, sQty, "", "Unable to add item. " + status, ref ErrorDb);
                    //    Logger.writeLog(Counter + " " + BarcodeColumnName + " " + ItemNoColumnName + " " + sQty + " " + "" + " Unable to add item: " + status);
                    //}

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

        public static bool UpdatePurchaseOrderStatus(string PurchaseOrderHdrRefNo)
        {
            try
            {
                //PurchaseOrderDetCollection podCol = new PurchaseOrderDetCollection();
                //podCol.Where(PurchaseOrderDet.Columns.PurchaseOrderHdrRefNo, PurchaseOrderHdrRefNo);
                //podCol.Load();//

                #region *) OBSOLETE
                //DataTable dt = new DataTable();
                //string sqlString = "Select PurchaseOrderDetRefNo, Quantity from PurchaseOrderDet where PurchaseOrderHdrRefNo = '" +
                //    PurchaseOrderHdrRefNo + "'";
                //dt = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
                //bool fullyReceived = true;
                //foreach (DataRow pod in dt.Rows)
                //{
                //    decimal totalStockedIn = InventoryController.getQuantityStockedInForPurchaseOrder(pod["PurchaseOrderDetRefNo"].ToString());
                //    decimal qty= 0;
                //    if (decimal.TryParse(pod["Quantity"].ToString(), out qty))
                //    {
                //        if (qty != totalStockedIn)
                //        {
                //            fullyReceived = false;
                //        }
                //    }
                //}
                #endregion

                bool fullyReceived = true;
                DataTable dt = new DataTable();
                string sqlString = @"
                                        IF EXISTS (SELECT * FROM PurchaseOrderDet WHERE ISNULL({2}, 0) = 0 AND PurchaseOrderHdrRefNo = '{0}' AND Quantity > ISNULL({1}, 0))
                                            SELECT CAST(0 AS bit) -- Partially Received
                                        ELSE
                                            SELECT CAST(1 AS bit) -- Fully Received
                                    ";
                sqlString = string.Format(sqlString, PurchaseOrderHdrRefNo, PurchaseOrderDet.UserColumns.QtyReceived, PurchaseOrderDet.UserColumns.IsDetailDeleted);
                object obj = DataService.ExecuteScalar(new QueryCommand(sqlString, "PowerPOS"));
                if (obj != null && obj is bool)
                    fullyReceived = (bool)obj;
                else
                    fullyReceived = false;

                PurchaseOrderHdr poh = new PurchaseOrderHdr(PurchaseOrderHdrRefNo);
                if (poh != null && poh.IsLoaded && poh.PurchaseOrderHdrRefNo != "")
                {
                    if (fullyReceived)
                        poh.Status = "Posted";
                    else
                        poh.Status = "Partially Received";
                    //poh.Save(UserInfo.username);
                    Logger.writeLog(string.Format("Updating PO {0}", PurchaseOrderHdrRefNo));
                    QueryCommand cmd = poh.GetUpdateCommand(UserInfo.username);
                    int res = DataService.ExecuteQuery(cmd);
                    foreach (var param in cmd.Parameters)
                    {
                        Logger.writeLog(string.Format("{0} : {1}", param.ParameterName, param.ParameterValue));
                    }
                    Logger.writeLog(string.Format("Update PO Status to {0} for {1}. Row affected : {2}", poh.Status, PurchaseOrderHdrRefNo, res.ToString()));
                }
                else
                {
                    if (poh == null)
                        Logger.writeLog("Cannot find PO " + PurchaseOrderHdrRefNo + " (poh is null)");
                    else if (poh.PurchaseOrderHdrRefNo == "")
                        Logger.writeLog("Cannot find PO " + PurchaseOrderHdrRefNo + " (poh.PurchaseOrderHdrRefNo = empty)");
                    else if (!poh.IsLoaded)
                        Logger.writeLog("Cannot find PO " + PurchaseOrderHdrRefNo + " (poh.IsLoaded = false)");
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error Update Purchase Order Status. " + ex.Message);
                return false;
            }

        }

        #region Update GST

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
        public bool UpdateGSTForDetail(int gstType)
        {
            LoadGST();
            decimal gstPercentage = (decimal)GST / 100;

            foreach (PurchaseOrderDet pd in InvDet)
            {
                pd.GSTRule = gstType;
                //tmp.GSTRule = gstType;
                if (pd.GSTRule == 1)
                    pd.GSTAmount = (pd.PackingSizeCost * pd.PackingQuantity) * gstPercentage;
                else if (pd.GSTRule == 2)
                    pd.GSTAmount = ((pd.PackingSizeCost * pd.PackingQuantity) / (1 + gstPercentage)) * gstPercentage;
                else
                    pd.GSTAmount = 0; 
            }
            return true;
        }
        #endregion

        public bool GetApprovePOAndUpdateCostPriceCommand(bool updateCostPrice, out string status, out QueryCommandCollection cmdColl)
        {
            bool isSuccess = false;
            status = "";
            cmdColl = new QueryCommandCollection();

            try
            {
                QueryCommandCollection qmc = new QueryCommandCollection();

                string sql = "UPDATE PurchaseOrderHdr SET {0} = '{1}', ModifiedOn = GETDATE(), ModifiedBy = '{2}' WHERE PurchaseOrderHdrRefNo = '{3}'";
                sql = string.Format(sql, PurchaseOrderHdr.UserColumns.Status, "Approved", UserInfo.username, InvHdr.PurchaseOrderHdrRefNo);
                qmc.Add(new QueryCommand(sql, "PowerPOS"));

                if (updateCostPrice)
                {
                    for (int i = 0; i < InvDet.Count; i++)
                    {
                        #region *) Update item supplier map
                        ItemSupplierMapCollection ism = new ItemSupplierMapCollection();
                        ism.Where(ItemSupplierMap.Columns.SupplierID, int.Parse(InvHdr.Supplier));
                        ism.Where(ItemSupplierMap.Columns.ItemNo, InvDet[i].ItemNo);
                        ism.Load();

                        if (ism.Count > 0)
                        {
                            bool upd = false;

                            if (InvDet[i].PackingSizeName != null && InvDet[i].PackingSizeName != "")
                            {
                                if (InvDet[i].PackingSizeName == ism[0].PackingSize1)
                                {
                                    ism[0].CostPrice1 = (decimal)InvDet[i].PackingSizeCost;
                                }
                                else if (InvDet[i].PackingSizeName == ism[0].PackingSize2)
                                {
                                    ism[0].CostPrice2 = (decimal)InvDet[i].PackingSizeCost;
                                }
                                else if (InvDet[i].PackingSizeName == ism[0].PackingSize3)
                                {
                                    ism[0].CostPrice3 = (decimal)InvDet[i].PackingSizeCost;
                                }
                                else if (InvDet[i].PackingSizeName == ism[0].PackingSize4)
                                {
                                    ism[0].CostPrice4 = (decimal)InvDet[i].PackingSizeCost;
                                }
                                else if (InvDet[i].PackingSizeName == ism[0].PackingSize5)
                                {
                                    ism[0].CostPrice5 = (decimal)InvDet[i].PackingSizeCost;
                                }
                                else if (InvDet[i].PackingSizeName == ism[0].PackingSize6)
                                {
                                    ism[0].CostPrice6 = (decimal)InvDet[i].PackingSizeCost;
                                }
                                else if (InvDet[i].PackingSizeName == ism[0].PackingSize7)
                                {
                                    ism[0].CostPrice7 = (decimal)InvDet[i].PackingSizeCost;
                                }
                                else if (InvDet[i].PackingSizeName == ism[0].PackingSize8)
                                {
                                    ism[0].CostPrice8 = (decimal)InvDet[i].PackingSizeCost;
                                }
                                else if (InvDet[i].PackingSizeName == ism[0].PackingSize9)
                                {
                                    ism[0].CostPrice9 = (decimal)InvDet[i].PackingSizeCost;
                                }
                                else if (InvDet[i].PackingSizeName == ism[0].PackingSize10)
                                {
                                    ism[0].CostPrice10 = (decimal)InvDet[i].PackingSizeCost;
                                }
                                upd = true;
                            }
                            else if (InvDet[i].FactoryPrice > -1)
                            {
                                if ((InvDet[i].FactoryPrice == 0) && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.DontUpdateFactoryPriceIfZero), false))
                                {
                                    // Don't update
                                }
                                else
                                {
                                    ism[0].CostPrice = (decimal)InvDet[i].FactoryPrice;
                                    upd = true;
                                }
                            }

                            if (upd)
                                qmc.Add(ism[0].GetUpdateCommand(UserInfo.username));
                        }
                        else
                        {
                            Item it = new Item(InvDet[i].ItemNo);
                            if (InvDet[i].FactoryPrice > -1)
                            {
                                if ((InvDet[i].FactoryPrice == 0) && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.DontUpdateFactoryPriceIfZero), false))
                                {
                                    // Don't update
                                }
                                else
                                {
                                    it.FactoryPrice = (decimal)InvDet[i].FactoryPrice;
                                    qmc.Add(it.GetUpdateCommand(UserInfo.username));
                                }
                            }
                        }
                        #endregion
                    }
                }
                
                cmdColl = qmc;

                isSuccess = true;
            }
            catch (Exception ex)
            {
                isSuccess = false;
                status = "Error Occured : " + ex.Message;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }
                
    }
}
