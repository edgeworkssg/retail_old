using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Transactions;
using System.Collections;
using PowerPOS.Container;
using SubSonic;
using System.ComponentModel;
using System.IO;
using System.Drawing;
namespace PowerPOS
{
    [System.ComponentModel.DataObject]
    [Serializable]
    public partial class InventoryController
    {
        public struct CostingTypes
        {
            public static string FIFO = "fifo";
            public static string FixedAvg = "fixed avg";
            public static string WeightedAvg = "weighted avg";
        }
        public string ActiveCostingType = "";

        #region "Inventory Movement Type Constant"
        public const string InventoryMovementType_StockIn = "Stock In";
        public const string InventoryMovementType_StockOut = "Stock Out";
        public const string InventoryMovementType_Sales = "Sales";
        public const string InventoryMovementType_TransferOut = "Transfer Out";
        public const string InventoryMovementType_TransferIn = "Transfer In";
        public const string InventoryMovementType_AdjustmentIn = "Adjustment In";
        public const string InventoryMovementType_AdjustmentOut = "Adjustment Out";
        public const string InventoryMovementType_ReturnOut = "Return Out";
        public const string InventoryMovementType_ReturnIn = "Return In";
        #endregion


        public InventoryHdr InvHdr; //Save the header information of the Inventory Document
        public InventoryDetCollection InvDet; //Save the detail information (Item by Item) of the inventory document

        #region "Constructor"
        public InventoryController()
        {
            AssignCostingType(PowerPOS.Container.CostingMethods.FixedAvg);

            //Create a new inventory objects
            InvHdr = new InventoryHdr();
            InvHdr.UniqueID = Guid.NewGuid(); //assign a uniqueid

            //Create the new default values
            InvHdr.InventoryDate = DateTime.Now; //date of the documents
            InvHdr.UserName = UserInfo.username; //the name of the user doing it
            InvHdr.Remark = ""; //remark
            InvHdr.FreightCharge = 0; //freight
            InvHdr.Deleted = false; //deleted
            InvDet = new InventoryDetCollection();
        }

        public InventoryController(PowerPOS.Container.CostingMethods CostingType)
        {
            //CostingType -> Do not this variable anymore
            AssignCostingType(CostingType);

            //Create a new inventory objects
            InvHdr = new InventoryHdr();
            InvHdr.UniqueID = Guid.NewGuid(); //assign a uniqueid

            //Create the new default values
            InvHdr.InventoryDate = DateTime.Now; //date of the documents
            InvHdr.UserName = UserInfo.username; //the name of the user doing it
            InvHdr.Remark = ""; //remark
            InvHdr.FreightCharge = 0; //freight
            InvHdr.Deleted = false; //deleted
            InvDet = new InventoryDetCollection();

        }

        //Load existing saved inventory reference number
        public InventoryController(PowerPOS.Container.CostingMethods CostingType, string InventoryRefNo)
        {
            AssignCostingType(CostingType);

            InvHdr = new InventoryHdr(InventoryRefNo);

            //create new inventory reference no....                                 
            InvDet = new InventoryDetCollection();
            Query qr = InventoryDet.CreateQuery();
            InvDet.Load(qr.WHERE(InventoryDet.Columns.InventoryHdrRefNo, InventoryRefNo).
                ExecuteDataSet().Tables[0]);
        }

        private void AssignCostingType(PowerPOS.Container.CostingMethods CostingType)
        {
            string Stg = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
            if (Stg==null || Stg=="")
                ActiveCostingType = CostingTypes.FIFO;
            else if (Stg.ToLower() == "fixed avg")
                ActiveCostingType = CostingTypes.FixedAvg;
            else if (Stg.ToLower() == "weighted avg")
                ActiveCostingType = CostingTypes.WeightedAvg;
            else
                ActiveCostingType = CostingTypes.FIFO;


            //if (CostingType == CostingMethods.FIFO)
            //    ActiveCostingType = CostingTypes.FIFO;
            //else if (CostingType == CostingMethods.FixedAvg)
            //    ActiveCostingType = CostingTypes.FixedAvg;
            //else if (CostingType == CostingMethods.WeightedAvg)
            //    ActiveCostingType = CostingTypes.WeightedAvg;
            //else
            //    ActiveCostingType = CostingTypes.FIFO;
        }
        #endregion

        #region "Get ID"

        //Get the maximum Inventory Line ID
        private string GetInvDetMaxID(out string status)
        {
            status = "";
            try
            {
                if (InvDet != null)
                {
                    if (InvDet.Count == 0) return "1";
                    return (int.Parse(InvDet[InvDet.Count - 1].InventoryDetRefNo.Split('.')[1]) + 1).ToString();
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
        public string GetInvHdrRefNo()
        {
            return InvHdr.InventoryHdrRefNo;
        }

        //Get the custom ref no
        public string GetCustomRefNo()
        {
            return InvHdr.CustomRefNo;
        }

        public int GetGSTRule()
        {
            return InvHdr.GSTRule;
        }

        #endregion
        public static string GetTransferDestination(string InventoryHdrRefNo)
        {
            LocationTransfer lt = new LocationTransfer(LocationTransfer.Columns.FromInventoryHdrRefNo, InventoryHdrRefNo);
            if (lt.IsLoaded && !lt.IsNew)
            {
                return new InventoryLocation(lt.ToInventoryLocationID).InventoryLocationName;
            }
            return "";
        }

        public static string GetSourceDestination(string InventoryHdrRefNo)
        {
            LocationTransfer lt = new LocationTransfer(LocationTransfer.Columns.ToInventoryHdrRefNo, InventoryHdrRefNo);
            if (lt.IsLoaded && !lt.IsNew)
            {
                return lt.FromInventoryHdrRefNo;
            }
            return "";
        }
        #region "Fetch Inventory Detail Info"

        public bool LoadConfirmedInventoryController(string InventoryHdrRefNo)
        {
            InvHdr = new InventoryHdr(InventoryHdrRefNo);
            if (!InvHdr.IsNew && InvHdr.IsLoaded)
            {
                Query qry = InventoryDet.CreateQuery();

                InvDet = new InventoryDetCollection();
                //InvDet.Load(qry.WHERE(InventoryDet.Columns.InventoryHdrRefNo, InventoryHdrRefNo).ExecuteDataSet().Tables[0]);

                string sql = "SELECT * FROM InventoryDet WHERE InventoryHdrRefNo = '{0}' ORDER BY CAST(REPLACE(InventoryDetRefNo, InventoryHdrRefNo + '.', '') AS int)";
                sql = string.Format(sql, InventoryHdrRefNo);
                InvDet.Load(DataService.GetDataSet(new QueryCommand(sql, "PowerPOS")).Tables[0]);

                return true;
            }
            return false;
        }
        public DataTable FetchMergedInventoryItems(bool displayOnHand, bool displayCostPrice, out string status)
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
                                    decimal.Parse(dt.Rows[k]["Quantity"].ToString());
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
                            decimal.Parse(dt.Rows[k]["Quantity"].ToString()) +
                            decimal.Parse(dt.Rows[i]["Quantity"].ToString());

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
                                decimal.Parse(dt.Rows[k]["FactoryPrice"].ToString()) / decimal.Parse(dt.Rows[k]["Quantity"].ToString());
                }
            }
            return dt;
        }

