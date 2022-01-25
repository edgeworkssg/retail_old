using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic;
using System.Data;

namespace PowerPOS
{
    public class ItemTagController
    {
        public static QueryCommandCollection FetchItemTagUpdate(InventoryHdr invHdr, InventoryDetCollection invDetColl)
        {
            if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.EnableProductSerialNo), false))
                return new QueryCommandCollection();

            string movementType =  invHdr.MovementType;
            int inventoryLocationID = invHdr.InventoryLocationID.GetValueOrDefault(0);

            QueryCommandCollection qmc = new QueryCommandCollection();

            foreach (var invDet in invDetColl)
            {
                var serialNoColl = invDet.SerialNo;
                if(serialNoColl == null || serialNoColl.Count == 0) 
                    continue;

                var item = new Item(invDet.ItemNo);
                if (item.IsNew) 
                    continue;

                if (!item.IsUseSerialNo)
                {
                    item.IsUseSerialNo = true;
                    qmc.Add(item.GetUpdateCommand("SYSTEM"));
                }

                foreach (var serialNo in serialNoColl)
                {
                    if (string.IsNullOrEmpty(serialNo)) continue;

                    string itemTagID = invDet.ItemNo + "-" + serialNo;
                    var itemTag = new ItemTagSummary(itemTagID);
                    if (itemTag.IsNew)
                    {
                        itemTag = new ItemTagSummary();
                        itemTag.ItemTagSummaryID = itemTagID;
                        itemTag.ItemNo = invDet.ItemNo;
                        itemTag.SerialNo = serialNo;
                        itemTag.UniqueID = Guid.NewGuid();
                    }

                    itemTag.InventoryLocationID = inventoryLocationID;
                    itemTag.IsAvailable = movementType.ToUpper().Contains("IN");
                    itemTag.Deleted = false;

                    if (!itemTag.DirtyColumns.Contains(ItemTagSummary.InventoryLocationIDColumn.ColumnName))
                        itemTag.DirtyColumns.Add(ItemTagSummary.InventoryLocationIDColumn);
                    
                    if (!itemTag.DirtyColumns.Contains(ItemTagSummary.IsAvailableColumn.ColumnName))
                        itemTag.DirtyColumns.Add(ItemTagSummary.IsAvailableColumn);

                    if (!itemTag.DirtyColumns.Contains(ItemTagSummary.DeletedColumn.ColumnName))
                        itemTag.DirtyColumns.Add(ItemTagSummary.DeletedColumn);

                    if (itemTag.IsNew)
                        qmc.Add(itemTag.GetInsertCommand("SYSTEM"));
                    else
                        qmc.Add(itemTag.GetUpdateCommand("SYSTEM"));

                    qmc.Add(InitItemTagHistory(itemTag, invHdr.InventoryDate, invHdr.MovementType, invDet.InventoryDetRefNo, invHdr.StockOutReasonID.GetValueOrDefault(0)));

                }
            }

            return qmc;
        }

        public static QueryCommandCollection FetchItemTagUpdateStockTake(StockTake stockTakeData,  int reasonID)
        {
            if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.EnableProductSerialNo), false))
                return new QueryCommandCollection();

            QueryCommandCollection qmc = new QueryCommandCollection();

            try
            {
                string movementType = "Adjustment";
                int inventoryLocationID = stockTakeData.InventoryLocationID;
                var serialNoColl = stockTakeData.SerialNo;
                if(serialNoColl == null)
                    serialNoColl = new List<string>();

                var item = new Item(stockTakeData.ItemNo);
                if (item.IsNew)
                    return qmc;

                if (!item.IsUseSerialNo)
                {
                    item.IsUseSerialNo = true;
                    qmc.Add(item.GetUpdateCommand("SYSTEM"));
                }

                string sql = @"
                SELECT *
                FROM	ItemTagSummary
                WHERE	ItemNo = @ItemNo
	                    AND InventoryLocationID = @InventoryLocationID";

                QueryCommand cmd = new QueryCommand(sql);
                cmd.AddParameter("@ItemNo", item.ItemNo, DbType.String);
                cmd.AddParameter("@InventoryLocationID", inventoryLocationID, DbType.Int32);

                var itemTagColl = new ItemTagSummaryCollection();
                itemTagColl.LoadAndCloseReader(DataService.GetReader(cmd));

                foreach (var itemTag in itemTagColl)
                {
                    if (serialNoColl.Exists(o => o.IsEqual(itemTag.SerialNo)))
                        continue;

                    if (!itemTag.IsAvailable.GetValueOrDefault(false)) 
                        continue;
                    
                    if (item.Deleted.GetValueOrDefault(false)) 
                        continue;

                    itemTag.IsAvailable = false;
                    qmc.Add(itemTag.GetUpdateCommand("STOCK TAKE"));
                    qmc.Add(InitItemTagHistory(itemTag, stockTakeData.StockTakeDate, movementType + " Out", "-", reasonID));
                }

                foreach (var serialNo in serialNoColl)
                {
                    var itemTag = itemTagColl.Where(o => o.SerialNo.IsEqual(serialNo)).FirstOrDefault();
                    if (itemTag != null && 
                        !itemTag.Deleted.GetValueOrDefault(false) && 
                        itemTag.IsAvailable.GetValueOrDefault(false))
                        continue;
                    
                    string itemTagID = stockTakeData.ItemNo + "-" + serialNo;
                    itemTag = new ItemTagSummary(itemTagID);
                    if(itemTag.IsNew)
                    {
                        itemTag = new ItemTagSummary();
                        itemTag.ItemTagSummaryID = itemTagID;
                        itemTag.ItemNo = stockTakeData.ItemNo;
                        itemTag.SerialNo = serialNo;
                        itemTag.UniqueID = Guid.NewGuid();
                    }

                    bool isNewSerial = itemTag.IsNew;
                    if (!itemTag.IsNew && (!itemTag.IsAvailable.GetValueOrDefault(false) || itemTag.Deleted.GetValueOrDefault(false)))
                        isNewSerial = true;

                    itemTag.InventoryLocationID = inventoryLocationID;
                    itemTag.IsAvailable = true;
                    itemTag.Deleted = false;

                    if(itemTag.IsNew)
                        qmc.Add(itemTag.GetInsertCommand("STOCK TAKE"));
                    else
                        qmc.Add(itemTag.GetUpdateCommand("STOCK TAKE"));

                    if (isNewSerial)
                        qmc.Add(InitItemTagHistory(itemTag, stockTakeData.StockTakeDate, movementType + " In", "-", reasonID));
                }

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return qmc;
        }

        private static QueryCommand InitItemTagHistory(ItemTagSummary itemTag, DateTime invDate, string movementType, string refNo, int? reasonID)
        {
            var itemTagStatus = new ItemTagStatusDetail();
            itemTagStatus.ItemTagStatusDetailID = Guid.NewGuid();
            itemTagStatus.ItemNo = itemTag.ItemNo;
            itemTagStatus.SerialNo = itemTag.SerialNo;
            itemTagStatus.InventoryLocationID = itemTag.InventoryLocationID;
            itemTagStatus.InventoryDate = invDate;
            itemTagStatus.MovementType = movementType;
            itemTagStatus.InventoryDetRefNo = refNo;
            itemTagStatus.StockOutReasonID = reasonID;
            itemTagStatus.Deleted = false;

            return itemTagStatus.GetInsertCommand("SYSTEM");
        }

        public static bool CheckSerialNoIsExistHelper(List<ItemTagModel> input, out string message)
        {

            bool isSuccess = true;
            message = "";

            if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.EnableProductSerialNo), false))
                return true;

            try
            {
                foreach (var lineInput in input)
                {
                    if (lineInput.SerialNoColl == null)
                        continue;
                    if (lineInput.SerialNoColl.Count == 0)
                        continue;

                    InventoryLocation invLoc = new InventoryLocation(lineInput.InventoryLocationID);
                    if (invLoc.IsNew)
                        continue;

                    Item item = new Item(lineInput.ItemNo);
                    if (item.IsNew || item.Deleted.GetValueOrDefault(false))
                        continue;

                    foreach (var serialNo in lineInput.SerialNoColl)
                    {
                        string sql = @"
                        SELECT  COUNT(*) RowNo
                        FROM	ItemTagSummary
                        WHERE	ISNULL(Deleted,0) = 0
		                        AND ISNULL(IsAvailable,0) = 1
		                        AND ItemNo = @ItemNo
		                        AND SerialNo = @SerialNo
		                        AND InventoryLocationID = @InventoryLocationID";

                        QueryCommand cmd = new QueryCommand(sql);
                        cmd.AddParameter("@ItemNo", lineInput.ItemNo, System.Data.DbType.String);
                        cmd.AddParameter("@SerialNo", serialNo, System.Data.DbType.String);
                        cmd.AddParameter("@InventoryLocationID", lineInput.InventoryLocationID, System.Data.DbType.Int32);

                        var dt = new DataTable();
                        dt.Load(DataService.GetReader(cmd));

                        bool isExist = false;
                        if (dt.Rows.Count > 0)
                            isExist = (dt.Rows[0]["RowNo"] + "").GetIntValue() > 0;

                        if (!isExist)
                        {
                            isSuccess = false;
                            message += string.Format("Serial No {0} for item {1} not exist in {2}", serialNo, item.ItemName, invLoc.InventoryLocationName);
                            message += Environment.NewLine;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }

        public static bool CheckSerialNoIsExist(List<ItemTagModel> input, out string message)
        {
            bool isSuccess = true;
            message = "";

            if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.EnableProductSerialNo), false))
                return true;

            try
            {
                string XMLFILENAME = "\\WS.XML";
                string WS_URL = "http://localhost:51831/PowerPOSWeb/Synchronization/Synchronization.asmx";
                DataSet dsURL = new DataSet();
                dsURL.ReadXml(AppDomain.CurrentDomain.BaseDirectory + XMLFILENAME);
                WS_URL = dsURL.Tables[0].Rows[0]["URL"].ToString();
                if (!WS_URL.ToLower().StartsWith("http://localhost:"))
                {

                    PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                    ws.Url = WS_URL;
                    var wsInput = (from o in input
                                   select new PowerPOSLib.PowerPOSSync.ItemTagModel
                                   {
                                       ItemNo = o.ItemNo,
                                       InventoryLocationID = o.InventoryLocationID,
                                       SerialNoColl = o.SerialNoColl == null ? null : o.SerialNoColl.ToArray()
                                   }).ToArray();

                    isSuccess = ws.CheckSerialNoIsExist(wsInput, out message);
                }
                else
                    isSuccess = CheckSerialNoIsExistHelper(input, out message);
            }
            catch (Exception ex)
            {
                isSuccess = false;
                message = ex.Message;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }

        public static bool CheckSerialNoIsNotExistHelper(List<ItemTagModel> input, out string message)
        {

            bool isSuccess = true;
            message = "";

            if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.EnableProductSerialNo), false))
                return true;

            try
            {
                foreach (var lineInput in input)
                {
                    if (lineInput.SerialNoColl == null)
                        continue;
                    if (lineInput.SerialNoColl.Count == 0)
                        continue;

                    InventoryLocation invLoc = new InventoryLocation(lineInput.InventoryLocationID);
                    if (invLoc.IsNew)
                        continue;

                    Item item = new Item(lineInput.ItemNo);
                    if (item.IsNew || item.Deleted.GetValueOrDefault(false))
                        continue;

                    foreach (var serialNo in lineInput.SerialNoColl)
                    {
                        string sql = @"
                        SELECT  COUNT(*) RowNo
                        FROM	ItemTagSummary
                        WHERE	ISNULL(Deleted,0) = 0
		                        AND ISNULL(IsAvailable,0) = 1
		                        AND ItemNo = @ItemNo
		                        AND SerialNo = @SerialNo
		                        AND InventoryLocationID = @InventoryLocationID";

                        QueryCommand cmd = new QueryCommand(sql);
                        cmd.AddParameter("@ItemNo", lineInput.ItemNo, System.Data.DbType.String);
                        cmd.AddParameter("@SerialNo", serialNo, System.Data.DbType.String);
                        cmd.AddParameter("@InventoryLocationID", lineInput.InventoryLocationID, System.Data.DbType.Int32);

                        var dt = new DataTable();
                        dt.Load(DataService.GetReader(cmd));

                        bool isExist = false;
                        if (dt.Rows.Count > 0)
                            isExist = (dt.Rows[0]["RowNo"] + "").GetIntValue() > 0;

                        if (isExist)
                        {
                            isSuccess = false;
                            message += string.Format("Serial No {0} for item {1} already exist in {2}", serialNo, item.ItemName, invLoc.InventoryLocationName);
                            message += Environment.NewLine;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }

        public static bool CheckSerialNoIsNotExist(List<ItemTagModel> input, out string message)
        {
            bool isSuccess = true;
            message = "";

            if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.EnableProductSerialNo), false))
                return true;

            try
            {
                string XMLFILENAME = "\\WS.XML";
                string WS_URL = "http://localhost:51831/PowerPOSWeb/Synchronization/Synchronization.asmx";
                DataSet dsURL = new DataSet();
                dsURL.ReadXml(AppDomain.CurrentDomain.BaseDirectory + XMLFILENAME);
                WS_URL = dsURL.Tables[0].Rows[0]["URL"].ToString();
                if (!WS_URL.ToLower().StartsWith("http://localhost:"))
                {

                    PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                    ws.Url = WS_URL;
                    var wsInput = (from o in input
                                   select new PowerPOSLib.PowerPOSSync.ItemTagModel
                                   {
                                       ItemNo = o.ItemNo,
                                       InventoryLocationID = o.InventoryLocationID,
                                       SerialNoColl = o.SerialNoColl == null ? null : o.SerialNoColl.ToArray()
                                   }).ToArray();

                    isSuccess = ws.CheckSerialNoIsNotExist(wsInput, out message);
                }
                else
                    isSuccess = CheckSerialNoIsNotExistHelper(input, out message);
            }
            catch (Exception ex)
            {
                isSuccess = false;
                message = ex.Message;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }
    }

    [Serializable]
    public class ItemTagModel
    {
        public string ItemNo { set; get; }
        public int InventoryLocationID { set; get; }
        public List<string> SerialNoColl { set; get; }
    }
}