        public DataTable FetchMergedInventoryItems(bool displayOnHand, bool displayCostPrice, bool displayAlternateCostPrice, out string status)
        {
            DataTable dt = FetchUnSavedInventoryItems(displayOnHand, displayCostPrice, displayAlternateCostPrice, out status);

            for (int k = 0; k < dt.Rows.Count; k++)
            {
                if (dt.Rows[k]["Quantity"].ToString() != "0")
                {
                    if (displayAlternateCostPrice)
                    {
                        dt.Rows[k]["FactoryPrice"] =
                                    decimal.Parse(dt.Rows[k]["AlternateCostPrice"].ToString()) *
                                    decimal.Parse(dt.Rows[k]["Quantity"].ToString());
                    }
                    else if (displayCostPrice)
                    {
                        dt.Rows[k]["FactoryPrice"] =
                                    decimal.Parse(dt.Rows[k]["CostOfGoods"].ToString()) *
                                    decimal.Parse(dt.Rows[k]["Quantity"].ToString());
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
                            decimal.Parse(dt.Rows[k]["Quantity"].ToString()) +
                            decimal.Parse(dt.Rows[i]["Quantity"].ToString());

                        if (displayCostPrice || displayAlternateCostPrice)
                        {
                            dt.Rows[k]["FactoryPrice"] =
                                decimal.Parse(dt.Rows[k]["FactoryPrice"].ToString()) + decimal.Parse(dt.Rows[i]["FactoryPrice"].ToString());
                        }
                        
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
                    if (displayCostPrice || displayAlternateCostPrice)
                        dt.Rows[k]["FactoryPrice"] =
                                decimal.Parse(dt.Rows[k]["FactoryPrice"].ToString()) / decimal.Parse(dt.Rows[k]["Quantity"].ToString());
                }
            }
            return dt;
        }
        /*
        //Fetch Current Inventory structures        
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
                dTable.Columns.Add("FactoryPrice", System.Type.GetType("System.Decimal"));
                dTable.Columns.Add("OnHand", System.Type.GetType("System.Int32"));
                dTable.Columns.Add("Remark");
                dTable.Columns.Add("InventoryDetRefNo");
                dTable.Columns.Add("Deleted", System.Type.GetType("System.Boolean"));
                dTable.Columns.Add("CostOfGoods", System.Type.GetType("System.Decimal"));
                Item myItem;
                //map OrderDet
                for (int i = InvDet.Count - 1; i > -1; i--)
                {
                    dr = dTable.NewRow();
                    myItem = new Item(InvDet[i].ItemNo);

                    if (myItem.IsInInventory == true)
                    {
                        dr["ItemNo"] = InvDet[i].ItemNo;
                        dr["ItemName"] = myItem.ItemName;
                        dr["Quantity"] = InvDet[i].Quantity;
                        if (displayCostPrice)
                        {
                            dr["FactoryPrice"] = InvDet[i].FactoryPrice;
                        }
                        else
                        {
                            dr["FactoryPrice"] = 0;
                        }
                        dr["CostOfGoods"] = InvDet[i].CostOfGoods;
                        dr["Remark"] = InvDet[i].Remark;
                        dr["InventoryDetRefNo"] = InvDet[i].InventoryDetRefNo;
                        if (displayOnHand && InvHdr.InventoryLocationID.HasValue)
                        {
                            dr["OnHand"] =
                                    InventoryController.GetStockBalanceQtyByItemByDate
                                    (InvDet[i].ItemNo, InvHdr.InventoryLocationID.Value,
                                    InvHdr.InventoryDate,
                                    out status);
                        }
                        else
                        {
                            dr["OnHand"] = 0;
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
        */

	    public DataTable GetInventoryItemsForExport()
	    {
			var items = new ItemCollection();
		    items.Where(Item.Columns.Deleted, false);
		    items.Load();

		    var itemsByItemNo = new Dictionary<string, Item>();
		    foreach (var item in items)
			    itemsByItemNo.Add(item.ItemNo, item);

			var result = new DataTable();

			result.Columns.Add("ItemNo");
			result.Columns.Add("ItemName");
			result.Columns.Add("BalQty", Type.GetType("System.Int32"));
			result.Columns.Add("Qty", Type.GetType("System.Int32"));
			result.Columns.Add("RetailPrice", Type.GetType("System.Decimal"));
            result.Columns.Add("CostPrice", Type.GetType("System.Decimal"));
			result.Columns.Add("TotalCostPrice", Type.GetType("System.Decimal"));
			result.Columns.Add("Remark");

			for (int i = 0; i < InvDet.Count; i++)
			{
				if (itemsByItemNo.ContainsKey(InvDet[i].ItemNo))
				{
					var myItem = itemsByItemNo[InvDet[i].ItemNo];
					var dr = result.NewRow();
					{
						dr["ItemNo"] = InvDet[i].ItemNo;
						dr["ItemName"] = myItem.ItemName;

						string status;
						dr["BalQty"] = GetStockBalanceQtyByItemByDate(InvDet[i].ItemNo, InvHdr.InventoryLocationID.Value,
						                                               InvHdr.InventoryDate, out status);
						dr["Qty"] = InvDet[i].Quantity;

						dr["CostPrice"] = InvDet[i].FactoryPrice;

						if (InvHdr.MovementType == null || InvHdr.MovementType.EndsWith(" In"))
							dr["TotalCostPrice"] = InvDet[i].CostOfGoods;
						else
							dr["TotalCostPrice"] = FetchCostOfGoodsByItemNo(InvDet[i].ItemNo,
							                                                  InvHdr.InventoryLocationID.GetValueOrDefault(0));

						dr["RetailPrice"] = myItem.RetailPrice;
						dr["Remark"] = InvDet[i].Remark;

						result.Rows.Add(dr);
					}
				}
			}
			return result;
		}

	    public DataTable FetchUnSavedInventoryItems(bool displayOnHand, bool displayCostPrice, out string status)
	    {
		    return FetchUnSavedInventoryItems(displayOnHand, displayCostPrice, out status, 0, 5000, false);
	    }

        public DataTable FetchUnSavedInventoryItems(bool displayOnHand, bool displayCostPrice, bool displayAlternateCostPrice, out string status)
        {
            return FetchUnSavedInventoryItems(displayOnHand, displayCostPrice, out status, 0, 5000, displayAlternateCostPrice);
        }

        public DataTable FetchUnSavedInventoryItems(bool displayOnHand, bool displayCostPrice, out string status, int currentPage, int pageSize)
        {
            return FetchUnSavedInventoryItems(displayOnHand, displayCostPrice, out status, currentPage, pageSize, false);
        }


		public DataTable FetchUnSavedInventoryItems(bool displayOnHand, bool displayCostPrice, out string status, int currentPage, int pageSize, bool displayAlternateCostPrice)
        {
            status = "";
            try
            {
                //create and return a datatable.....
                var dTable = new DataTable();

	            dTable.Columns.Add("ItemNo");
                dTable.Columns.Add("ItemName");
                dTable.Columns.Add("Barcode");
                dTable.Columns.Add("Quantity", Type.GetType("System.Decimal"));
                dTable.Columns.Add("CategoryName");
                dTable.Columns.Add("OnHand", Type.GetType("System.Decimal"));
                dTable.Columns.Add("Remark");
                dTable.Columns.Add("InventoryDetRefNo");
                dTable.Columns.Add("Deleted", Type.GetType("System.Boolean"));              
                dTable.Columns.Add("RetailPrice", Type.GetType("System.Decimal"));
                if (displayCostPrice)
                {
                    dTable.Columns.Add("FactoryPrice", Type.GetType("System.Decimal"));
                    dTable.Columns.Add("CostOfGoods", Type.GetType("System.Decimal"));
                    dTable.Columns.Add("InitialFactoryPrice", Type.GetType("System.Decimal"));
                }
                dTable.Columns.Add("Attributes1");
                dTable.Columns.Add("Attributes2");
                dTable.Columns.Add("Attributes3");
                dTable.Columns.Add("Attributes4");
                dTable.Columns.Add("Attributes5");
                dTable.Columns.Add("Attributes6");
                dTable.Columns.Add("Attributes7");
                dTable.Columns.Add("UOM");
                dTable.Columns.Add("Currency");
                dTable.Columns.Add("ItemDescription");
                dTable.Columns.Add("TotalCostPrice",Type.GetType("System.Decimal"));
                if (displayAlternateCostPrice)
                {
                    dTable.Columns.Add("AlternateCostPrice", Type.GetType("System.Decimal"));
                }
                dTable.Columns.Add("GSTAmount", Type.GetType("System.Double"));
                dTable.Columns.Add("GSTRule", typeof(Int32));
                dTable.Columns.Add("ItemImage", typeof(Byte[]));
				dTable.Columns.Add("SerialNo", typeof(string));
                dTable.Columns.Add("Discount", Type.GetType("System.Decimal"));

                //map OrderDet
				for (int i = Math.Max(0, currentPage * pageSize); i < Math.Min(InvDet.Count, (currentPage + 1) * pageSize); i++)
                {
                    var dr = dTable.NewRow();
                    var myItem = new Item(InvDet[i].ItemNo);

                    //if (myItem.IsInInventory == true)
                    {
                        dr["ItemNo"] = InvDet[i].ItemNo;
                        dr["ItemName"] = myItem.ItemName;
                        dr["Barcode"] = myItem.Barcode;
                        dr["CategoryName"] = myItem.CategoryName;
                        dr["Attributes1"] = myItem.Attributes1;
                        dr["Attributes2"] = myItem.Attributes2;
                        dr["Attributes3"] = myItem.Attributes3;
                        dr["Attributes4"] = myItem.Attributes4;
                        dr["Attributes5"] = myItem.Attributes5;
                        dr["Attributes6"] = myItem.Attributes6;
                        dr["Attributes7"] = myItem.Attributes7;
                        dr["Quantity"] = InvDet[i].Quantity;
                        dr["ItemDescription"] = myItem.ItemDesc;
                        dr["UOM"] = String.IsNullOrEmpty(myItem.Userfld1) ? "" : myItem.Userfld1;
                        dr["Currency"] = InvHdr.Currency == null ? "" :InvHdr.Currency.CurrencyCode;
						if (displayCostPrice)
                        {
                            dr["FactoryPrice"] = InvDet[i].FactoryPrice;
                            if (InvHdr.MovementType == null || InvHdr.MovementType.EndsWith(" In") || InvHdr.MovementType == InventoryController.InventoryMovementType_ReturnOut)
                                dr["CostOfGoods"] = InvDet[i].CostOfGoods;
                            else
                                dr["CostOfGoods"] = FetchCostOfGoodsByItemNo(InvDet[i].ItemNo, InvHdr.InventoryLocationID.GetValueOrDefault(0));
                            dr["InitialFactoryPrice"] = InvDet[i].InitialFactoryPrice.HasValue ? InvDet[i].InitialFactoryPrice.Value : InvDet[i].FactoryPrice;
                        }

						dr["RetailPrice"] = InvDet[i].RetailPrice ?? myItem.RetailPrice;
                        dr["Remark"] = InvDet[i].Remark;
                        dr["InventoryDetRefNo"] = InvDet[i].InventoryDetRefNo;
                        dr["TotalCostPrice"] = InvDet[i].TotalCost.GetValueOrDefault(0);
                        dr["GSTRule"] = InvHdr.GSTRule;
                        dr["GSTAmount"] = InvDet[i].GSTAmount.GetValueOrDefault(0);
                        dr["Discount"] = InvDet[i].Discount.GetValueOrDefault(0);

                        if (displayAlternateCostPrice)
                        {
                            dr["AlternateCostPrice"] = InvDet[i].AlternateCostPrice.GetValueOrDefault(0);
                        }


						if (displayOnHand && InvHdr.InventoryLocationID.HasValue)
						{
                            if (PointOfSaleInfo.IntegrateWithInventory)
                                dr["OnHand"] = GetStockBalanceQtyByItemSummaryByDate(InvDet[i].ItemNo, InvHdr.InventoryLocationID.Value, InvHdr.InventoryDate,
                                                               out status);
                            else
                            {
                                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CheckQuantityonServer), false))
                                {
                                    SyncClientController.Load_WS_URL();
                                    PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                                    ws.Timeout = 100000;
                                    ws.Url = SyncClientController.WS_URL;
                                    dr["OnHand"] = ws.GetStockBalanceQtyByItemSummaryByDate(InvDet[i].ItemNo, InvHdr.InventoryLocationID.Value, InvHdr.InventoryDate,
                                                                       out status);
                                }
                                else 
                                {
                                    dr["OnHand"] = GetStockBalanceQtyByItemSummaryByDate(InvDet[i].ItemNo, InvHdr.InventoryLocationID.Value, InvHdr.InventoryDate,
                                                               out status);
                                }
                                

                                
                            }
						}
                        else
                        {
                            dr["OnHand"] = 0;
                        }

                        if (InvDet[i].Deleted.HasValue)
                        {
                            dr["Deleted"] = InvDet[i].Deleted.Value;
                        }
                        else
                        {
                            dr["Deleted"] = false;
                        }
						dr["SerialNo"] = "-";
                        var serialNo = InvDet[i].SerialNo;
                        if (serialNo != null && serialNo.Count > 0)
                            dr["SerialNo"] = string.Format("{0} item(s)", serialNo.Count);
						

                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.AddItemPicture), false))
                        {
                            //string[] extensions = { "jpg", "png", "bmp", "jpeg" };

                            string itemNo = myItem.ItemNo;
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
                                            dr["ItemImage"] = ItemController.ResizeImageBiteToArray(Image.FromFile(ImagePath), new Size(40, 40)); ;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (myItem.ItemImage != null)
                                {
                                    dr["ItemImage"] = myItem.ItemImage;
                                }
                            }

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

        #endregion

        #region "Add and Delete Item"
        /*
        //Add new item into inventory - set the quantity to be 1
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public bool AddItemIntoInventory(string ItemID, out string status)
        {
            status = "";
            try
            {
                if (!(new Item(ItemID).IsLoaded))
                {
                    status = "Item with Item No " + ItemID + " does not exist in the system";
                    return false;
                }
                if (InvDet != null)
                {
                    InventoryDet tmp;
                    tmp = new InventoryDet();
                    tmp.CostOfGoods = 0;
                    tmp.Discount = 0;
                    tmp.Quantity = 1;
                    tmp.InventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                    tmp.InventoryDetRefNo = tmp.InventoryHdrRefNo + "." + GetInvDetMaxID(out status);
                    if (status != "")
                    {
                        return false;
                    }
                    tmp.ItemNo = ItemID;
                    //tmp.FactoryPrice = tmp.Item.FactoryPrice;
                    if (InvHdr.CurrencyId.HasValue)
                    {
                        tmp.FactoryPrice = ItemController.FetchCostPrice(InvHdr.CurrencyId.Value, ItemID);
                    }
                    else
                    {
                        tmp.FactoryPrice = tmp.Item.FactoryPrice;
                    }

                    InvDet.Add(tmp);
                    return true;
                }
                else
                {
                    status = "Inventory Detail has not been created.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }

        }
        */
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
                    InventoryDet tmp;
                    tmp = new InventoryDet();
                    tmp.Deleted = true;
                    tmp.CostOfGoods = COGS;
                    tmp.Discount = 0;
                    tmp.Quantity = Qty;
                    tmp.InventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                    tmp.InventoryDetRefNo = tmp.InventoryHdrRefNo + "." + GetInvDetMaxID(out status);

                    if (status != "")
                    {
                        return false;
                    }
                    tmp.ItemNo = ItemID;
                    //tmp.FactoryPrice = tmp.Item.FactoryPrice;
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
        public bool AddItemIntoInventory(string orderDetID, string ItemID, decimal Qty, out string status)
        {
            status = "";
            try
            {
                //Add into InventoryDet -- Check if already existance and non promo            
                if (InvDet != null)
                {
                    InventoryDet tmp;
                    tmp = new InventoryDet();

                    tmp.Discount = 0;
                    tmp.Quantity = Qty;
                    tmp.InventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                    tmp.InventoryDetRefNo = tmp.InventoryHdrRefNo + "." + GetInvDetMaxID(out status);
                    if (status != "")
                    {
                        return false;
                    }
                    tmp.ItemNo = ItemID;
                    /*if (!tmp.Item.IsInInventory)
                    {
                        status = "Error, not an inventory item.";
                        return false;
                    }*/
                    tmp.FactoryPrice = tmp.Item.FactoryPrice;
                    tmp.CostOfGoods = (decimal)tmp.Quantity * tmp.FactoryPrice;
                    tmp.UniqueID = Guid.NewGuid();
                    tmp.OrderDetID = orderDetID;
                    tmp.Gst = 0;
                    tmp.RemainingQty = 0;
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

        public bool AddItemIntoInventoryForSales(string ItemID, decimal Qty, decimal COGS, out string status)
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
                    InventoryDet tmp;
                    tmp = new InventoryDet();
                    tmp.Deleted = true;
                    tmp.CostOfGoods = COGS;
                    tmp.Discount = 0;
                    tmp.Quantity = Qty;
                    tmp.InventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                    tmp.InventoryDetRefNo = tmp.InventoryHdrRefNo + "." + GetInvDetMaxID(out status);

                    if (status != "")
                    {
                        return false;
                    }
                    tmp.ItemNo = ItemID;
                    tmp.FactoryPrice = COGS;

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

        public bool AddItemIntoInventoryForSales(string ItemID, int Qty, decimal COGS, out string status)
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
                    InventoryDet tmp;
                    tmp = new InventoryDet();
                    tmp.Deleted = true;
                    tmp.CostOfGoods = COGS;
                    tmp.Discount = 0;
                    tmp.Quantity = Qty;
                    tmp.InventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                    tmp.InventoryDetRefNo = tmp.InventoryHdrRefNo + "." + GetInvDetMaxID(out status);

                    if (status != "")
                    {
                        return false;
                    }
                    tmp.ItemNo = ItemID;
                    tmp.FactoryPrice = COGS;
                    
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
        public bool AddItemIntoInventoryStockTake(string ItemID, decimal Qty, decimal COGS, out string status)
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
                    InventoryDet tmp;
                    tmp = new InventoryDet();
                    tmp.Deleted = true;
                    tmp.CostOfGoods = COGS;
                    tmp.Discount = 0;
                    tmp.Quantity = Qty;
                    tmp.InventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                    tmp.InventoryDetRefNo = tmp.InventoryHdrRefNo + "." + GetInvDetMaxID(out status);

                    if (status != "")
                    {
                        return false;
                    }
                    tmp.ItemNo = ItemID;
                    tmp.FactoryPrice = COGS;
                    
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

        public bool AddItemIntoInventoryStockTakeWithBatchNo(string ItemID, decimal Qty, decimal COGS, string BatchNo, out string status)
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
                    InventoryDet tmp;
                    tmp = new InventoryDet();
                    tmp.Deleted = true;
                    tmp.CostOfGoods = COGS;
                    tmp.Discount = 0;
                    tmp.Quantity = Qty;
                    tmp.InventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                    tmp.InventoryDetRefNo = tmp.InventoryHdrRefNo + "." + GetInvDetMaxID(out status);

                    if (status != "")
                    {
                        return false;
                    }
                    tmp.ItemNo = ItemID;
                    tmp.FactoryPrice = COGS;

                    tmp.Userfld1 = BatchNo;

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

        #region *) Add Item For Stock In 
        //Add item into inventory line with Quantity For Stock In
        public bool AddItemIntoInventoryStockIn(string ItemID, decimal Qty, out string status)
        {
            var Item = new Item(ItemID);
            return AddItemIntoInventoryStockIn(Item, Qty, null, out status);
        }

        public bool AddItemIntoInventoryStockIn(string ItemID, decimal Qty, decimal CostPrice, out string status)
        {
            var Item = new Item(ItemID);
            return AddItemIntoInventoryStockIn(Item, Qty, CostPrice, out status);
        }

        public bool AddItemIntoInventoryStockInFromFile(string ItemID, decimal Qty, decimal CostPrice, out string status)
        {
            var Item = new Item(ItemID);
            return AddItemIntoInventoryStockInFromFile(Item, Qty, CostPrice, out status);
        }

        public decimal FetchCostPriceByItemForStockIn(string ItemNo, string supplier)
        {
            decimal res = 0;
            Item it = new Item(ItemNo);
            if (it != null && it.ItemNo != "")
            {
                res = it.FactoryPrice;
            }
            int temp = 0;
            if (supplier != "" && int.TryParse(supplier, out temp))
            {

                ItemSupplierMapCollection ism = new ItemSupplierMapCollection();
                ism.Where(ItemSupplierMap.Columns.ItemNo, ItemNo);
                ism.Where(ItemSupplierMap.Columns.SupplierID, supplier);
                ism.Load();
                if (ism.Count > 0)
                {
                    res = ism[0].CostPrice;
                }

            }
            return res;
        }

        //Add item into inventory line with Quantity For Stock In
        public bool AddItemIntoInventoryStockIn(Item Item, decimal Qty, decimal? CostPrice, out string status)
        {
            try
            {
                if (!Item.IsLoaded)
                {
                    status = string.Format("Item with Item No {0} does not exist in the system", Item.ItemNo);
                    return false;
                }
                if (!Item.IsInInventory && Item.NonInventoryProduct == false)
                {
                    status = string.Format("Item with Item No {0} is not an inventory item", Item.ItemNo);
                    return false;
                }
                //Add into InventoryDet -- Check if already existance and non promo            
                if (InvDet != null)
                {
                    string ItemNo = Item.NonInventoryProduct ? Item.DeductedItem : Item.ItemNo;

                    int existinvdet = InvDet.Find("ItemNo", ItemNo);
                    if (existinvdet != -1)
                    {
                        decimal newQty = 0;
                        decimal divider = 0;
                        if (Item.NonInventoryProduct)
                        {
                            divider = (Item.DeductConvType ? (1 / Item.DeductConvRate == 0 ? 1 : Item.DeductConvRate) : (Item.DeductConvRate));
                            newQty = Qty * divider;
                        }
                        else
                            newQty = Qty;

                        InvDet[existinvdet].Quantity = InvDet[existinvdet].Quantity + newQty;
                        InvDet[existinvdet].TotalInitialCost = InvDet[existinvdet].FactoryPrice * InvDet[existinvdet].Quantity.GetValueOrDefault(0);
                        InvDet[existinvdet].TotalCost = InvDet[existinvdet].FactoryPrice * InvDet[existinvdet].Quantity;

                    }
                    else
                    {
                        InventoryDet tmp;
                        tmp = new InventoryDet();

                        //set selected
                        tmp.Deleted = true;
                        tmp.CostOfGoods = 0;
                        tmp.Discount = 0;
                        decimal divider = 0;
                        if (Item.NonInventoryProduct)
                        {
                            divider = (Item.DeductConvType ? (1 / Item.DeductConvRate == 0 ? 1 : Item.DeductConvRate) : (Item.DeductConvRate));
                            tmp.Quantity = Qty * divider;
                        }
                        else
                            tmp.Quantity = Qty;
                        tmp.InventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                        tmp.InventoryDetRefNo = tmp.InventoryHdrRefNo + "." + GetInvDetMaxID(out status);
                        if (status != "")
                        {
                            return false;
                        }
                        tmp.ItemNo = Item.NonInventoryProduct ? Item.DeductedItem : Item.ItemNo;

                        #region *) What is this for? Commented for now
                        //// Init WebService
                        //SyncClientController.Load_WS_URL();
                        //PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                        //ws.Url = SyncClientController.WS_URL;
                        #endregion

                        decimal costprice = 0;
                        if (CostPrice.HasValue)
                            costprice = CostPrice.Value;
                        else
                            costprice = FetchCostPriceByItemForStockIn(Item.ItemNo, this.getSupplier() == null || this.getSupplier() == "--Select Supplier--" ? "" : this.getSupplier());
                        tmp.FactoryPrice = Item.NonInventoryProduct ? (costprice / (divider == 0 ? 1 : divider)) : costprice; 

                        tmp.InitialFactoryPrice = tmp.FactoryPrice;
                        tmp.TotalInitialCost = tmp.FactoryPrice * tmp.Quantity.GetValueOrDefault(0);
                        tmp.TotalCost = tmp.FactoryPrice * tmp.Quantity;
                        tmp.UniqueID = Guid.NewGuid();
                        tmp.RetailPrice = Item.RetailPrice;
                        InvDet.Add(tmp);
                    }
                    status = "";
                    return true;
                }

                status = "Inventory Detail has not been initialized.";
                return false;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }

        //Add item into inventory line with Quantity For Stock In
        public bool AddItemIntoInventoryStockInFromFile(Item Item, decimal Qty, decimal CostPrice, out string status)
        {
            try
            {
                if (!Item.IsLoaded)
                {
                    status = string.Format("Item with Item No {0} does not exist in the system", Item.ItemNo);
                    return false;
                }
                if (!Item.IsInInventory && Item.NonInventoryProduct == false)
                {
                    status = string.Format("Item with Item No {0} is not an inventory item", Item.ItemNo);
                    return false;
                }
                //Add into InventoryDet -- Check if already existance and non promo            
                if (InvDet != null)
                {
                    int existinvdet = InvDet.Find("ItemNo", Item.ItemNo);
                    if (existinvdet != -1)
                    {
                        InvDet[existinvdet].Quantity = InvDet[existinvdet].Quantity + Qty;

                    }
                    else
                    {
                        InventoryDet tmp;
                        tmp = new InventoryDet();

                        //set selected
                        tmp.Deleted = true;
                        tmp.CostOfGoods = 0;
                        tmp.Discount = 0;
                        decimal divider = 0;
                        if (Item.NonInventoryProduct)
                        {
                            divider = (Item.DeductConvType ? (1 / Item.DeductConvRate == 0 ? 1 : Item.DeductConvRate) : (Item.DeductConvRate));
                            tmp.Quantity = Qty * divider;
                        }
                        else
                            tmp.Quantity = Qty;
                        tmp.InventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                        tmp.InventoryDetRefNo = tmp.InventoryHdrRefNo + "." + GetInvDetMaxID(out status);
                        if (status != "")
                        {
                            return false;
                        }
                        tmp.ItemNo = Item.NonInventoryProduct ? Item.DeductedItem : Item.ItemNo;
                        // Init WebService
                        SyncClientController.Load_WS_URL();
                        PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                        ws.Url = SyncClientController.WS_URL;

                        decimal costprice = CostPrice != 0 ? CostPrice : FetchCostPriceByItemForStockIn(Item.ItemNo, this.getSupplier() == null || this.getSupplier() == "--Select Supplier--" ? "" : this.getSupplier());
                        tmp.FactoryPrice = Item.NonInventoryProduct ? (costprice / (divider == 0 ? 1 : divider)) : costprice;

                        tmp.InitialFactoryPrice = tmp.FactoryPrice;
                        tmp.TotalInitialCost = tmp.FactoryPrice * tmp.Quantity.GetValueOrDefault(0);
                        tmp.TotalCost = tmp.FactoryPrice * tmp.Quantity;
                        tmp.UniqueID = Guid.NewGuid();
                        InvDet.Add(tmp);
                    }
                    status = "";
                    return true;
                }

                status = "Inventory Detail has not been initialized.";
                return false;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }

        #endregion

        public bool AddItemIntoInventory(string ItemID, int Qty, out string status)
        {
            var Item = new Item(ItemID);
	        return AddItemIntoInventory(Item, Qty, out status);
        }

        public bool AddItemIntoInventory(string ItemID, decimal Qty, out string status)
        {
            var Item = new Item(ItemID);
            return AddItemIntoInventory(Item, Qty, out status);
        }

        public bool AddItemIntoInventoryForStockIn(string ItemID, decimal Qty, decimal CostPrice, string BatchNo, out string status)
        {
            Item Item = new Item(ItemID);

            try
            {
                if (!Item.IsLoaded)
                {
                    status = string.Format("Item with Item No {0} does not exist in the system", Item.ItemNo);
                    return false;
                }
                if (!Item.IsInInventory)
                {
                    status = string.Format("Item with Item No {0} is not an inventory item", Item.ItemNo);
                    return false;
                }
                //Add into InventoryDet -- Check if already existance and non promo            
                if (InvDet != null)
                {
                    int existinvdet = InvDet.Find("ItemNo", Item.ItemNo);
                    if (existinvdet != -1)
                    {
                        InvDet[existinvdet].Quantity = InvDet[existinvdet].Quantity + Qty;

                    }
                    else
                    {
                        InventoryDet tmp;
                        tmp = new InventoryDet();

                        //set selected
                        tmp.Deleted = true;
                        tmp.CostOfGoods = 0;
                        tmp.Discount = 0;
                        tmp.Quantity = Qty;
                        tmp.InventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                        tmp.InventoryDetRefNo = tmp.InventoryHdrRefNo + "." + GetInvDetMaxID(out status);
                        if (status != "")
                        {
                            return false;
                        }
                        tmp.ItemNo = Item.ItemNo;
                        if (InvHdr.CurrencyId.HasValue)
                        {
                            tmp.FactoryPrice = ItemController.FetchCostPrice(InvHdr.CurrencyId.Value, Item.ItemNo);
                        }
                        else
                        {
                            if (InvHdr.InventoryLocationID.GetValueOrDefault(0) != 0)
                            {
                                //Logger.writeLog(">> masuk sini cuy : ItemSummaryController.FetchCostPrice()");
                                tmp.FactoryPrice = ItemSummaryController.FetchCostPrice(Item.ItemNo, InvHdr.InventoryLocationID.GetValueOrDefault(0));
                            }
                            else
                            {
                                //Logger.writeLog(">> masuk sini cuy : (tmp.FactoryPrice = Item.FactoryPrice;)");
                                tmp.FactoryPrice = Item.FactoryPrice;
                            }
                        }
                        //tmp.CostOfGoods = tmp.FactoryPrice;
                        tmp.Userfld1 = BatchNo;

                        tmp.InitialFactoryPrice = tmp.FactoryPrice;
                        tmp.TotalInitialCost = tmp.FactoryPrice * tmp.Quantity.GetValueOrDefault(0);

                        // override costprice
                        if (CostPrice > 0)
                        {
                            tmp.FactoryPrice = CostPrice;
                            tmp.CostOfGoods = CostPrice;
                            tmp.InitialFactoryPrice = tmp.FactoryPrice;
                            tmp.TotalCost = tmp.FactoryPrice * tmp.Quantity;
                        }

                        tmp.UniqueID = Guid.NewGuid();
                        InvDet.Add(tmp);
                    }
                    status = "";
                    return true;
                }

                status = "Inventory Detail has not been initialized.";
                return false;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }

        public bool AddItemIntoInventoryUsingAltCost(string ItemID, decimal Qty, bool usingAltCost, out string status)
        {
            var Item = new Item(ItemID);
            return AddItemIntoInventoryUsingAltCost(Item, Qty, usingAltCost,out status);
        }

        public bool AddItemIntoInventoryForStockOut(string ItemID, decimal Qty, string BatchNo, out string status)
        {
            Item Item = new Item(ItemID);

            try
            {
                if (!Item.IsLoaded)
                {
                    status = string.Format("Item with Item No {0} does not exist in the system", Item.ItemNo);
                    return false;
                }
                if (!Item.IsInInventory && Item.NonInventoryProduct == false)
                {
                    status = string.Format("Item with Item No {0} is not an inventory item", Item.ItemNo);
                    return false;
                }
                //Add into InventoryDet -- Check if already existance and non promo            
                if (InvDet != null)
                {
                    int existinvdet = InvDet.Find("ItemNo", Item.ItemNo);
                    if (existinvdet != -1)
                    {
                        InvDet[existinvdet].Quantity = InvDet[existinvdet].Quantity + Qty;

                    }
                    else
                    {
                        InventoryDet tmp;
                        tmp = new InventoryDet();

                        //set selected
                        tmp.Deleted = true;
                        tmp.CostOfGoods = 0;
                        tmp.Discount = 0;
                        decimal divider = 0;
                        if (Item.NonInventoryProduct)
                        {
                            divider = (Item.DeductConvType ? (1 / Item.DeductConvRate == 0 ? 1 : Item.DeductConvRate) : (Item.DeductConvRate));
                            tmp.Quantity = Qty * divider;
                        }
                        else
                            tmp.Quantity = Qty;
                        tmp.InventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                        tmp.InventoryDetRefNo = tmp.InventoryHdrRefNo + "." + GetInvDetMaxID(out status);
                        if (status != "")
                        {
                            return false;
                        }
                        tmp.ItemNo = Item.NonInventoryProduct ? Item.DeductedItem : Item.ItemNo;
                        tmp.FactoryPrice = ItemSummaryController.GetAvgCostPrice(Item.ItemNo, InvHdr.InventoryLocationID.GetValueOrDefault(0));
                        //tmp.CostOfGoods = tmp.FactoryPrice;
                        tmp.Userfld1 = BatchNo;
                        tmp.UniqueID = Guid.NewGuid();
                        InvDet.Add(tmp);
                    }
                    status = "";
                    return true;
                }

                status = "Inventory Detail has not been initialized.";
                return false;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }

        //Add item into inventory line with Quantity
        public bool AddItemIntoInventory(Item Item, decimal Qty, out string status)
        {
            try
            {
                if (!Item.IsLoaded)
                {
                    status = string.Format("Item with Item No {0} does not exist in the system", Item.ItemNo);
                    return false;
                }
                if (!Item.IsInInventory && Item.NonInventoryProduct == false)
                {
                    status = string.Format("Item with Item No {0} is not an inventory item", Item.ItemNo);
                    return false;
                }
                //Add into InventoryDet -- Check if already existance and non promo            
                if (InvDet != null)
                {
                    int existinvdet = InvDet.Find("ItemNo", Item.ItemNo);
                    if (existinvdet != -1)
                    {
                        decimal newQty = 0;
                        decimal divider = 0;
                        if (Item.NonInventoryProduct)
                        {
                            divider = (Item.DeductConvType ? (1 / Item.DeductConvRate == 0 ? 1 : Item.DeductConvRate) : (Item.DeductConvRate));
                            newQty = Qty * divider;
                        }
                        else
                            newQty = Qty;

                        InvDet[existinvdet].Quantity = InvDet[existinvdet].Quantity + newQty;

                    }
                    else
                    {
                        InventoryDet tmp;
                        tmp = new InventoryDet();

                        //set selected
                        tmp.Deleted = true;
                        tmp.CostOfGoods = 0;
                        tmp.Discount = 0;
                        decimal divider = 0;
                        if (Item.NonInventoryProduct)
                        {
                            divider = (Item.DeductConvType ? (1 / Item.DeductConvRate == 0 ? 1 : Item.DeductConvRate) : (Item.DeductConvRate));
                            tmp.Quantity = Qty * divider;
                        }
                        else
                            tmp.Quantity = Qty;
                        tmp.InventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                        tmp.InventoryDetRefNo = tmp.InventoryHdrRefNo + "." + GetInvDetMaxID(out status);
                        if (status != "")
                        {
                            return false;
                        }
                        tmp.ItemNo = Item.NonInventoryProduct ? Item.DeductedItem : Item.ItemNo;

                        decimal costprice = 0;
                        if (InvHdr.CurrencyId.HasValue)
                        {
                            costprice = ItemController.FetchCostPrice(InvHdr.CurrencyId.Value, Item.ItemNo);
                            tmp.FactoryPrice = Item.NonInventoryProduct ? (costprice / (divider == 0 ? 1 : divider)) : costprice; 
                        }
                        else
                        {
                            if (InvHdr.InventoryLocationID.GetValueOrDefault(0) != 0)
                            {
                                //Logger.writeLog(">> masuk sini cuy : ItemSummaryController.FetchCostPrice()");
                                tmp.FactoryPrice = ItemSummaryController.FetchCostPrice(tmp.Item.ItemNo, InvHdr.InventoryLocationID.GetValueOrDefault(0));
                            }
                            else
                            {
                                //Logger.writeLog(">> masuk sini cuy : (tmp.FactoryPrice = Item.FactoryPrice;)");
                                costprice = Item.FactoryPrice;
                                tmp.FactoryPrice = Item.NonInventoryProduct ? (costprice / (divider == 0 ? 1 : divider)) : costprice; 
                            }
                        }
                        //tmp.CostOfGoods = tmp.FactoryPrice;
                        tmp.UniqueID = Guid.NewGuid();
                        InvDet.Add(tmp);
                    }
                    status = "";
                    return true;
                }

                status = "Inventory Detail has not been initialized.";
                return false;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }

        //Add item into inventory line with Quantity
        public bool AddItemIntoInventory(Item Item, int Qty, out string status)
        {
	        try
            {
                if (!Item.IsLoaded)
                {
                    status = string.Format("Item with Item No {0} does not exist in the system", Item.ItemNo);
                    return false;
                }
                if (!Item.IsInInventory && Item.NonInventoryProduct == false)
                {
                    status = string.Format("Item with Item No {0} is not an inventory item", Item.ItemNo);
                    return false;
                }
                //Add into InventoryDet -- Check if already existance and non promo            
                if (InvDet != null)
                {
                    int existinvdet = InvDet.Find("ItemNo", Item.ItemNo);
                    if (existinvdet != -1)
                    {
                        decimal newQty = 0;
                        decimal divider = 0;
                        if (Item.NonInventoryProduct)
                        {
                            divider = (Item.DeductConvType ? (1 / Item.DeductConvRate == 0 ? 1 : Item.DeductConvRate) : (Item.DeductConvRate));
                            newQty = Qty * divider;
                        }
                        else
                            newQty = Qty;

                        InvDet[existinvdet].Quantity = InvDet[existinvdet].Quantity + newQty;
                        
                    }
                    else
                    {
                        InventoryDet tmp;
                        tmp = new InventoryDet();

                        //set selected
                        tmp.Deleted = true;
                        tmp.CostOfGoods = 0;
                        tmp.Discount = 0;
                        decimal divider = 0;
                        if (Item.NonInventoryProduct)
                        {
                            divider = (Item.DeductConvType ? (1 / Item.DeductConvRate == 0 ? 1 : Item.DeductConvRate) : (Item.DeductConvRate));
                            tmp.Quantity = Qty * divider;
                        }
                        else
                            tmp.Quantity = Qty;
                        tmp.InventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                        tmp.InventoryDetRefNo = tmp.InventoryHdrRefNo + "." + GetInvDetMaxID(out status);
                        if (status != "")
                        {
                            return false;
                        }
                        tmp.ItemNo = Item.NonInventoryProduct ? Item.DeductedItem : Item.ItemNo;
                        decimal costprice = 0;
                        if (InvHdr.CurrencyId.HasValue)
                        {
                            costprice = ItemController.FetchCostPrice(InvHdr.CurrencyId.Value, Item.ItemNo);
                            tmp.FactoryPrice = Item.NonInventoryProduct ? (costprice / (divider == 0 ? 1 : divider)) : costprice; 
                        }
                        else
                        {
                            if (InvHdr.InventoryLocationID.GetValueOrDefault(0) != 0)
                            {
                                //Logger.writeLog(">> masuk sini cuy : ItemSummaryController.FetchCostPrice()");
                                tmp.FactoryPrice = ItemSummaryController.FetchCostPrice(tmp.Item.ItemNo, InvHdr.InventoryLocationID.GetValueOrDefault(0));
                            }
                            else
                            {
                                //Logger.writeLog(">> masuk sini cuy : (tmp.FactoryPrice = Item.FactoryPrice;)");
                                costprice = Item.FactoryPrice;
                                tmp.FactoryPrice = Item.NonInventoryProduct ? (costprice / (divider == 0 ? 1 : divider)) : costprice; 
                            }
                        }
                        //tmp.CostOfGoods = tmp.FactoryPrice;
                        tmp.UniqueID = Guid.NewGuid();
                        InvDet.Add(tmp);
                    }
                    status = "";
                    return true;
                }

	            status = "Inventory Detail has not been initialized.";
	            return false;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }

        //Add item into inventory line with Quantity
        public bool AddItemIntoInventoryUsingAltCost(Item Item, decimal Qty, bool usingAltCost, out string status)
        {
            try
            {
                if (!Item.IsLoaded)
                {
                    status = string.Format("Item with Item No {0} does not exist in the system", Item.ItemNo);
                    return false;
                }
                if (!Item.IsInInventory && Item.NonInventoryProduct == false)
                {
                    status = string.Format("Item with Item No {0} is not an inventory item", Item.ItemNo);
                    return false;
                }
                //Add into InventoryDet -- Check if already existance and non promo            
                if (InvDet != null)
                {
                    string ItemNo = Item.NonInventoryProduct ? Item.DeductedItem : Item.ItemNo;

                    int existinvdet = InvDet.Find("ItemNo", ItemNo);
                    if (existinvdet != -1)
                    {
                        decimal newQty = 0;
                        decimal divider = 0;
                        if (Item.NonInventoryProduct)
                        {
                            divider = (Item.DeductConvType ? (1 / Item.DeductConvRate == 0 ? 1 : Item.DeductConvRate) : (Item.DeductConvRate));
                            newQty = Qty * divider;
                        }
                        else
                            newQty = Qty;

                        InvDet[existinvdet].Quantity = InvDet[existinvdet].Quantity + newQty;
                        InvDet[existinvdet].TotalInitialCost = InvDet[existinvdet].FactoryPrice * InvDet[existinvdet].Quantity.GetValueOrDefault(0);
                        InvDet[existinvdet].TotalCost = InvDet[existinvdet].FactoryPrice * InvDet[existinvdet].Quantity;

                    }
                    else
                    {
                        InventoryDet tmp;
                        tmp = new InventoryDet();

                        //set selected
                        tmp.Deleted = true;
                        tmp.CostOfGoods = 0;
                        tmp.Discount = 0;
                        decimal divider = 0;
                        if (Item.NonInventoryProduct)
                        {
                            divider = (Item.DeductConvType ? (1 / Item.DeductConvRate == 0 ? 1 : Item.DeductConvRate) : (Item.DeductConvRate));
                            tmp.Quantity = Qty * divider;
                        }
                        else
                            tmp.Quantity = Qty;
                        tmp.InventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                        tmp.InventoryDetRefNo = tmp.InventoryHdrRefNo + "." + GetInvDetMaxID(out status);
                        if (status != "")
                        {
                            return false;
                        }
                        
                        tmp.ItemNo = Item.NonInventoryProduct ? Item.DeductedItem : Item.ItemNo;
                        decimal costprice = 0;
                        if (InvHdr.CurrencyId.HasValue)
                        {
                            costprice = ItemController.FetchCostPrice(InvHdr.CurrencyId.Value, Item.ItemNo);
                            tmp.FactoryPrice = Item.NonInventoryProduct ? (costprice / (divider == 0 ? 1 : divider)) : costprice; 
                        }
                        else
                        {
                            if (InvHdr.InventoryLocationID.GetValueOrDefault(0) != 0)
                            {
                                //Logger.writeLog(">> masuk sini cuy : ItemSummaryController.FetchCostPrice()");
                                tmp.FactoryPrice = ItemSummaryController.FetchCostPrice(tmp.ItemNo, InvHdr.InventoryLocationID.GetValueOrDefault(0));
                            }
                            else
                            {
                                //Logger.writeLog(">> masuk sini cuy : (tmp.FactoryPrice = Item.FactoryPrice;)");
                                costprice = Item.FactoryPrice;
                                tmp.FactoryPrice = Item.NonInventoryProduct ? (costprice / (divider == 0 ? 1 : divider)) : costprice; 
                            }
                        }
                        //tmp.CostOfGoods = tmp.FactoryPrice;

                        if (usingAltCost)
                        {
                            costprice = tmp.FactoryPrice;
                            tmp.AlternateCostPrice = Item.NonInventoryProduct ? (costprice / (divider == 0 ? 1 : divider)) : costprice;
                        }
                        tmp.UniqueID = Guid.NewGuid();
                        InvDet.Add(tmp);
                    }
                    status = "";
                    return true;
                }

                status = "Inventory Detail has not been initialized.";
                return false;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }

        //Add item into inventory line with Quantity
        public bool AddItemIntoInventoryStockReturn(Item Item, decimal Qty, decimal? CostPrice, bool usingAltCost, out string status)
        {
            try
            {
                if (!Item.IsLoaded)
                {
                    status = string.Format("Item with Item No {0} does not exist in the system", Item.ItemNo);
                    return false;
                }
                if (!Item.IsInInventory && Item.NonInventoryProduct == false)
                {
                    status = string.Format("Item with Item No {0} is not an inventory item", Item.ItemNo);
                    return false;
                }
                //Add into InventoryDet -- Check if already existance and non promo            
                if (InvDet != null)
                {
                    string ItemNo = Item.NonInventoryProduct ? Item.DeductedItem : Item.ItemNo;

                    int existinvdet = InvDet.Find("ItemNo", ItemNo);
                    if (existinvdet != -1)
                    {
                        decimal newQty = 0;
                        decimal divider = 0;
                        if (Item.NonInventoryProduct)
                        {
                            divider = (Item.DeductConvType ? (1 / Item.DeductConvRate == 0 ? 1 : Item.DeductConvRate) : (Item.DeductConvRate));
                            newQty = Qty * divider;
                        }
                        else
                            newQty = Qty;

                        InvDet[existinvdet].Quantity = InvDet[existinvdet].Quantity + newQty;
                        InvDet[existinvdet].TotalInitialCost = InvDet[existinvdet].FactoryPrice * InvDet[existinvdet].Quantity.GetValueOrDefault(0);
                        InvDet[existinvdet].TotalCost = InvDet[existinvdet].FactoryPrice * InvDet[existinvdet].Quantity;

                    }
                    else
                    {
                        InventoryDet tmp;
                        tmp = new InventoryDet();

                        //set selected
                        tmp.Deleted = true;
                        tmp.CostOfGoods = 0;
                        tmp.Discount = 0;
                        
                        decimal divider = 0;
                        if (Item.NonInventoryProduct)
                        {
                            divider = (Item.DeductConvType ? (1 / Item.DeductConvRate == 0 ? 1 : Item.DeductConvRate) : (Item.DeductConvRate));
                            tmp.Quantity = Qty * divider;
                        }
                        else
                            tmp.Quantity = Qty;

                        tmp.InventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                        tmp.InventoryDetRefNo = tmp.InventoryHdrRefNo + "." + GetInvDetMaxID(out status);
                        if (status != "")
                        {
                            return false;
                        }
                        tmp.ItemNo = Item.NonInventoryProduct ? Item.DeductedItem : Item.ItemNo;

                        if (CostPrice.HasValue)
                        {
                            //tmp.FactoryPrice = CostPrice.Value;
                            tmp.FactoryPrice = Item.NonInventoryProduct ? (CostPrice.Value / (divider == 0 ? 1 : divider)) : CostPrice.Value; 
                        }
                        else
                            tmp.FactoryPrice = FetchCostPriceByItemForStockIn(Item.ItemNo, this.getSupplier() == null || this.getSupplier() == "--Select Supplier--" ? "" : this.getSupplier());
                        
                        //tmp.CostOfGoods = tmp.FactoryPrice;

                        if (usingAltCost)
                        {
                            tmp.AlternateCostPrice = tmp.FactoryPrice;
                        }
                        tmp.InitialFactoryPrice = tmp.FactoryPrice;
                        tmp.TotalInitialCost = tmp.FactoryPrice * tmp.Quantity.GetValueOrDefault(0);
                        tmp.TotalCost = tmp.FactoryPrice * tmp.Quantity;
                        tmp.UniqueID = Guid.NewGuid();
                        InvDet.Add(tmp);
                    }
                    status = "";
                    return true;
                }

                status = "Inventory Detail has not been initialized.";
                return false;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }

        public bool AddItemIntoInventoryStockReturn(string ItemID, decimal Qty, bool usingAltCost, out string status)
        {
            var Item = new Item(ItemID);
            return AddItemIntoInventoryStockReturn(Item, Qty, null, usingAltCost, out status);

        }

        public bool AddItemIntoInventoryStockReturn(string ItemID, decimal Qty, decimal CostPrice, bool usingAltCost, out string status)
        {
            var Item = new Item(ItemID);
            return AddItemIntoInventoryStockReturn(Item, Qty, CostPrice, usingAltCost, out status);

        }

        /// <summary>
        /// Add item for Delivery Stock Out purpose. Will keep reference to OrderDet
        /// </summary>
        /// <param name="ItemID"></param>
        /// <param name="Qty"></param>
        /// <param name="OrderDetID"></param>
        public void AddItemIntoInventoryForDelivery(string ItemID, decimal Qty, string OrderDetID)
        {
            string status = "";
            try
            {
                if (InvDet == null)
                    throw new Exception("(error)Inventory Detail has not been initialized.");

                Item tmpItem = new Item(ItemID);
                if (!tmpItem.IsLoaded)
                    throw new Exception("(warning)Item with Item No " + ItemID + " does not exist in the system");

                if (!tmpItem.IsInInventory && tmpItem.NonInventoryProduct == false)
                    throw new Exception("(warning)Item with Item No " + ItemID + " is not an inventory item");

                //Add into InventoryDet -- Check if already existance and non promo            
                InventoryDet tmp;
                tmp = new InventoryDet();
                tmp.Deleted = true;
                tmp.CostOfGoods = 0;
                tmp.Discount = 0;
                decimal divider = 0;
                if (tmpItem.NonInventoryProduct)
                {
                    divider = (tmpItem.DeductConvType ? (1 / tmpItem.DeductConvRate == 0 ? 1 : tmpItem.DeductConvRate) : (tmpItem.DeductConvRate));
                    tmp.Quantity = Qty * divider;
                }
                else
                    tmp.Quantity = Qty;
                tmp.InventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                tmp.InventoryDetRefNo = tmp.InventoryHdrRefNo + "." + GetInvDetMaxID(out status);
                tmp.DORefNo = OrderDetID;

                if (status != "") throw new Exception(status);

                tmp.ItemNo = tmpItem.NonInventoryProduct ? tmpItem.DeductedItem : tmpItem.ItemNo;
                string currency = "";
                decimal costprice = 0;
                if (InvHdr.CurrencyId.HasValue)
                {
                    costprice = ItemController.FetchCostPrice(InvHdr.CurrencyId.Value, ItemID);
                }
                else
                {
                    costprice = tmp.Item.FactoryPrice;
                }
                tmp.FactoryPrice = tmpItem.NonInventoryProduct ? (costprice / (divider == 0 ? 1 : divider)) : costprice; 
                tmp.UniqueID = Guid.NewGuid();
                InvDet.Add(tmp);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                throw ex;
            }

        }

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
        public bool DeleteFromInventoryDetail(string ID, out string status)
        {
            status = "";
            try
            {
                return InvDet.Remove((PowerPOS.InventoryDet)InvDet.Find(ID));
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }

        //Add item into inventory line with Quantity
        public bool AddItemIntoInventoryWithRetailPrice(string ItemID, int Qty, decimal COGS, decimal retailPrice, out string status)
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
                    InventoryDet tmp;
                    tmp = new InventoryDet();
                    tmp.Deleted = true;
                    tmp.CostOfGoods = COGS;
                    tmp.Discount = 0;
                    tmp.Quantity = Qty;
                    tmp.InventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                    tmp.InventoryDetRefNo = tmp.InventoryHdrRefNo + "." + GetInvDetMaxID(out status);
                    tmp.Userfloat2 = retailPrice;
                    if (status != "")
                    {
                        return false;
                    }
                    tmp.ItemNo = ItemID;
                    //tmp.FactoryPrice = tmp.Item.FactoryPrice;
                    if (InvHdr.CurrencyId.HasValue)
                    {
                        tmp.FactoryPrice = COGS;
                    }
                    else
                    {
                        tmp.FactoryPrice = COGS;
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
        public bool AddItemIntoInventoryWithRetailPriceForPO(string ItemID, decimal Qty, decimal COGS, decimal retailPrice, string PurchaseOrderDetRefNo, decimal totalCost, out string status)
        {
            status = "";
            try
            {
                Item tmpItem = new Item(ItemID);

                if (!(tmpItem.IsLoaded))
                {
                    status = "Item with Item No " + ItemID + " does not exist in the system";
                    return false;
                }
                //Add into InventoryDet -- Check if already existance and non promo            
                if (InvDet != null)
                {
                    InventoryDet tmp;
                    tmp = new InventoryDet();
                    tmp.Deleted = true;
                    tmp.CostOfGoods = COGS;
                    tmp.Discount = 0;
                    decimal divider = 0;
                    if (tmpItem.NonInventoryProduct)
                    {
                        divider = (tmpItem.DeductConvType ? (1 / tmpItem.DeductConvRate == 0 ? 1 : tmpItem.DeductConvRate) : (tmpItem.DeductConvRate));
                        tmp.Quantity = Qty * divider;
                    }
                    else
                        tmp.Quantity = Qty;
                    tmp.InventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                    tmp.InventoryDetRefNo = tmp.InventoryHdrRefNo + "." + GetInvDetMaxID(out status);
                    tmp.Userfloat2 = retailPrice;
                    tmp.TotalInitialCost = totalCost;
                    tmp.TotalCost = totalCost;
                    if (status != "")
                    {
                        return false;
                    }
                    tmp.ItemNo = ItemID;
                    //tmp.FactoryPrice = tmp.Item.FactoryPrice;
                    //if (InvHdr.CurrencyId.HasValue)
                    //{
                    //    tmp.FactoryPrice = COGS;
                    //}
                    //else
                    //{
                    //    tmp.FactoryPrice = COGS;
                    //}
                    tmp.FactoryPrice = tmpItem.NonInventoryProduct ? (COGS / (divider == 0 ? 1 : divider)) : COGS;
                    tmp.InitialFactoryPrice = tmpItem.NonInventoryProduct ? (COGS / (divider == 0 ? 1 : divider)) : COGS;
                    tmp.UniqueID = Guid.NewGuid();
                    tmp.PurchaseOrderDetRefNo = PurchaseOrderDetRefNo;
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

        #endregion

        #region "Change Item Attributes"

        //Set Quantity
        public bool ChangeItemQty(string ID, decimal newQty, out string status)
        {

            try
            {
                status = "";
                InventoryDet myTmpDet;

                myTmpDet = (PowerPOS.InventoryDet)InvDet.Find(ID);

                if (myTmpDet == null)
                {
                    status = "Inventory detail has not been created";
                    return false;
                }

                myTmpDet.Quantity = newQty;

                return true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return false;
            }
        }

        public bool ChangeItemQtyStockIn(string ID, decimal newQty, out string status)
        {

            try
            {
                status = "";
                InventoryDet myTmpDet;

                myTmpDet = (PowerPOS.InventoryDet)InvDet.Find(ID);

                if (myTmpDet == null)
                {
                    status = "Inventory detail has not been created";
                    return false;
                }

                myTmpDet.Quantity = newQty;
                myTmpDet.TotalInitialCost = newQty * myTmpDet.InitialFactoryPrice.GetValueOrDefault(0);
                myTmpDet.TotalCost = newQty * myTmpDet.FactoryPrice;

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
                InventoryDet myTmpDet;

                myTmpDet = (PowerPOS.InventoryDet)InvDet.Find(ID);

                if (myTmpDet == null)
                    return false;

                myTmpDet.FactoryPrice = newFactoryPrice;
                myTmpDet.InitialFactoryPrice = newFactoryPrice;
                myTmpDet.TotalInitialCost = newFactoryPrice * myTmpDet.Quantity.GetValueOrDefault(0);
                myTmpDet.TotalCost = newFactoryPrice * myTmpDet.Quantity.GetValueOrDefault(0);

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
        public bool ChangeTotalCostPrice(string ID, decimal newTotalCost, out string status)
        {
            status = "";
            try
            {
                InventoryDet myTmpDet;

                myTmpDet = (PowerPOS.InventoryDet)InvDet.Find(ID);

                if (myTmpDet == null)
                    return false;

                myTmpDet.TotalCost = newTotalCost;
                myTmpDet.TotalInitialCost = newTotalCost;
                myTmpDet.FactoryPrice = newTotalCost / myTmpDet.Quantity.GetValueOrDefault(1);
                myTmpDet.FactoryPrice = Math.Round(myTmpDet.FactoryPrice / (1 - (myTmpDet.Discount.GetValueOrDefault(0) / 100)), 4);
                myTmpDet.InitialFactoryPrice = myTmpDet.FactoryPrice;

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
        public bool ChangeAlternateCostPrice(string ID, decimal newFactoryPrice, out string status)
        {
            status = "";
            try
            {
                InventoryDet myTmpDet;

                myTmpDet = (PowerPOS.InventoryDet)InvDet.Find(ID);

                if (myTmpDet == null)
                    return false;

                myTmpDet.AlternateCostPrice = newFactoryPrice;
                myTmpDet.TotalCost = newFactoryPrice * myTmpDet.Quantity.GetValueOrDefault(0);

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
        public bool ChangeRetailPrice(string ID, decimal newPrice, out string status)
        {
            status = "";
            try
            {
                InventoryDet myTmpDet;

                myTmpDet = (PowerPOS.InventoryDet)InvDet.Find(ID);

                if (myTmpDet == null)
                    return false;

                myTmpDet.RetailPrice = newPrice;

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }

        public bool ChangeLineDiscount(string ID, decimal discount, out string status)
        {
            status = "";
            try
            {
                InventoryDet myTmpDet;

                myTmpDet = (PowerPOS.InventoryDet)InvDet.Find(ID);

                if (myTmpDet == null)
                    return false;

                myTmpDet.Discount = discount;

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }
        
        /*
        //Set Factory Price
        public bool ChangeFactoryPriceByItemNo(string ItemNo, decimal newFactoryPrice, out string status)
        {
            status = "";
            try
            {
                for (int i = 0; i < InvDet.Count; i++)
                {
                    if (InvDet[i].ItemNo == ItemNo)
                    {
                        InvDet[i].FactoryPrice = newFactoryPrice;
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
        */

        //Set Remarks
        public bool ChangeItemRemark(string ID, string Remark, out string status)
        {
            status = "";
            try
            {
                InventoryDet myTmpDet;

                myTmpDet = (PowerPOS.InventoryDet)InvDet.Find(ID);
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

        //Set Remark
        public bool ChangeStockInRefNoByItemNo(string ItemNo, string StockInRefNo, out string status)
        {
            status = "";
            try
            {
                for (int i = 0; i < InvDet.Count; i++)
                {
                    if (InvDet[i].ItemNo == ItemNo)
                    {
                        InvDet[i].StockInRefNo = StockInRefNo;
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

        public bool SetInventoryHeaderInfo(string PurchaseOrderNo, string Supplier,
            string Remark, decimal freightCharges, double ExchangeRate, decimal Discount)
        {

            InvHdr.PurchaseOrderNo = PurchaseOrderNo;
            InvHdr.Supplier = Supplier;
            InvHdr.Remark = Remark;
            InvHdr.FreightCharge = freightCharges;
            InvHdr.ExchangeRate = ExchangeRate;
            InvHdr.Discount = Discount;
            return true;
        }

        public bool SetInventoryHeaderAdditionalInfo(string customField1, string customField2, string customField3,
            string customField4, string customField5, decimal additionalCost1, decimal additionalCost2,
            decimal additionalCost3, decimal additionalCost4, decimal additionalCost5)
        {
            InvHdr.CustomField1 = customField1;
            InvHdr.CustomField2 = customField2;
            InvHdr.CustomField3 = customField3;
            InvHdr.CustomField4 = customField4;
            InvHdr.CustomField5 = customField5;
            InvHdr.AdditionalCost1 = additionalCost1;
            InvHdr.AdditionalCost2 = additionalCost2;
            InvHdr.AdditionalCost3 = additionalCost3;
            InvHdr.AdditionalCost4 = additionalCost4;
            InvHdr.AdditionalCost5 = additionalCost5;
            return true;
        }

        public bool SetVendorInvoiceNo(string VendorInvoiceNo)
        {
            InvHdr.VendorInvoiceNo = VendorInvoiceNo;
            return true;
        }

        public bool SetPurchaseOrder(string PurchaseOrderNo)
        {
            InvHdr.PurchaseOrderNo = PurchaseOrderNo;

            return true;
        }

        public bool SetInventoryDate(DateTime InventoryDate)
        {
            InvHdr.InventoryDate = InventoryDate;

            return true;
        }

        public bool SetInventoryStockDocumentNo(string StockDocumentNo)
        {
            InvHdr.StockDocumentNo = StockDocumentNo;

            return true;
        }

        public bool SetInventoryMovementType(string MovementType)
        {
            InvHdr.MovementType = MovementType;

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

        public bool CreateStockTakeEntries(string username, string takenBy, string verifiedBy, out string status)
        {
            return CreateStockTakeEntries(username, takenBy, verifiedBy, false, out status);
        }

        public bool CreateStockTakeEntries(string username, string takenBy, string verifiedBy, bool autoMarked, out string status)
        {
            try
            {
                InventoryDetCollection mergedTmpDet = new InventoryDetCollection();
                MergeInventoryDet(ref mergedTmpDet);
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
                        decimal BalQtyAtEntry = InventoryController.GetStockBalanceQtyByItemByDate(InvDet[i].ItemNo, InvHdr.InventoryLocationID.Value, InvHdr.InventoryDate, out status);
                        decimal AdjustmentQty = InvDet[i].Quantity.GetValueOrDefault(0) - BalQtyAtEntry;
                        SQL =
                            "UPDATE StockTake " +
                            "SET StockTakeQty = " + InvDet[i].Quantity + " " +
                                ", BalQtyAtEntry = " + BalQtyAtEntry + " " +
                                ", AdjustmentQty = " + AdjustmentQty + " " +
                                ", takenby = '" + takenBy + "' " +
                                ", verifiedBy = '" + verifiedBy + "' " +
                                ", stocktakedate = '" + InvHdr.InventoryDate.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                ", ModifiedOn = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "', ModifiedBy = '" + username + "' " +
                                ", userflag1 = 0 " +
                                ", Userfld10 = '" + InvDet[i].Userfld10 + "'"+
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
                        //st.CostOfGoods = InventoryController.FetchAverageCostOfGoodsLeftByItemNo(st.BalQtyAtEntry.GetValueOrDefault(0), InvDet[i].ItemNo, InvHdr.InventoryLocationID.Value);
                        st.CostOfGoods = ItemSummaryController.GetAvgCostPrice_StockTake(st.ItemNo, InvHdr.InventoryLocationID.Value, InvHdr.InventoryDate);
                        st.TakenBy = takenBy;
                        st.VerifiedBy = verifiedBy;
                        st.Remark = InvHdr.Remark;
                        st.Marked = false || autoMarked;
                        st.InventoryLocationID = InvHdr.InventoryLocationID.Value;
                        st.SerialNo = InvDet[i].SerialNo;
                        st.Deleted = false;
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
        }

        public bool CreateStockTakeEntriesWithBatchNo(string username, string takenBy, string verifiedBy, out string status)
        {
            bool DisplayBatchNo = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Mobile.DisplayBatchNo), false);
            if (DisplayBatchNo)
            {
                InventoryDetCollection mergedTmpDet = new InventoryDetCollection();
                //MergeInventoryDet(ref mergedTmpDet);
                //InvDet = mergedTmpDet;
                QueryCommandCollection cmd = new QueryCommandCollection();
                //loop through inventory det
                for (int i = 0; i < InvDet.Count; i++)
                {
                    //check if stocktake entry exist
                    string SQL = "select top 1 stocktakeid from stocktake " +
                                "where itemno = '" + InvDet[i].ItemNo + "' and inventorylocationid =" + InvHdr.InventoryLocationID.Value + " and userfld1 = '" + InvDet[i].Userfld1 + "' and isadjusted=0 and isnull(userflag1,0) = 0 " +
                                "order by stocktakedate desc";
                    QueryCommand cmdTmp = new QueryCommand(SQL, "PowerPOS");
                    object tmpObj = DataService.ExecuteScalar(cmdTmp);
                    if (tmpObj != null && tmpObj is int)
                    {
                        string SQL2 = "select top 1 StockTakeQty from stocktake " +
                                "where itemno = '" + InvDet[i].ItemNo + "' and inventorylocationid =" + InvHdr.InventoryLocationID.Value + " and userfld1 = '" + InvDet[i].Userfld1 + "' and isadjusted=0 and isnull(userflag1,0) = 0 " +
                                    "and ISNULL(userfld2, '') <> '" + InvHdr.UniqueID.ToString("N") + "'" +
                                "order by stocktakedate desc";
                        QueryCommand cmdTmp2 = new QueryCommand(SQL2, "PowerPOS");
                        object tmpObj2 = DataService.ExecuteScalar(cmdTmp2);

                        if (tmpObj2 != null)
                        {
                            decimal lastStockTakeQty = (decimal)tmpObj2;
                            decimal balQtyAtEntry = InventoryController.GetStockBalanceQtyByItemByDate(InvDet[i].ItemNo, InvHdr.InventoryLocationID.Value, InvHdr.InventoryDate, out status);
                            decimal newStockTakeQty = lastStockTakeQty + (InvDet[i].Quantity.HasValue ? InvDet[i].Quantity.Value : 0);

                            SQL = @"UPDATE StockTake 
                                    SET 
                                        StockTakeQty = @StockTakeQty
                                        , AdjustmentQty = @AdjustmentQty
                                        , BalQtyAtEntry = @BalQtyAtEntry
                                        , ModifiedOn = GETDATE()
                                        , ModifiedBy = @ModifiedBy
                                        , userfld2 = @userfld2 
                                    WHERE stocktakeid = @stocktakeid";

                            cmdTmp = new QueryCommand(SQL, "PowerPOS");
                            cmdTmp.AddParameter("@StockTakeQty", newStockTakeQty);
                            cmdTmp.AddParameter("@BalQtyAtEntry", balQtyAtEntry);
                            cmdTmp.AddParameter("@AdjustmentQty", newStockTakeQty - balQtyAtEntry);
                            cmdTmp.AddParameter("@ModifiedBy", username);
                            cmdTmp.AddParameter("@userfld2", InvHdr.UniqueID.ToString("N"));
                            cmdTmp.AddParameter("@stocktakeid", tmpObj.ToString());
                            cmd.Add(cmdTmp);
                        }
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
                        //st.CostOfGoods = InventoryController.FetchAverageCostOfGoodsLeftByItemNo(st.BalQtyAtEntry.GetValueOrDefault(0), InvDet[i].ItemNo, InvHdr.InventoryLocationID.Value);
                        st.CostOfGoods = ItemSummaryController.GetAvgCostPrice(InvDet[i].ItemNo, InvHdr.InventoryLocationID.Value);
                        st.TakenBy = takenBy;
                        st.VerifiedBy = verifiedBy;
                        st.Marked = false;
                        st.InventoryLocationID = InvHdr.InventoryLocationID.Value;
                        st.BatchNo = InvDet[i].Userfld1;
                        st.UniqueID = InvHdr.UniqueID.ToString("N");
                        //cmd.Add(st.GetInsertCommand(username));

                        //0 : UniqueID, 1: BatchNo, 2: ItemNo, 3: StockTakeDate, 4: StockTakeQty, 5: BalQtyAtEntry, 6: AdjustmentQty, 7: CostOfGoods, 
                        //8: TakenBy, 9: VerifiedBy, 10: InventoryLocationID, 11: username 
                        string sql = @"IF((SELECT COUNT(*) FROM Stocktake WHERE ISNULL(userfld2, '') = '{0}' and ISNULL(userfld1, '') = '{1}' 
                                            and isadjusted=0 and isnull(userflag1,0) = 0 and itemno = '{2}')<=0)
                                BEGIN 
	                                INSERT INTO StockTake (IsAdjusted, ItemNo, StockTakeDate, StockTakeQty , BalQtyAtEntry, AdjustmentQty, CostOfGoods, TakenBy, VerifiedBy, 
                                    Marked, InventoryLocationID, Userfld1, Userfld2, createdBy, createdOn, modifiedby, modifiedon, AuthorizedBy) 
	                                VALUES (0, '{2}','{3}',{4},{5}, {6}, {7}, '{8}', '{9}', 0, {10}, '{1}', '{0}', '{11}',GETDATE(),'{11}',GETDATE(), '') 
                                END
                                ";
                        sql = string.Format(sql, st.UniqueID, st.BatchNo, st.ItemNo, st.StockTakeDate.ToString("yyyy-MM-dd HH:mm:ss"), st.StockTakeQty.ToString(),
                                st.BalQtyAtEntry.ToString(), st.AdjustmentQty.ToString(), st.CostOfGoods.ToString(), st.TakenBy, st.VerifiedBy, st.InventoryLocationID.ToString(),
                                username);
                        //Logger.writeLog("Query to Execute : " + sql);

                        //string sqlString = "Insert into StockTake(ItemNo, IsAdjusted, StockTakeDate,)"
                        //cmd.Add(st.GetInsertCommand(username));
                        cmd.Add(new QueryCommand(sql));
                    }
                }
                DataService.ExecuteTransaction(cmd);
                status = "";
                return true;
            }
            else
            {
                try
                {
                    InventoryDetCollection mergedTmpDet = new InventoryDetCollection();
                    MergeInventoryDet(ref mergedTmpDet);
                    InvDet = mergedTmpDet;
                    QueryCommandCollection cmd = new QueryCommandCollection();
                    //loop through inventory det
                    for (int i = 0; i < InvDet.Count; i++)
                    {

                        //check if stocktake entry exist
                        string SQL = "select top 1 stocktakeid from stocktake " +
                                    "where itemno = '" + InvDet[i].ItemNo + "' and inventorylocationid =" + InvHdr.InventoryLocationID.Value + " and ISNULL(userfld1, '') = '' and isadjusted=0 and isnull(userflag1,0) = 0 " +
                                    "order by stocktakedate desc";
                        QueryCommand cmdTmp = new QueryCommand(SQL, "PowerPOS");
                        object tmpObj = DataService.ExecuteScalar(cmdTmp);
                        if (tmpObj != null && tmpObj is int)
                        {
                            string SQL2 = "select top 1 StockTakeQty from stocktake " +
                                "where itemno = '" + InvDet[i].ItemNo + "' and inventorylocationid =" + InvHdr.InventoryLocationID.Value + " and ISNULL(userfld1, '') = '' and isadjusted=0 and isnull(userflag1,0) = 0 " +
                                    "and ISNULL(userfld2, '') <> '" + InvHdr.UniqueID.ToString("N") + "' " +
                                "order by stocktakedate desc";
                            QueryCommand cmdTmp2 = new QueryCommand(SQL2, "PowerPOS");
                            object tmpObj2 = DataService.ExecuteScalar(cmdTmp2);

                            if (tmpObj2 != null)
                            {
                                SQL =
                                        "UPDATE StockTake " +
                                        "SET StockTakeQty = " + (InvDet[i].Quantity.Value + (decimal)tmpObj2) + " " +
                                            ", ModifiedOn = GETDATE(), ModifiedBy = '" + username + "' " +
                                            ", userfld2 = '" + InvHdr.UniqueID.ToString("N") + "' " +
                                        "WHERE stocktakeid=" + tmpObj.ToString() + "";
                                cmdTmp = new QueryCommand(SQL, "PowerPOS");
                                cmd.Add(cmdTmp);
                            }
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
                            st.CostOfGoods = InventoryController.FetchAverageCostOfGoodsLeftByItemNo(st.BalQtyAtEntry.GetValueOrDefault(0), InvDet[i].ItemNo, InvHdr.InventoryLocationID.Value);
                            st.TakenBy = takenBy;
                            st.VerifiedBy = verifiedBy;
                            st.Marked = false;
                            st.InventoryLocationID = InvHdr.InventoryLocationID.Value;
                            st.BatchNo = InvDet[i].Userfld1;
                            st.UniqueID = InvHdr.UniqueID.ToString("N");

                            //0 : UniqueID, 1: BatchNo, 2: ItemNo, 3: StockTakeDate, 4: StockTakeQty, 5: BalQtyAtEntry, 6: AdjustmentQty, 7: CostOfGoods, 
                            //8: TakenBy, 9: VerifiedBy, 10: InventoryLocationID, 11: username 
                            string sql = @"IF((SELECT COUNT(*) FROM Stocktake WHERE ISNULL(userfld2, '') = '{0}' and ISNULL(userfld1, '') = '{1}' 
                                            and isadjusted=0 and isnull(userflag1,0) = 0 and itemno = '{2}')<=0)
                                BEGIN 
	                                INSERT INTO StockTake (IsAdjusted, ItemNo, StockTakeDate, StockTakeQty , BalQtyAtEntry, AdjustmentQty, CostOfGoods, TakenBy, VerifiedBy, 
                                    Marked, InventoryLocationID, Userfld1, Userfld2, createdBy, createdOn, modifiedby, modifiedon, AuthorizedBy ) 
	                                VALUES (0, '{2}','{3}',{4},{5}, {6}, {7}, '{8}', '{9}', 0, {10}, '{1}', '{0}', '{11}',GETDATE(),'{11}',GETDATE(), '') 
                                END
                                ";
                            sql = string.Format(sql, st.UniqueID,st.BatchNo, st.ItemNo, st.StockTakeDate.ToString("yyyy-MM-dd HH:mm:ss"), st.StockTakeQty.ToString(), 
                                    st.BalQtyAtEntry.ToString(), st.AdjustmentQty.ToString(), st.CostOfGoods.ToString(), st.TakenBy, st.VerifiedBy, st.InventoryLocationID.ToString(), 
                                    username);

                            //string sqlString = "Insert into StockTake(ItemNo, IsAdjusted, StockTakeDate,)"
                            //cmd.Add(st.GetInsertCommand(username));
                            cmd.Add(new QueryCommand(sql));
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
            }
        }

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
        public bool ImportFromDataCollectorTextFile(string filepath, out DataTable message)
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
            var beforeModification = new InventoryDetCollection();
            InvDet.CopyTo(beforeModification);
            DataTable errorDb = CreateCSVImportErrorMessageDataTable();
            try
            {
                string itemNoColumnName;
                string qtyColumnName;
                string barcodeColumnName;

                #region *) Get Columns' Name

                if (message.Columns.Contains("Item No")) itemNoColumnName = "Item No";
                else if (message.Columns.Contains("ItemNo")) itemNoColumnName = "ItemNo";
                else itemNoColumnName = "";

                if (message.Columns.Contains("Qty")) qtyColumnName = "Qty";
                else if (message.Columns.Contains("Quantity")) qtyColumnName = "Quantity"; 
				else qtyColumnName = "";

                if (message.Columns.Contains("Barcode")) barcodeColumnName = "Barcode";
                else
                {
                    if (itemNoColumnName == "")
                        throw new Exception("Cannot find ItemNo/Barcode field");
                    barcodeColumnName = "";
                }

				#endregion

                var newMessage = new DataTable();
                newMessage.Columns.Add("ItemNo", Type.GetType("System.String"));
                newMessage.Columns.Add("Qty", Type.GetType("System.Decimal"));

				var ItemNoCollection = new HashSet<string>();
	            
				var existingItems = new ItemCollection();
	            existingItems.Where(Item.Columns.Deleted, false);
	            existingItems.Load();

				var existingItemsByItemNo = new Dictionary<string, Item>();
				var existingItemsByBarcode = new Dictionary<string, Item>();

	            foreach (var existingItem in existingItems)
	            {
		            if (existingItemsByItemNo.ContainsKey(existingItem.ItemNo))
		            {
			            AddNewImportErrorMessage(0, existingItem.Barcode, existingItem.ItemNo, "", "",
			                                     "Duplicate ItemNo found. Please check this item on the POS Back-End web",
			                                     ref errorDb);
			            continue;
		            }
		            existingItemsByItemNo.Add(existingItem.ItemNo, existingItem);

		            if (!string.IsNullOrEmpty(existingItem.Barcode))
		            {
			            if (existingItemsByBarcode.ContainsKey(existingItem.Barcode))
			            {
				            AddNewImportErrorMessage(0, existingItem.Barcode, existingItem.ItemNo, "", "",
				                                     "Duplicate Barcode found. Please check this item on the POS Back-End web",
				                                     ref errorDb);
				            continue;
			            }
			            existingItemsByBarcode.Add(existingItem.Barcode, existingItem);
		            }
	            }

	            for (int i = 0; i < message.Rows.Count; i++)
                {
                    var row = message.Rows[i];

                    string itemNo = "";
                    string barcode = "";

	                #region *) Get Raw information 

                    if (itemNoColumnName != "") itemNo = row[itemNoColumnName].ToString();
                    if (barcodeColumnName != "") barcode = row[barcodeColumnName].ToString();
                    string sQty = string.IsNullOrEmpty(qtyColumnName) ? "0" : row[qtyColumnName].ToString();

					#endregion

                    decimal qty;

                    //If no Quantity, ignore
					if (string.IsNullOrEmpty(sQty) || (string.IsNullOrEmpty(itemNo) && string.IsNullOrEmpty(barcode))) continue;

                    #region *) Get ItemNo from Barcode, and Vice Versa
                    
					if (itemNo == "" && barcode == "")
                    {
                        AddNewImportErrorMessage(i, barcode, itemNo, sQty, "", string.Format("Unable to read this Item No / Barcode for row #{0}", i), ref errorDb);
                        continue;
                    }

	                if (itemNo == "")
	                {
						if (!existingItemsByBarcode.ContainsKey(barcode))
						{
							AddNewImportErrorMessage(i, barcode, itemNo, sQty, "", "Barcode not found. Please check this item on the POS Back-End web", ref errorDb);
							continue;
						}

						itemNo = existingItemsByBarcode[barcode].ItemNo;
	                }
	                else if (barcode == "")
	                {
						if (!existingItemsByItemNo.ContainsKey(itemNo))
						{
							AddNewImportErrorMessage(i, barcode, itemNo, sQty, "", "ItemNo not found. Please check this item on the POS Back-End web", ref errorDb);
							continue;
						}

						barcode = existingItemsByItemNo[itemNo].Barcode;
	                }

	                #endregion

                    if (!decimal.TryParse(sQty, out qty))
                        AddNewImportErrorMessage(i, barcode, itemNo, sQty, "", "Cannot read Quantity value. Please check the file that you are trying to import.", ref errorDb);

                    string status;
					AddItemIntoInventory(existingItemsByItemNo[itemNo], qty, out status);

                    if (status != "")
                    {
                        AddNewImportErrorMessage(i, barcode, itemNo, sQty, "", string.Format("Unable to add item. {0}", status), ref errorDb);
                        Logger.writeLog(string.Format("{0} {1} {2} {3} Unable to add item: {4}", i, barcodeColumnName, itemNoColumnName, sQty, status));
                    }

                    if (ItemNoCollection.Contains(itemNo))
                        AddNewImportErrorMessage(i, barcode, itemNo, sQty, "", "Duplicated Item No", ref errorDb);

                    ItemNoCollection.Add(itemNo);
                }


                //// Important Part
                //string status;
                //itemno = splitted[0].ToString();
                ////add to temporary inventory detail....
                //AddItemIntoInventory(itemno, Qty, out status);
                //if (status != "")
                //{
                //    AddNewImportErrorMessage(rowNumber, splitted[0], itemno, splitted[1], "", "Unable to add item. " + status, ref dt);
                //    Logger.writeLog(rowNumber + " " + splitted[0] + " " + itemno + " " + splitted[1] + " " + "" + " Unable to add item: " + status);

                //}

                //// If error occurred (Revert)
                ErrorMessage = errorDb;
                if (errorDb != null && errorDb.Rows.Count > 0)
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
                ErrorMessage = errorDb;
                return false;
            }
        }

        public bool ImportFromDataTableForStockIn(DataTable message, out DataTable ErrorMessage)
        {
            var beforeModification = new InventoryDetCollection();
            InvDet.CopyTo(beforeModification);
            DataTable errorDb = CreateCSVImportErrorMessageDataTable();
            try
            {
                string itemNoColumnName;
                string qtyColumnName;
                string barcodeColumnName;
                string costPriceColumnName;

                #region *) Get Columns' Name

                if (message.Columns.Contains("Item No")) itemNoColumnName = "Item No";
                else if (message.Columns.Contains("ItemNo")) itemNoColumnName = "ItemNo";
                else itemNoColumnName = "";

                if (message.Columns.Contains("Qty")) qtyColumnName = "Qty";
                else if (message.Columns.Contains("Quantity")) qtyColumnName = "Quantity";
                else qtyColumnName = "";

                if (message.Columns.Contains("Barcode")) barcodeColumnName = "Barcode";
                else
                {
                    if (itemNoColumnName == "")
                        throw new Exception("Cannot find ItemNo/Barcode field");
                    barcodeColumnName = "";
                }

                if (message.Columns.Contains("CostPrice")) costPriceColumnName = "CostPrice";
                else if (message.Columns.Contains("Cost Price")) costPriceColumnName = "Cost Price";
                else costPriceColumnName = "";

                #endregion

                var newMessage = new DataTable();
                newMessage.Columns.Add("ItemNo", Type.GetType("System.String"));
                newMessage.Columns.Add("Qty", typeof(decimal));
                newMessage.Columns.Add("CustomPrice", typeof(decimal));

                var ItemNoCollection = new HashSet<string>();

                var existingItems = new ItemCollection();
                existingItems.Where(Item.Columns.Deleted, false);
                existingItems.Load();

                var existingItemsByItemNo = new Dictionary<string, Item>();
                var existingItemsByBarcode = new Dictionary<string, Item>();

                foreach (var existingItem in existingItems)
                {
                    if (existingItemsByItemNo.ContainsKey(existingItem.ItemNo))
                    {
                        AddNewImportErrorMessage(0, existingItem.Barcode, existingItem.ItemNo, "", "",
                                                 "Duplicate ItemNo found. Please check this item on the POS Back-End web",
                                                 ref errorDb);
                        continue;
                    }
                    existingItemsByItemNo.Add(existingItem.ItemNo, existingItem);

                    if (!string.IsNullOrEmpty(existingItem.Barcode))
                    {
                        if (existingItemsByBarcode.ContainsKey(existingItem.Barcode))
                        {
                            AddNewImportErrorMessage(0, existingItem.Barcode, existingItem.ItemNo, "", "",
                                                     "Duplicate Barcode found. Please check this item on the POS Back-End web",
                                                     ref errorDb);
                            continue;
                        }
                        existingItemsByBarcode.Add(existingItem.Barcode, existingItem);
                    }
                }

                for (int i = 0; i < message.Rows.Count; i++)
                {
                    var row = message.Rows[i];

                    string itemNo = "";
                    string barcode = "";

                    #region *) Get Raw information

                    if (itemNoColumnName != "") itemNo = row[itemNoColumnName].ToString();
                    if (barcodeColumnName != "") barcode = row[barcodeColumnName].ToString();
                    string sQty = string.IsNullOrEmpty(qtyColumnName) ? "0" : row[qtyColumnName].ToString();
                    string sCostPrice = string.IsNullOrEmpty(costPriceColumnName) ? "0" : row[costPriceColumnName].ToString();

                    #endregion

                    decimal qty;
                    decimal costPrice;

                    //If no Quantity, ignore
                    if (string.IsNullOrEmpty(sQty) || (string.IsNullOrEmpty(itemNo) && string.IsNullOrEmpty(barcode))) continue;

                    #region *) Get ItemNo from Barcode, and Vice Versa

                    if (itemNo == "" && barcode == "")
                    {
                        AddNewImportErrorMessage(i, barcode, itemNo, sQty, "", string.Format("Unable to read this Item No / Barcode for row #{0}", i), ref errorDb);
                        continue;
                    }

                    if (itemNo == "")
                    {
                        if (!existingItemsByBarcode.ContainsKey(barcode))
                        {
                            AddNewImportErrorMessage(i, barcode, itemNo, sQty, "", "Barcode not found. Please check this item on the POS Back-End web", ref errorDb);
                            continue;
                        }

                        itemNo = existingItemsByBarcode[barcode].ItemNo;
                    }
                    else if (barcode == "")
                    {
                        if (!existingItemsByItemNo.ContainsKey(itemNo))
                        {
                            AddNewImportErrorMessage(i, barcode, itemNo, sQty, "", "ItemNo not found. Please check this item on the POS Back-End web", ref errorDb);
                            continue;
                        }

                        barcode = existingItemsByItemNo[itemNo].Barcode;
                    }

                    #endregion

                    if (!decimal.TryParse(sQty, out qty))
                        AddNewImportErrorMessage(i, barcode, itemNo, sQty, "", "Cannot read Quantity value. Please check the file that you are trying to import.", ref errorDb);

                    if (!decimal.TryParse(sCostPrice, out costPrice))
                        AddNewImportErrorMessage(i, barcode, itemNo, sCostPrice, "", "Cannot read Cost Price value. Please check the file that you are trying to import.", ref errorDb);


                    string status;
                    AddItemIntoInventoryStockInFromFile(itemNo, qty, costPrice, out status);

                    if (status != "")
                    {
                        AddNewImportErrorMessage(i, barcode, itemNo, sQty, "", string.Format("Unable to add item. {0}", status), ref errorDb);
                        Logger.writeLog(string.Format("{0} {1} {2} {3} Unable to add item: {4}", i, barcodeColumnName, itemNoColumnName, sQty, status));
                    }

                    if (ItemNoCollection.Contains(itemNo))
                        AddNewImportErrorMessage(i, barcode, itemNo, sQty, "", "Duplicated Item No", ref errorDb);

                    ItemNoCollection.Add(itemNo);
                }


                //// Important Part
                //string status;
                //itemno = splitted[0].ToString();
                ////add to temporary inventory detail....
                //AddItemIntoInventory(itemno, Qty, out status);
                //if (status != "")
                //{
                //    AddNewImportErrorMessage(rowNumber, splitted[0], itemno, splitted[1], "", "Unable to add item. " + status, ref dt);
                //    Logger.writeLog(rowNumber + " " + splitted[0] + " " + itemno + " " + splitted[1] + " " + "" + " Unable to add item: " + status);

                //}

                //// If error occurred (Revert)
                ErrorMessage = errorDb;
                if (errorDb != null && errorDb.Rows.Count > 0)
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
                ErrorMessage = errorDb;
                return false;
            }
        }

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

        public bool IsNew()
        {
            //saved or unsaved
            return InvHdr.IsNew;

        }

        public DataTable InvHdrToDataTable()
        {
            try
            {
                InventoryHdrCollection tmp = new InventoryHdrCollection();
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

        public static DataTable FetchItemTrace
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
            dt.Columns.Add("OnHand", Type.GetType("System.Decimal"));
            for (int i = 0; i < inv.Count; i++)
            {
                dt.Columns.Add(inv[i].InventoryLocationName, Type.GetType("System.Decimal"));
            }
            //populate the rows...
            DateTime myDate;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //DateTime.TryParse(dt.Rows[i]["InventoryDate"].ToString(), out myDate);
                //dt.Rows[i]["OnHand"] = InventoryController.GetStockBalanceQtyByItemByDate(ItemNo, 0, myDate.AddSeconds(1), out status);
                if (dt.Rows[i]["InventoryDate"] is DateTime)
                    myDate = (DateTime)dt.Rows[i]["InventoryDate"];
                else
                    myDate = DateTime.MinValue;
                dt.Rows[i]["OnHand"] = InventoryController.GetStockBalanceQtyByItemByDate(ItemNo, 0, myDate, out status);

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
                    //dt.Rows[i][inv[j].InventoryLocationName] =
                    //    InventoryController.GetStockBalanceQtyByItemByDate
                    //    (ItemNo, inv[j].InventoryLocationID, myDate.AddSeconds(1), out status).ToString().Replace("'", "").Replace("`", "");
                    dt.Rows[i][inv[j].InventoryLocationName] =
                        InventoryController.GetStockBalanceQtyByItemByDate
                        (ItemNo, inv[j].InventoryLocationID, myDate, out status).ToString().Replace("'", "").Replace("`", "");
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
            dt.Columns["MovementType"].ColumnName = "Event Stock";
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

                // If pre order item, don't include in the list
                if (myReceipt.IsPreOrder.GetValueOrDefault(false) == true) continue;

                drNewRow["Date"] = myReceipt.OrderDetDate;
                drNewRow["Qty"] = myReceipt.Quantity;
                if (v[p].InventoryHdrRefNo == "ADJUSTED")
                {
                    drNewRow["Event Stock"] = "Sales (Adjusted)";
                }
                else if (v[p].InventoryHdrRefNo == "")
                {
                    drNewRow["Event Stock"] = "Sales (Undeducted)";
                }
                else 
                {
                    drNewRow["Event Stock"] = "Sales (Unknown ID:" + v[p].InventoryHdrRefNo + ")";
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
        }

        public void SetInventoryHdrUserName(string pUserName)
        {
            InvHdr.UserName = pUserName;
        }

        public bool IsRecordExisted(string movementType)
        {
            if (!String.IsNullOrEmpty(InvHdr.InvoiceNo))
                return false;


            for (int i = 0; i < InvDet.Count; i++)
            {
                string sql = @"
                                SELECT CAST(CASE WHEN EXISTS(
                                    SELECT * 
                                    FROM InventoryDet det
                                        INNER JOIN InventoryHdr hdr ON hdr.InventoryHdrRefNo = det.InventoryHdrRefNo
                                    WHERE det.UniqueID = '{0}' AND hdr.MovementType = '{1}'
                                ) THEN 1 ELSE 0 END AS BIT)
                             ";
                sql = string.Format(sql, InvDet[i].UniqueID.ToString(), movementType);
                object exist = DataService.ExecuteScalar(new QueryCommand(sql, "PowerPOS"));
                if (exist is bool && (bool)exist == true)
                    return true;
            }
            return false;
        }

        public void CalculateAdditionalCost()
        {

            decimal SumOfFactoryPriceAndQty = 0;
            #region *) Calculate: calculate sum of factory price * quantity for distributing Freight charges
            for (int i = 0; i < InvDet.Count; i++)
                SumOfFactoryPriceAndQty += (decimal)InvDet[i].InitialFactoryPrice.GetValueOrDefault(0) * InvDet[i].Quantity.GetValueOrDefault(0);

            if (SumOfFactoryPriceAndQty == 0)
                SumOfFactoryPriceAndQty = 0.0001M;
            #endregion

            decimal totalCost = 0;
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.UseBasicCostPrice), false))
            {
                totalCost = 0;
            }
            else
            {
                #region *) Calculate total additional cost
                totalCost += InvHdr.FreightCharge.GetValueOrDefault(0);
                totalCost -= InvHdr.Discount.GetValueOrDefault(0);

                #region oldcode now GST calculation on function SET GST
                //if (InvHdr.GSTRule == 1) // Exclusive
                //    totalCost += InvHdr.Tax.GetValueOrDefault(0);
                //if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.IncludeGSTExclusive), false))
                //{
                //    if (InvHdr.GSTRule == 1) // Exclusive
                //        totalCost += InvHdr.Tax.GetValueOrDefault(0);
                //}
                //else
                //{
                //    if (InvHdr.GSTRule == 2) // Inclusive
                //        totalCost -= InvHdr.Tax.GetValueOrDefault(0); // gst must be excluded from cost price calculation
                //}     
                #endregion

                if (InvHdr.AdditionalCost1 != 0)
                {
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost1_IsPercentage), false))
                        totalCost += InvHdr.AdditionalCost1 / 100 * SumOfFactoryPriceAndQty;
                    else
                        totalCost += InvHdr.AdditionalCost1;
                }
                if (InvHdr.AdditionalCost2 != 0)
                {
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost2_IsPercentage), false))
                        totalCost += InvHdr.AdditionalCost2 / 100 * SumOfFactoryPriceAndQty;
                    else
                        totalCost += InvHdr.AdditionalCost2;
                }
                if (InvHdr.AdditionalCost3 != 0)
                {
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost3_IsPercentage), false))
                        totalCost += InvHdr.AdditionalCost3 / 100 * SumOfFactoryPriceAndQty;
                    else
                        totalCost += InvHdr.AdditionalCost3;
                }
                if (InvHdr.AdditionalCost4 != 0)
                {
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost4_IsPercentage), false))
                        totalCost += InvHdr.AdditionalCost4 / 100 * SumOfFactoryPriceAndQty;
                    else
                        totalCost += InvHdr.AdditionalCost4;
                }
                if (InvHdr.AdditionalCost5 != 0)
                {
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost5_IsPercentage), false))
                        totalCost += InvHdr.AdditionalCost5 / 100 * SumOfFactoryPriceAndQty;
                    else
                        totalCost += InvHdr.AdditionalCost5;
                }
                #endregion
            }

            for (int i = 0; i < InvDet.Count; i++)
            {
                decimal weight = (InvDet[i].InitialFactoryPrice.GetValueOrDefault(0) * InvDet[i].Quantity.GetValueOrDefault(0)) / SumOfFactoryPriceAndQty;
                if (InvDet[i].Quantity > 0)
                {
                    InvDet[i].FactoryPrice = InvDet[i].InitialFactoryPrice.GetValueOrDefault(0);

                    if (InvHdr.ExchangeRate > 0)
                        InvDet[i].FactoryPrice = InvDet[i].FactoryPrice * (decimal)InvHdr.ExchangeRate;

                    // Line discount
                    decimal discAmt = (InvDet[i].Discount.GetValueOrDefault(0) / 100) * InvDet[i].FactoryPrice;
                    InvDet[i].FactoryPrice -= discAmt;

                    //=== Commented because Discount is already included in totalCost and calculated below ===
                    //InvDet[i].FactoryPrice =
                    //    InvDet[i].FactoryPrice - ((weight * InvHdr.Discount.GetValueOrDefault(0)) / InvDet[i].Quantity.GetValueOrDefault(0));

                    // Include additional cost into the FactoryPrice so it will be included in Item Summary calculation
                    InvDet[i].FactoryPrice = InvDet[i].FactoryPrice
                        + (decimal)((weight * totalCost) / InvDet[i].Quantity);

                    InvDet[i].CostOfGoods = InvDet[i].FactoryPrice;

                    //InvDet[i].TotalCost = InvDet[i].TotalInitialCost + (weight * totalCost);
                    InvDet[i].TotalCost = InvDet[i].FactoryPrice * InvDet[i].Quantity.GetValueOrDefault(0);
                }
            }
            
        }

        //Set Factory Price
        public bool ChangeFactoryPriceByItemNo(string ItemNo, decimal newFactoryPrice, out string status)
        {
            status = "";
            try
            {
                for (int i = 0; i < InvDet.Count; i++)
                {
                    if (InvDet[i].ItemNo == ItemNo)
                    {
                        InvDet[i].FactoryPrice = newFactoryPrice;
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

        #region *) Purchase Order
        public static decimal getQuantityStockedInForPurchaseOrder(string PurchaseOrderdetRefNo)
        {
            try 
            {
            string sqlString = "Select sum(Quantity) From InventoryDet where " +
                InventoryDet.UserColumns.PurchaseOrderDetRefNo + " = '" +
                PurchaseOrderdetRefNo + "'";
            object tmpResult = DataService.ExecuteScalar(new QueryCommand(sqlString));
            if (tmpResult != null)
            {
                decimal result = 0;
                if (decimal.TryParse(tmpResult.ToString(), out result))
                    return result;
                else
                    return 0;
            }
            else
            {
                return 0;
            }
            } catch (Exception ex)
            {
                Logger.writeLog(ex.Message);
                return 0;
            }
                
        }
        #endregion

        
    }
}
