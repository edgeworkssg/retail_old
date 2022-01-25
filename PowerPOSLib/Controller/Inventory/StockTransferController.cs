using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic;
using System.Data;
using PowerPOS.Container;
using System.Web.Script.Serialization;

namespace PowerPOS
{
    public class TransferHdr
    {
        public string StockTransferHdrRefNo { get; set; }
        public DateTime TransferDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public int TransferFromLocationID { get; set; }
        public string TransferFromLocation { get; set; }
        public int TransferToLocationID { get; set; }
        public string TransferToLocation { get; set; }
        public string RequestBy { get; set; }
        public string Status { get; set; }
        public string Remark { get; set; }
        public string PriceLevel { get; set; }
        public int ReturnToWarehouseID { get; set; }
        public int ReturnToSupplierID { get; set; }
        public string CreditInvoiceNo { get; set; }
        public string InvoiceNo { get; set; }
        public bool AutoStockIn { get; set; }
    }

    public class TransferDet
    {
        public string StockTransferHdrRefNo { get; set; }
        public string StockTransferDetRefNo { get; set; }
        public string CategoryName { get; set; }
        public string ItemNo { get; set; }
        public string ItemName { get; set; }
        public string UOM { get; set; }
        public string ItemDepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int DepartmentOrder { get; set; }
        public double Quantity { get; set; }
        public double? FullFilledQuantity { get; set; }
        public string Status { get; set; }
        public string Remark { get; set; }
        public bool Deleted { get; set; }
        public double FactoryPrice { get; set; }
        public double P1Price { get; set; }
        public double P2Price { get; set; }
        public double P3Price { get; set; }
        public double P4Price { get; set; }
        public double P5Price { get; set; }
    }

    public static class StockTransferController
    {
        public const string REFNO_PREFIX = "ST";

        public static TransferHdr FetchTransferHdr(string stockTransferHdrRefNo, out string status)
        {
            status = "";
            TransferHdr data = new TransferHdr();

            try
            {
                StockTransferHdr sth = new StockTransferHdr(stockTransferHdrRefNo);
                if (sth.IsNew)
                    throw new Exception("Stock Transfer not found");

                InventoryLocation ilTo = new InventoryLocation(sth.TransferToLocationID.GetValueOrDefault(0));
                InventoryLocation ilFrom = new InventoryLocation(sth.TransferFromLocationID.GetValueOrDefault(0));

                data.StockTransferHdrRefNo = sth.StockTransferHdrRefNo;
                data.TransferDate = sth.TransferDate;
                data.RequiredDate = sth.RequiredDate;
                data.TransferFromLocationID = sth.TransferFromLocationID.GetValueOrDefault(0);
                data.TransferFromLocation = ilFrom.InventoryLocationName;

                data.TransferToLocationID = sth.TransferToLocationID.GetValueOrDefault(0);
                data.TransferToLocation = ilTo.InventoryLocationName;

                data.RequestBy = sth.UserName;
                data.Status = sth.Status;
                data.Remark = sth.Remark;
                data.PriceLevel = sth.PriceLevel;

                data.ReturnToWarehouseID = sth.ReturnToWarehouseID;
                data.ReturnToSupplierID = sth.ReturnToSupplierID;
                data.CreditInvoiceNo = sth.CreditInvoiceNo;
                data.InvoiceNo = sth.InvoiceNo;
                data.AutoStockIn = sth.AutoStockIn;
            }
            catch (Exception ex)
            {
                status = "Error : " + ex.Message;
                Logger.writeLog(ex);
            }

            return data;
        }

        public static List<TransferHdr> FetchTransferList(int fromInvLocID, int toInvLocID,
                                                    string docNo, string status,
                                                    int take, int skip,
                                                    string orderBy, string isAsc, out int totalData)
        {
            totalData = 0;
            List<TransferHdr> data = new List<TransferHdr>();

            try
            {
                if (string.IsNullOrEmpty(status))
                    status = "ALL";

                if (string.IsNullOrEmpty(orderBy))
                    orderBy = "StockTransferHdrRefNo";

                string orderDir = isAsc.ToLower().Equals("true") ? "ASC" : "DESC";

                if (orderBy.ToLower().Equals("transferfromlocationid"))
                    orderBy = "ISNULL(STH.TransferFromLocationID,0)";
                else if (orderBy.ToLower().Equals("transferfromlocation"))
                    orderBy = "ISNULL(ILFROM.InventoryLocationName,'')";
                else if (orderBy.ToLower().Equals("transfertolocationid"))
                    orderBy = "ISNULL(STH.TransferToLocationID,0)";
                else if (orderBy.ToLower().Equals("transfertolocation"))
                    orderBy = "ISNULL(ILTO.InventoryLocationName,'')";
                else if (orderBy.ToLower().Equals("requestby"))
                    orderBy = "STH.UserName";

                string sql = @"DECLARE @FromInvLocID AS INT;
                                DECLARE @ToInvLocID AS INT;
                                DECLARE @DocNo AS NVARCHAR(200);
                                DECLARE @Status AS NVARCHAR(200);
                                DECLARE @Skip AS INT;
                                DECLARE @Take AS INT;

                                SET @FromInvLocID = {0};
                                SET @ToInvLocID = {1};
                                SET @DocNo = N'{2}';
                                SET @Status = N'{3}';
                                SET @Skip = {4};
                                SET @Take = {5};

                                SELECT	 TAB.StockTransferHdrRefNo
		                                ,TAB.TransferDate
		                                ,TAB.RequiredDate
		                                ,TAB.TransferFromLocationID
		                                ,TAB.TransferFromLocation
		                                ,TAB.TransferToLocationID
		                                ,TAB.TransferToLocation
		                                ,TAB.RequestBy
		                                ,TAB.Status
		                                ,TAB.Remark
                                FROM	(
	                                SELECT   ROW_NUMBER() OVER(ORDER BY {6} {7}) RowNo
			                                ,STH.StockTransferHdrRefNo
			                                ,STH.TransferDate
			                                ,STH.RequiredDate
			                                ,STH.TransferFromLocationID
			                                ,ILFROM.InventoryLocationName TransferFromLocation
			                                ,ISNULL(STH.TransferToLocationID,0) TransferToLocationID
			                                ,ISNULL(ILTO.InventoryLocationName,'') TransferToLocation
			                                ,STH.UserName RequestBy
			                                ,STH.Status
			                                ,STH.Remark
	                                FROM	StockTransferHdr STH
			                                LEFT JOIN InventoryLocation ILFROM ON ILFROM.InventoryLocationID = STH.TransferFromLocationID
			                                LEFT JOIN InventoryLocation ILTO ON ILTO.InventoryLocationID = STH.TransferToLocationID
	                                WHERE	ISNULL(STH.Deleted,0) = 0
			                                AND (@FromInvLocID = 0 OR ISNULL(STH.TransferFromLocationID,0) = @FromInvLocID)
			                                AND (@ToInvLocID = 0 OR ISNULL(STH.TransferToLocationID,0) = @ToInvLocID)
			                                AND (STH.StockTransferHdrRefNo LIKE '%'+@DocNo+'%')
			                                AND (@Status = 'ALL' OR STH.Status = @Status)
	                                ) TAB
                                WHERE	TAB.RowNo > @Skip AND TAB.RowNo < (@Skip + @Take)
                                ORDER BY TAB.RowNo";
                sql = string.Format(sql, fromInvLocID
                                       , toInvLocID
                                       , docNo
                                       , status
                                       , skip
                                       , take
                                       , orderBy
                                       , orderDir);
                DataTable dt = new DataTable();
                dt.Load(DataService.GetReader(new QueryCommand(sql)));

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TransferHdr hdr = new TransferHdr();
                    hdr.StockTransferHdrRefNo = (string)dt.Rows[i]["StockTransferHdrRefNo"];
                    hdr.TransferDate = (DateTime)dt.Rows[i]["TransferDate"];
                    hdr.RequiredDate = (DateTime)dt.Rows[i]["RequiredDate"];
                    hdr.TransferFromLocationID = (int)dt.Rows[i]["TransferFromLocationID"];
                    hdr.TransferFromLocation = (string)dt.Rows[i]["TransferFromLocation"];
                    hdr.TransferToLocationID = (int)dt.Rows[i]["TransferToLocationID"];
                    hdr.TransferToLocation = (string)dt.Rows[i]["TransferToLocation"];
                    hdr.RequestBy = (string)dt.Rows[i]["RequestBy"];
                    hdr.Status = (string)dt.Rows[i]["Status"];
                    hdr.Remark = (string)dt.Rows[i]["Remark"];

                    data.Add(hdr);
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return data;
        }

        public static List<TransferDet> FetchTransferDet(string stockTransferHdrRefNo)
        {
            List<TransferDet> data = new List<TransferDet>();

            try
            {
                string sql = @"SELECT  STD.StockTransferDetRefNo
		                                ,STD.StockTransferHdrRefNo
		                                ,I.ItemNo
		                                ,I.ItemName
                                        ,ISNULL(I.Userfld1,'') UOM
		                                ,CTG.CategoryName
		                                ,ID.ItemDepartmentID
		                                ,ID.DepartmentName
                                        ,ISNULL(ID.DepartmentOrder,0) DepartmentOrder
		                                ,STD.Quantity
		                                ,STD.FullFilledQuantity FullFilledQuantity
		                                ,STD.Status
		                                ,STD.Remark
		                                ,ISNULL(STD.Deleted,0) Deleted
                                        ,ISNULL(STD.FactoryPrice,0) FactoryPrice
                                        ,ISNULL(I.Userfloat6,0) P1Price
                                        ,ISNULL(I.Userfloat7,0) P2Price
                                        ,ISNULL(I.Userfloat8,0) P3Price
                                        ,ISNULL(I.Userfloat9,0) P4Price
                                        ,ISNULL(I.Userfloat10,0) P5Price
                                FROM	StockTransferDet STD
		                                LEFT JOIN Item I ON I.ItemNo = STD.ItemNo
		                                LEFT JOIN Category CTG ON CTG.CategoryName = I.CategoryName
		                                LEFT JOIN ItemDepartment ID ON ID.ItemDepartmentID = CTG.itemdepartmentid
                                WHERE	ISNULL(STD.Deleted,0) = 0 AND STD.StockTransferHdrRefNo = '{0}'
                                ORDER BY STD.StockTransferDetRefNo DESC";
                sql = string.Format(sql, stockTransferHdrRefNo);
                DataTable dt = new DataTable();
                dt.Load(DataService.GetReader(new QueryCommand(sql)));

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TransferDet det = new TransferDet();
                    det.StockTransferHdrRefNo = (dt.Rows[i]["StockTransferHdrRefNo"] + "");
                    det.StockTransferDetRefNo = (dt.Rows[i]["StockTransferDetRefNo"] + "");
                    det.CategoryName = (dt.Rows[i]["CategoryName"] + "");
                    det.ItemNo = (dt.Rows[i]["ItemNo"] + "");
                    det.ItemName = (dt.Rows[i]["ItemName"] + "");
                    det.UOM = (dt.Rows[i]["UOM"] + "");
                    det.ItemDepartmentID = (dt.Rows[i]["ItemDepartmentID"] + "");
                    det.DepartmentOrder = (dt.Rows[i]["DepartmentOrder"] + "").GetIntValue();
                    det.DepartmentName = (dt.Rows[i]["DepartmentName"] + "");
                    det.Quantity = (dt.Rows[i]["Quantity"] + "").GetDoubleValue();

                    double fullFillQty = 0;
                    if (double.TryParse(dt.Rows[i]["FullFilledQuantity"] + "", out fullFillQty))
                        det.FullFilledQuantity = fullFillQty;
                    else
                        det.FullFilledQuantity = null;

                    det.Status = (dt.Rows[i]["Status"] + "");
                    det.Remark = (dt.Rows[i]["Remark"] + "");
                    det.Deleted = (bool)dt.Rows[i]["Deleted"];
                    det.FactoryPrice = dt.Rows[i]["FactoryPrice"].ToString().GetDoubleValue();

                    det.P1Price = dt.Rows[i]["P1Price"].ToString().GetDoubleValue();
                    det.P2Price = dt.Rows[i]["P2Price"].ToString().GetDoubleValue();
                    det.P3Price = dt.Rows[i]["P3Price"].ToString().GetDoubleValue();
                    det.P4Price = dt.Rows[i]["P4Price"].ToString().GetDoubleValue();
                    det.P5Price = dt.Rows[i]["P5Price"].ToString().GetDoubleValue();

                    data.Add(det);
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return data;
        }

        public static bool SaveTransferHdr(TransferHdr data, string userName, out string stockTransferHdrRefNo, out string status)
        {
            bool isSuccess = false;
            status = "";
            stockTransferHdrRefNo = "";
            try
            {
                #region *) Validation

                InventoryLocation ilFrom = new InventoryLocation(data.TransferFromLocationID);
                if (ilFrom.IsNew)
                    throw new Exception("Source inventory location not found");

                InventoryLocation ilTo = new InventoryLocation(data.TransferToLocationID);
                if (ilTo.IsNew && data.TransferToLocationID != 0)
                    throw new Exception("Destination inventory location not found");

                #endregion

                QueryCommandCollection qmc = new QueryCommandCollection();

                StockTransferHdr sh = new StockTransferHdr(data.StockTransferHdrRefNo);
                if (sh.IsNew)
                {
                    string refNo = REFNO_PREFIX + DateTime.Now.ToString("yyyyMMdd");
                    string sql = @"SELECT	ISNULL(MAX(CAST(REPLACE(StockTransferHdrRefNo,'{0}','') AS INT)),0)+1 LastNo
                                    FROM	StockTransferHdr
                                    WHERE	StockTransferHdrRefNo LIKE '{0}%'";
                    sql = string.Format(sql, refNo);
                    DataTable dt = new DataTable();
                    dt.Load(DataService.GetReader(new QueryCommand(sql)));

                    if (dt.Rows.Count > 0)
                        sh.StockTransferHdrRefNo = refNo + (dt.Rows[0]["LastNo"] + "").GetIntValue().ToString("000");
                    else
                        throw new Exception("Failed generating new document ref no");
                    sh.Deleted = false;
                    sh.TransferFromLocationID = data.TransferFromLocationID;
                }
                if (data.TransferToLocationID != 0)
                    sh.TransferToLocationID = data.TransferToLocationID;
                sh.TransferDate = data.TransferDate;
                sh.RequiredDate = data.RequiredDate;
                sh.UserName = data.RequestBy;
                sh.Status = data.Status;
                sh.Remark = data.Remark;
                sh.PriceLevel = data.PriceLevel;
                sh.ReturnToSupplierID = data.ReturnToSupplierID;
                sh.ReturnToWarehouseID = data.ReturnToWarehouseID;
                sh.AutoStockIn = data.AutoStockIn;

                var detData = sh.StockTransferDetRecords().ToList();

                #region *) Get Default Price Level from Inventory Location
                bool showPriceLevel = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.ShowPriceLevelForWebOrder), false);
                if (showPriceLevel
                    && string.IsNullOrEmpty(sh.PriceLevel)
                    && sh.Status == "Submitted"
                    && sh.TransferFromLocationID.HasValue)
                {
                    InventoryLocation iloc = new InventoryLocation(sh.TransferFromLocationID.Value);
                    if (iloc != null && iloc.InventoryLocationID == sh.TransferFromLocationID.Value)
                    {
                        sh.PriceLevel = iloc.DefaultPriceLevel;

                        string priceLevel = sh.PriceLevel;
                        foreach (var std in detData)
                        {
                            if (!string.IsNullOrEmpty(priceLevel))
                            {
                                Item item = new Item(std.ItemNo);
                                if (item != null && item.ItemNo == std.ItemNo)
                                {
                                    if (priceLevel == "P1" && item.P1Price.HasValue)
                                        std.FactoryPrice = item.P1Price.Value;
                                    else if (priceLevel == "P2" && item.P2Price.HasValue)
                                        std.FactoryPrice = item.P2Price.Value;
                                    else if (priceLevel == "P3" && item.P3Price.HasValue)
                                        std.FactoryPrice = item.P3Price.Value;
                                    else if (priceLevel == "P4" && item.P4Price.HasValue)
                                        std.FactoryPrice = item.P4Price.Value;
                                    else if (priceLevel == "P5" && item.P5Price.HasValue)
                                        std.FactoryPrice = item.P5Price.Value;
                                }
                                else
                                {
                                    throw new Exception("Item not found : " + std.ItemNo);
                                }
                            }
                            else
                            {
                                std.FactoryPrice = 0;
                            }
                        }
                    }
                }
                #endregion

                if (sh.IsNew)
                    qmc.Add(sh.GetInsertCommand(userName));
                else
                    qmc.Add(sh.GetUpdateCommand(userName));

                foreach (var std in detData)
                {
                    std.CostOfGoods = std.FactoryPrice * Convert.ToDecimal(std.Quantity);
                    std.Status = sh.Status;
                    if (sh.Status.ToLower().Equals("submitted"))
                        std.FullFilledQuantity = null;
                    qmc.Add(std.GetUpdateCommand(userName));

                    if (sh.Status.ToLower().Equals("submitted") && (std.Deleted.GetValueOrDefault(false) || std.Quantity == 0))
                    {
                        string sql = string.Format("DELETE StockTransferDet WHERE StockTransferDetRefNo = '{0}'", std.StockTransferDetRefNo);
                        qmc.Add(new QueryCommand(sql));
                    }
                }

                stockTransferHdrRefNo = sh.StockTransferHdrRefNo;

                DataService.ExecuteTransaction(qmc);

                isSuccess = true;
            }
            catch (Exception ex)
            {
                isSuccess = false;
                status = ex.Message;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }

        public static bool AddTransferItem(string stockTransferHdrRefNo, string itemNo, double qty, string userName, out TransferDet transferDet, out string status)
        {
            bool isSuccess = false;
            status = "";
            transferDet = new TransferDet();

            try
            {
                StockTransferHdr sth = new StockTransferHdr(stockTransferHdrRefNo);
                if (sth.IsNew)
                    throw new Exception("Stock Transfer Hdr not found");

                Item theItem = new Item(itemNo);
                if (theItem.IsNew)
                    throw new Exception("Item not found");

                Category ctg = theItem.Category;
                ItemDepartment itemDept = new ItemDepartment(ctg.ItemDepartmentId);

                Query qr = new Query("StockTransferDet");
                qr.AddWhere(StockTransferDet.Columns.StockTransferHdrRefNo, stockTransferHdrRefNo);
                qr.AddWhere(StockTransferDet.Columns.Deleted, false);
                qr.AddWhere(StockTransferDet.Columns.ItemNo, itemNo);
                StockTransferDet std = new StockTransferDetController().FetchByQuery(qr).FirstOrDefault();
                if (std == null)
                {
                    string sql = @"SELECT  ISNULL(MAX(CAST(ISNULL(REPLACE(StockTransferDetRefNo,StockTransferHdrRefNo+'.',''),'0') AS INT)),0)+1 LastNo
                                    FROM	StockTransferDet
                                    WHERE	StockTransferHdrRefNo = '{0}'";
                    sql = string.Format(sql, stockTransferHdrRefNo);
                    int lastNo = 1;
                    DataTable dt = new DataTable();
                    dt.Load(DataService.GetReader(new QueryCommand(sql)));
                    if (dt.Rows.Count > 0)
                        lastNo = (dt.Rows[0]["LastNo"] + "").GetIntValue();

                    std = new StockTransferDet();
                    std.StockTransferHdrRefNo = stockTransferHdrRefNo;
                    std.StockTransferDetRefNo = stockTransferHdrRefNo + "." + lastNo;
                    std.ItemNo = itemNo;
                    std.Quantity = qty;
                    std.FullFilledQuantity = 0;
                    std.Deleted = false;
                }
                else
                {
                    std.Quantity += qty;
                }

                std.FactoryPrice = ItemSummaryController.GetAvgCostPrice(itemNo, sth.TransferFromLocationID.GetValueOrDefault(0));
                std.CostOfGoods = std.FactoryPrice * Convert.ToDecimal(std.Quantity);
                std.Status = sth.Status;
                std.Save(userName);

                transferDet.StockTransferHdrRefNo = std.StockTransferHdrRefNo;
                transferDet.StockTransferDetRefNo = std.StockTransferDetRefNo;
                transferDet.CategoryName = ctg.CategoryName;
                transferDet.ItemNo = std.ItemNo;
                transferDet.ItemName = theItem.ItemName;
                transferDet.UOM = theItem.UOM;
                transferDet.ItemDepartmentID = itemDept.ItemDepartmentID;
                transferDet.DepartmentOrder = itemDept.DepartmentOrder.GetValueOrDefault(0);
                transferDet.DepartmentName = itemDept.DepartmentName;
                transferDet.Quantity = std.Quantity;
                transferDet.FullFilledQuantity = std.FullFilledQuantity.GetValueOrDefault(0);
                transferDet.Status = std.Status;
                transferDet.Remark = std.Remark;
                transferDet.Deleted = std.Deleted.GetValueOrDefault(false);


                isSuccess = true;
            }
            catch (Exception ex)
            {
                status = "Error : " + ex.Message;
                Logger.writeLog(ex);
                isSuccess = false;
            }

            return isSuccess;
        }

        public static bool AddAllTransferItems(string stockTransferHdrRefNo, string userName, out string status)
        {
            bool isSuccess = false;
            status = "";

            try
            {
                StockTransferHdr sth = new StockTransferHdr(stockTransferHdrRefNo);
                if (sth.IsNew)
                    throw new Exception("Stock Transfer Hdr not found");

                string sql = @"DELETE StockTransferDet WHERE StockTransferHdrRefNo = '{0}'

                                INSERT INTO StockTransferDet (StockTransferHdrRefNo, StockTransferDetRefNo, ItemNo, Quantity, FullFilledQuantity, Deleted, FactoryPrice, CostOfGoods, Status,CreatedOn, CreatedBy, ModifiedOn, ModifiedBy)
                                SELECT   '{0}' StockTransferHdrRefNo
		                                ,'{0}.'+CAST(ROW_NUMBER() OVER(ORDER BY I.CategoryName, I.ItemName) AS VARCHAR(MAX)) StockTransferDetRefNo
		                                ,I.ItemNo
		                                ,0 Quantity
		                                ,0 FullFilledQuantity
		                                ,0 Deleted
		                                ,0 FactoryPrice
		                                ,0 CostOfGoods
		                                ,'Pending' Status
		                                ,GETDATE() CreatedOn
		                                ,'{1}' CreatedBy
		                                ,GETDATE() ModifiedOn
		                                ,'{1}' ModifiedBy
                                FROM	Item I
                                WHERE	ISNULL(I.Deleted,0) = 0
		                                AND ISNULL(I.IsInInventory,0) = 1
                                ORDER BY I.CategoryName, I.ItemName";
                sql = string.Format(sql, stockTransferHdrRefNo, userName);
                DataService.ExecuteQuery(new QueryCommand(sql));

                isSuccess = true;
            }
            catch (Exception ex)
            {
                status = "Error : " + ex.Message;
                Logger.writeLog(ex);
                isSuccess = false;
            }

            return isSuccess;
        }

        public static bool ChangeItemQty(string stockTransferDetRefNo, double qty, string userName, out string status)
        {
            status = "";
            bool isSucess = false;

            try
            {
                StockTransferDet std = new StockTransferDet(stockTransferDetRefNo);
                StockTransferHdr sth = std.StockTransferHdr;

                if (std.IsNew)
                    throw new Exception("Stock Transfer Det Not Found");

                std.Quantity = qty;
                std.FactoryPrice = ItemSummaryController.GetAvgCostPrice(std.ItemNo, sth.TransferFromLocationID.GetValueOrDefault(0));
                std.CostOfGoods = std.FactoryPrice * Convert.ToDecimal(std.Quantity);
                std.Save(userName);

                isSucess = true;
            }
            catch (Exception ex)
            {
                isSucess = false;
                status = "Error : " + ex.Message;
                Logger.writeLog(ex);
            }

            return isSucess;
        }

        public static bool DeleteTransferItem(List<string> stockTransferDetRefNo, string userName, out string status)
        {
            status = "";
            bool isSucess = false;

            try
            {
                string sql = string.Format("DELETE StockTransferDet WHERE StockTransferDetRefNo IN ('{0}')", string.Join("','", stockTransferDetRefNo.ToArray()));
                DataService.ExecuteQuery(new QueryCommand(sql));
                isSucess = true;
            }
            catch (Exception ex)
            {
                isSucess = false;
                status = "Error : " + ex.Message;
                Logger.writeLog(ex);
            }

            return isSucess;
        }

        public static bool ChangeFullFilledQty(string stockTransferDetRefNo, double fullFilledQty, string remark, string userName, out string status, out TransferDet transferDet)
        {
            bool isSuccess = false;
            transferDet = new TransferDet();
            status = "";
            try
            {
                StockTransferDet std = new StockTransferDet(stockTransferDetRefNo);
                if (std.IsNew)
                    throw new Exception("Stock Transfer Detail not found");

                Item theItem = std.Item;
                Category ctg = theItem.Category;
                ItemDepartment itemDept = new ItemDepartment(ctg.ItemDepartmentId);

                std.Remark = remark;
                std.FullFilledQuantity = fullFilledQty;
                std.Save(userName);

                transferDet.StockTransferHdrRefNo = std.StockTransferHdrRefNo;
                transferDet.StockTransferDetRefNo = std.StockTransferDetRefNo;
                transferDet.CategoryName = ctg.CategoryName;
                transferDet.ItemNo = std.ItemNo;
                transferDet.ItemName = theItem.ItemName;
                transferDet.UOM = theItem.UOM;
                transferDet.ItemDepartmentID = itemDept.ItemDepartmentID;
                transferDet.DepartmentOrder = itemDept.DepartmentOrder.GetValueOrDefault(0);
                transferDet.DepartmentName = itemDept.DepartmentName;
                transferDet.Quantity = std.Quantity;
                transferDet.FullFilledQuantity = std.FullFilledQuantity.GetValueOrDefault(0);
                transferDet.Status = std.Status;
                transferDet.Remark = std.Remark;
                transferDet.Deleted = std.Deleted.GetValueOrDefault(false);

                isSuccess = true;
            }
            catch (Exception ex)
            {
                isSuccess = false;
                status = "Error : " + ex.Message;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }

        public static bool RejectTransferDetail(List<string> stockTransferDetRefNo, string userName, out string status)
        {
            bool isSuccess = false;
            status = "";
            try
            {
                string sql = string.Format("UPDATE StockTransferDet SET FullFilledQuantity = 0, Status = 'Rejected' WHERE StockTransferDetRefNo IN ('{0}')", string.Join("','", stockTransferDetRefNo.ToArray()));
                DataService.ExecuteQuery(new QueryCommand(sql));
                isSuccess = true;
            }
            catch (Exception ex)
            {
                isSuccess = false;
                status = "Error : " + ex.Message;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }

        public static bool SetTallyAllStockTransfer(List<string> stockTransferDetRefNo, string userName, out string status)
        {
            bool isSuccess = false;
            status = "";

            try
            {
                string sql = string.Format("UPDATE StockTransferDet SET FullFilledQuantity = Quantity, Status = 'Submitted' WHERE StockTransferDetRefNo IN ('{0}')", string.Join("','", stockTransferDetRefNo.ToArray()));
                DataService.ExecuteQuery(new QueryCommand(sql));
                isSuccess = true;
            }
            catch (Exception ex)
            {
                isSuccess = false;
                status = "Error : " + ex.Message;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }

        public static bool SetTallyAllStockTransfer(string stockTransferHdrRefNo, string userName, out string status)
        {
            bool isSuccess = false;
            status = "";

            try
            {
                string sql = string.Format("UPDATE StockTransferDet SET FullFilledQuantity = Quantity, Status = 'Submitted' WHERE StockTransferHdrRefNo ='{0}'", stockTransferHdrRefNo);
                DataService.ExecuteQuery(new QueryCommand(sql));
                isSuccess = true;
            }
            catch (Exception ex)
            {
                isSuccess = false;
                status = "Error : " + ex.Message;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }

        public static bool ReceiveStockTransfer(string stockTransferHdrRefNo, string userName, out string status)
        {
            bool isSuccess = false;
            status = "";

            try
            {
                StockTransferHdr sth = new StockTransferHdr(stockTransferHdrRefNo);
                if (sth.IsNew)
                    throw new Exception("Stock Transfer Not Found");

                if (sth.UserName.ToLower().Equals(userName.ToLower()))
                    throw new Exception("Receive Transfer cannot done by same person. Please login with different account");

                bool useTransferApproval = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.UseTransferApproval),false);

                if (!useTransferApproval && sth.Status.ToLower() != "submitted")
                    throw new Exception("Stockt Transfer status must be Submitted");
                else if (useTransferApproval && sth.Status.ToLower() != "approved")
                    throw new Exception("Stockt Transfer status must be Approved");

                string sql = @"SELECT    StockTransferDetRefNo
		                                ,FullFilledQuantity
		                                ,ItemNo
                                        ,FactoryPrice
                                FROM	StockTransferDet 
                                WHERE	FullFilledQuantity > 0
		                                AND StockTransferHdrRefNo = '{0}'";
                sql = string.Format(sql, stockTransferHdrRefNo);
                DataTable dt = new DataTable();
                dt.Load(DataService.GetReader(new QueryCommand(sql)));

                QueryCommandCollection qmc = new QueryCommandCollection();

                if (dt.Rows.Count == 0)
                {
                    string updateHdrSql = "UPDATE StockTransferHdr SET Status = 'Rejected', ModifiedOn = GETDATE(), ModifiedBy = '{1}', Userfld3 = '{1}' WHERE StockTransferHdrRefNo = '{0}'";
                    updateHdrSql = string.Format(updateHdrSql, stockTransferHdrRefNo, userName);

                    string updateDetSql = "UPDATE StockTransferDet SET Status = 'Rejected', ModifiedOn = GETDATE(), ModifiedBy = '{1}' WHERE StockTransferHdrRefNo = '{0}'";
                    updateDetSql = string.Format(updateDetSql, stockTransferHdrRefNo, userName);

                    qmc.Add(new QueryCommand(updateHdrSql));
                    qmc.Add(new QueryCommand(updateDetSql));
                }
                else
                {
                    bool isTransferToWH = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.StockTransferWillGoThroughWarehouse), false);

                    decimal totalCogs = 0;
                    string InventoryHdrRefFrom = "";
                    string InventoryHdrRefTo = "";

                    if (isTransferToWH)
                    {
                        #region *) Transfer to warehouse first
                        DataTable dtDirect = dt.Clone();

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string itemNo = dt.Rows[i]["ItemNo"] + "";
                            string stockTransferDetRefNo = dt.Rows[i]["StockTransferDetRefNo"] + "";
                            decimal qty = (dt.Rows[i]["FullFilledQuantity"] + "").GetDecimalValue();
                            decimal factoryPrice = (dt.Rows[i]["FactoryPrice"] + "").GetDecimalValue();
                            decimal costOfGoods = factoryPrice * Convert.ToDecimal(qty);

                            QueryCommandCollection cmdCol = new QueryCommandCollection();
                            if (qty > 0)
                            {
                                int? supplierID = ItemSupplierMapController.GetPreferredSupplier(itemNo);
                                if (!supplierID.HasValue)
                                {
                                    dtDirect.Rows.Add(dt.Rows[i].ItemArray);
                                    continue;
                                }

                                Supplier s = new Supplier(supplierID.Value);
                                if (!s.IsWarehouse.GetValueOrDefault(false) || !s.WarehouseID.HasValue)
                                {
                                    dtDirect.Rows.Add(dt.Rows[i].ItemArray);
                                    continue;
                                }

                                int warehouseID = s.WarehouseID.Value;

                                // Will return stock to warehouse first, and then warehouse will transfer to destination location

                                PowerPOS.Container.CostingMethods CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
                                string strCostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
                                if (strCostingMethod.ToLower() == "fifo")
                                    CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
                                else if (strCostingMethod.ToLower() == "fixed avg")
                                    CostingMethod = PowerPOS.Container.CostingMethods.FixedAvg;
                                else
                                    CostingMethod = PowerPOS.Container.CostingMethods.FIFO;

                                #region *) Return Out
                                InventoryController ctrlReturnOut = new InventoryController(CostingMethod);
                                ctrlReturnOut.SetInventoryHeaderInfo(sth.StockTransferHdrRefNo, "", "Transfer: " + sth.StockTransferHdrRefNo, 0, 0, 0);
                                ctrlReturnOut.AddItemIntoInventoryStockReturn(itemNo, qty, factoryPrice, false, out status);
                                if (status == "")
                                {
                                    qmc.AddRange(ctrlReturnOut.CreateStockReturnQueryCommand(userName, 1, sth.TransferFromLocationID.GetValueOrDefault(0), false, true));
                                }
                                #endregion

                                #region *) Return In
                                InventoryController ctrlReturnIn = new InventoryController(CostingMethod);
                                ctrlReturnIn.SetInventoryHeaderInfo(sth.StockTransferHdrRefNo, "", "Transfer: " + sth.StockTransferHdrRefNo, 0, 0, 0);
                                ctrlReturnIn.AddItemIntoInventoryStockIn(itemNo, qty, factoryPrice, out status);
                                
                                if (status == "")
                                {
                                    qmc.AddRange(ctrlReturnIn.CreateReturnInQueryCommand(userName, warehouseID, false, true));
                                }
                                #endregion

                                //DataService.ExecuteTransaction(cmdCol);
                                //cmdCol = new QueryCommandCollection(); // reset

                                if (warehouseID != sth.TransferToLocationID.GetValueOrDefault(0))
                                {
                                    #region *) Transfer
                                    InventoryController invCtrlTransfer = new InventoryController();
                                    invCtrlTransfer.SetInventoryHeaderInfo(sth.StockTransferHdrRefNo, "", "Transfer: " + sth.StockTransferHdrRefNo, 0, 0, 0);
                                    invCtrlTransfer.AddItemIntoInventoryStockIn(itemNo, qty, factoryPrice, out status);
                                    QueryCommandCollection stockOut = new QueryCommandCollection();
                                    if (!invCtrlTransfer.TransferOutAutoReceiveWithQueryCommand(userName, warehouseID, sth.TransferToLocationID.GetValueOrDefault(0), out InventoryHdrRefFrom, out InventoryHdrRefTo,out stockOut, out status))
                                    {
                                        throw new Exception(status);
                                    }
                                    qmc.AddRange(stockOut);
                                    #endregion
                                }
                            }
                        }
                        #endregion

                        // For those items that do not transfer to warehouse first
                        #region *) Direct transfer only
                        InventoryController invCtrlIn = new InventoryController();
                        invCtrlIn.SetInventoryHeaderInfo(sth.StockTransferHdrRefNo, "", "Transfer: " + sth.StockTransferHdrRefNo, 0, 0, 0);

                        for (int i = 0; i < dtDirect.Rows.Count; i++)
                        {
                            string itemNo = dtDirect.Rows[i]["ItemNo"] + "";
                            string stockTransferDetRefNo = dtDirect.Rows[i]["StockTransferDetRefNo"] + "";
                            decimal qty = (dtDirect.Rows[i]["FullFilledQuantity"] + "").GetDecimalValue();
                            decimal factoryPrice = (dtDirect.Rows[i]["FactoryPrice"] + "").GetDecimalValue();
                            decimal costOfGoods = factoryPrice * Convert.ToDecimal(qty);
                            if (qty > 0)
                            {
                                invCtrlIn.AddItemIntoInventoryStockIn(itemNo, qty, factoryPrice, out status);

                                string updateDetSql = @"UPDATE	 StockTransferDet
                                                    SET		 FactoryPrice = '{2}'
		                                                    ,CostOfGoods = Quantity * '{2}'
		                                                    ,ModifiedOn = GETDATE()
		                                                    ,ModifiedBy = '{1}' 
                                                    WHERE StockTransferDetRefNo = '{0}'";
                                updateDetSql = string.Format(updateDetSql, stockTransferHdrRefNo
                                                                         , userName
                                                                         , factoryPrice.ToString("0.000"));
                                qmc.Add(new QueryCommand(updateDetSql));
                            }
                        }
                        QueryCommandCollection so = new QueryCommandCollection();
                        if (!invCtrlIn.TransferOutAutoReceiveWithQueryCommand(userName, sth.TransferFromLocationID.GetValueOrDefault(0), sth.TransferToLocationID.GetValueOrDefault(0), out InventoryHdrRefFrom, out InventoryHdrRefTo,out so, out status))
                        {
                            throw new Exception(status);
                        }
                        qmc.AddRange(so);
                        #endregion
                    }
                    else
                    {
                        #region *) Direct transfer only
                        InventoryController invCtrlIn = new InventoryController();
                        invCtrlIn.SetInventoryHeaderInfo(sth.StockTransferHdrRefNo, "", "Transfer: " + sth.StockTransferHdrRefNo, 0, 0, 0);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string itemNo = dt.Rows[i]["ItemNo"] + "";
                            string stockTransferDetRefNo = dt.Rows[i]["StockTransferDetRefNo"] + "";
                            decimal qty = (dt.Rows[i]["FullFilledQuantity"] + "").GetDecimalValue();
                            decimal factoryPrice = (dt.Rows[i]["FactoryPrice"] + "").GetDecimalValue();
                            //decimal factoryPrice = ItemSummaryController.GetAvgCostPrice(itemNo, sth.TransferFromLocationID.GetValueOrDefault(0));
                            decimal costOfGoods = factoryPrice * Convert.ToDecimal(qty);
                            if (qty > 0)
                            {
                                //invCtrlIn.AddItemIntoInventory(itemNo, qty, out status);
                                invCtrlIn.AddItemIntoInventoryStockIn(itemNo, qty, factoryPrice, out status);

                                //invCtrlIn.AddItemIntoInventory(stockTransferDetRefNo, itemNo, qty, factoryPrice, costOfGoods, out status);
                                //invCtrlOut.AddItemIntoInventory(stockTransferDetRefNo, itemNo, qty, factoryPrice, costOfGoods, out status);

                                string updateDetSql = @"UPDATE	 StockTransferDet
                                                    SET		 FactoryPrice = '{2}'
		                                                    ,CostOfGoods = Quantity * '{2}'
		                                                    ,ModifiedOn = GETDATE()
		                                                    ,ModifiedBy = '{1}' 
                                                    WHERE StockTransferDetRefNo = '{0}'";
                                updateDetSql = string.Format(updateDetSql, stockTransferHdrRefNo
                                                                         , userName
                                                                         , factoryPrice.ToString("0.000"));
                                qmc.Add(new QueryCommand(updateDetSql));
                            }
                        }

                        int pointOfSaleID = 0;
                        Setting st = new SettingController().FetchAll().FirstOrDefault();
                        if (st != null)
                            pointOfSaleID = st.PointOfSaleID;

                        QueryCommandCollection StockOut = new QueryCommandCollection();
                        if (!invCtrlIn.TransferOutAutoReceiveWithQueryCommand(userName, sth.TransferFromLocationID.GetValueOrDefault(0), sth.TransferToLocationID.GetValueOrDefault(0), out InventoryHdrRefFrom, out InventoryHdrRefTo, out StockOut, out status))
                        {
                            throw new Exception(status);
                        }

                        qmc.AddRange(StockOut);

                        //var cmdIn = invCtrlIn.StockInCommandServer(userName, sth.TransferToLocationID.GetValueOrDefault(0), pointOfSaleID, false, 0, out status);
                        //var cmdOut = invCtrlOut.StockOutCommandServer(userName, 0, sth.TransferFromLocationID.GetValueOrDefault(0), pointOfSaleID, 1, out status, out totalCogs);

                        //qmc.AddRange(cmdIn);
                        //qmc.AddRange(cmdOut);
                        #endregion
                    }

                    string updateHdrSql = @"UPDATE	 StockTransferHdr 
                                            SET		 Status = 'Received'
		                                            ,Userfld1 = '{2}'
		                                            ,Userfld2 = '{3}'
		                                            ,Userfloat1 = '{4}'
		                                            ,ModifiedOn = GETDATE()
		                                            ,ModifiedBy = '{1}' 
                                            WHERE StockTransferHdrRefNo = '{0}'";
                    updateHdrSql = string.Format(updateHdrSql, stockTransferHdrRefNo
                                                             , userName
                                                             , InventoryHdrRefTo
                                                             , InventoryHdrRefFrom
                                                             , totalCogs.ToString("0.000"));

                    string updateDetSql1 = "UPDATE StockTransferDet SET Status = 'Received', ModifiedOn = GETDATE(), ModifiedBy = '{1}' WHERE StockTransferHdrRefNo = '{0}' AND ISNULL(FullFilledQuantity,0) > 0";
                    updateDetSql1 = string.Format(updateDetSql1, stockTransferHdrRefNo, userName);

                    string updateDetSql2 = "UPDATE StockTransferDet SET Status = 'Rejected', ModifiedOn = GETDATE(), ModifiedBy = '{1}' WHERE StockTransferHdrRefNo = '{0}' AND ISNULL(FullFilledQuantity,0) = 0";
                    updateDetSql2 = string.Format(updateDetSql2, stockTransferHdrRefNo, userName);

                    qmc.Add(new QueryCommand(updateHdrSql));
                    qmc.Add(new QueryCommand(updateDetSql1));
                    qmc.Add(new QueryCommand(updateDetSql2));
                }

                DataService.ExecuteTransaction(qmc);

                isSuccess = true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                isSuccess = false;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }

        public static bool ReceiveStockTransferThroughWarehouse(string stockTransferHdrRefNo, string userName, out string status)
        {
            bool isSuccess = false;
            status = "";

            try
            {
                StockTransferHdr sth = new StockTransferHdr(stockTransferHdrRefNo);
                if (sth.IsNew)
                    throw new Exception("Stock Transfer Not Found");

                if (sth.UserName.ToLower().Equals(userName.ToLower()))
                    throw new Exception("Receive Transfer cannot done by same person. Please login with different account");

                bool useTransferApproval = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.UseTransferApproval), false);

                if (!useTransferApproval && sth.Status.ToLower() != "submitted")
                    throw new Exception("Stockt Transfer status must be Submitted");
                else if (useTransferApproval && sth.Status.ToLower() != "approved")
                    throw new Exception("Stockt Transfer status must be Approved");

                string sql = @"SELECT    StockTransferDetRefNo
		                                ,FullFilledQuantity
		                                ,ItemNo
                                        ,FactoryPrice
                                FROM	StockTransferDet 
                                WHERE	FullFilledQuantity > 0
		                                AND StockTransferHdrRefNo = '{0}'";
                sql = string.Format(sql, stockTransferHdrRefNo);
                DataTable dt = new DataTable();
                dt.Load(DataService.GetReader(new QueryCommand(sql)));

                QueryCommandCollection qmc = new QueryCommandCollection();

                if (dt.Rows.Count == 0)
                {
                    string updateHdrSql = "UPDATE StockTransferHdr SET Status = 'Rejected', ModifiedOn = GETDATE(), ModifiedBy = '{1}', Userfld3 = '{1}' WHERE StockTransferHdrRefNo = '{0}'";
                    updateHdrSql = string.Format(updateHdrSql, stockTransferHdrRefNo, userName);

                    string updateDetSql = "UPDATE StockTransferDet SET Status = 'Rejected', ModifiedOn = GETDATE(), ModifiedBy = '{1}' WHERE StockTransferHdrRefNo = '{0}'";
                    updateDetSql = string.Format(updateDetSql, stockTransferHdrRefNo, userName);

                    qmc.Add(new QueryCommand(updateHdrSql));
                    qmc.Add(new QueryCommand(updateDetSql));
                }
                else
                {
                    bool isTransferToWH = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.StockTransferWillGoThroughWarehouse), false);

                    decimal totalCogs = 0;
                    string InventoryHdrRefFrom = "";
                    string InventoryHdrRefTo = "";

                    if (!isTransferToWH)
                    {
                        #region *) Direct transfer only
                        InventoryController invCtrlIn = new InventoryController();
                        invCtrlIn.SetInventoryHeaderInfo(sth.StockTransferHdrRefNo, "", "Transfer: " + sth.StockTransferHdrRefNo, 0, 0, 0);


                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string itemNo = dt.Rows[i]["ItemNo"] + "";
                            string stockTransferDetRefNo = dt.Rows[i]["StockTransferDetRefNo"] + "";
                            decimal qty = (dt.Rows[i]["FullFilledQuantity"] + "").GetDecimalValue();
                            decimal factoryPrice = (dt.Rows[i]["FactoryPrice"] + "").GetDecimalValue();
                            //decimal factoryPrice = ItemSummaryController.GetAvgCostPrice(itemNo, sth.TransferFromLocationID.GetValueOrDefault(0));
                            decimal costOfGoods = factoryPrice * Convert.ToDecimal(qty);
                            if (qty > 0)
                            {
                                //invCtrlIn.AddItemIntoInventory(itemNo, qty, out status);
                                invCtrlIn.AddItemIntoInventoryStockIn(itemNo, qty, factoryPrice, out status);

                                //invCtrlIn.AddItemIntoInventory(stockTransferDetRefNo, itemNo, qty, factoryPrice, costOfGoods, out status);
                                //invCtrlOut.AddItemIntoInventory(stockTransferDetRefNo, itemNo, qty, factoryPrice, costOfGoods, out status);

                                string updateDetSql = @"UPDATE	 StockTransferDet
                                                SET		 FactoryPrice = '{2}'
                                                        ,CostOfGoods = Quantity * '{2}'
                                                        ,ModifiedOn = GETDATE()
                                                        ,ModifiedBy = '{1}' 
                                                WHERE StockTransferDetRefNo = '{0}'";
                                updateDetSql = string.Format(updateDetSql, stockTransferHdrRefNo
                                                                         , userName
                                                                         , factoryPrice.ToString("0.000"));
                                qmc.Add(new QueryCommand(updateDetSql));
                            }
                        }

                        int pointOfSaleID = 0;
                        Setting st = new SettingController().FetchAll().FirstOrDefault();
                        if (st != null)
                            pointOfSaleID = st.PointOfSaleID;

                        QueryCommandCollection StockOut = new QueryCommandCollection();
                        if (!invCtrlIn.TransferOutAutoReceive(userName, sth.TransferFromLocationID.GetValueOrDefault(0), sth.TransferToLocationID.GetValueOrDefault(0), out InventoryHdrRefFrom, out InventoryHdrRefTo, out status))
                        {
                            throw new Exception(status);
                        }

                        //var cmdIn = invCtrlIn.StockInCommandServer(userName, sth.TransferToLocationID.GetValueOrDefault(0), pointOfSaleID, false, 0, out status);
                        //var cmdOut = invCtrlOut.StockOutCommandServer(userName, 0, sth.TransferFromLocationID.GetValueOrDefault(0), pointOfSaleID, 1, out status, out totalCogs);

                        //qmc.AddRange(cmdIn);
                        //qmc.AddRange(cmdOut);
                        #endregion


                        string updateHdrSql = @"UPDATE	 StockTransferHdr 
                                            SET		 Status = 'Received'
		                                            ,Userfld1 = '{2}'
		                                            ,Userfld2 = '{3}'
		                                            ,Userfloat1 = '{4}'
		                                            ,ModifiedOn = GETDATE()
		                                            ,ModifiedBy = '{1}' 
                                            WHERE StockTransferHdrRefNo = '{0}'";
                        updateHdrSql = string.Format(updateHdrSql, stockTransferHdrRefNo
                                                                 , userName
                                                                 , InventoryHdrRefTo
                                                                 , InventoryHdrRefFrom
                                                                 , totalCogs.ToString("0.000"));

                        string updateDetSql1 = "UPDATE StockTransferDet SET Status = 'Received', ModifiedOn = GETDATE(), ModifiedBy = '{1}' WHERE StockTransferHdrRefNo = '{0}' AND ISNULL(FullFilledQuantity,0) > 0";
                        updateDetSql1 = string.Format(updateDetSql1, stockTransferHdrRefNo, userName);

                        string updateDetSql2 = "UPDATE StockTransferDet SET Status = 'Rejected', ModifiedOn = GETDATE(), ModifiedBy = '{1}' WHERE StockTransferHdrRefNo = '{0}' AND ISNULL(FullFilledQuantity,0) = 0";
                        updateDetSql2 = string.Format(updateDetSql2, stockTransferHdrRefNo, userName);

                        qmc.Add(new QueryCommand(updateHdrSql));
                        qmc.Add(new QueryCommand(updateDetSql1));
                        qmc.Add(new QueryCommand(updateDetSql2));
                    }
                    else
                    {
                        if (!sth.AutoStockIn)
                        {
                            #region *) Transfer In
                            InventoryController invCtrlIn = new InventoryController();
                            invCtrlIn.SetInventoryHeaderInfo(sth.StockTransferHdrRefNo, "", "Transfer: " + sth.StockTransferHdrRefNo, 0, 0, 0);


                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                string itemNo = dt.Rows[i]["ItemNo"] + "";
                                string stockTransferDetRefNo = dt.Rows[i]["StockTransferDetRefNo"] + "";
                                decimal qty = (dt.Rows[i]["FullFilledQuantity"] + "").GetDecimalValue();
                                decimal factoryPrice = (dt.Rows[i]["FactoryPrice"] + "").GetDecimalValue();
                               
                                decimal costOfGoods = factoryPrice * Convert.ToDecimal(qty);
                                if (qty > 0)
                                {
                                    //invCtrlIn.AddItemIntoInventory(itemNo, qty, out status);
                                    invCtrlIn.AddItemIntoInventoryStockIn(itemNo, qty, factoryPrice, out status);

                                    string updateDetSql = @"UPDATE	 StockTransferDet
                                                SET		 FactoryPrice = '{2}'
                                                        ,CostOfGoods = Quantity * '{2}'
                                                        ,ModifiedOn = GETDATE()
                                                        ,ModifiedBy = '{1}' 
                                                WHERE StockTransferDetRefNo = '{0}'";
                                    updateDetSql = string.Format(updateDetSql, stockTransferHdrRefNo
                                                                             , userName
                                                                             , factoryPrice.ToString("0.000"));
                                    qmc.Add(new QueryCommand(updateDetSql));
                                }
                            }

                            int pointOfSaleID = 0;
                            Setting st = new SettingController().FetchAll().FirstOrDefault();
                            if (st != null)
                                pointOfSaleID = st.PointOfSaleID;

                            QueryCommandCollection StockOut = new QueryCommandCollection();
                            if (!invCtrlIn.TransferIn(userName, sth.ReturnToWarehouseID, sth.TransferToLocationID.GetValueOrDefault(0), out InventoryHdrRefTo, out status))
                            {
                                throw new Exception(status);
                            }

                            #endregion


                            string updateHdrSql = @"UPDATE	 StockTransferHdr 
                                            SET		 Status = 'Received'
		                                            ,Userfld2 = '{2}'
		                                            ,Userfloat1 = '{3}'
		                                            ,ModifiedOn = GETDATE()
		                                            ,ModifiedBy = '{1}' 
                                            WHERE StockTransferHdrRefNo = '{0}'";
                            updateHdrSql = string.Format(updateHdrSql, stockTransferHdrRefNo
                                                                     , userName
                                                                     , InventoryHdrRefTo
                                                                     , totalCogs.ToString("0.000"));

                            string updateDetSql1 = "UPDATE StockTransferDet SET Status = 'Received', ModifiedOn = GETDATE(), ModifiedBy = '{1}' WHERE StockTransferHdrRefNo = '{0}' AND ISNULL(FullFilledQuantity,0) > 0";
                            updateDetSql1 = string.Format(updateDetSql1, stockTransferHdrRefNo, userName);

                            string updateDetSql2 = "UPDATE StockTransferDet SET Status = 'Rejected', ModifiedOn = GETDATE(), ModifiedBy = '{1}' WHERE StockTransferHdrRefNo = '{0}' AND ISNULL(FullFilledQuantity,0) = 0";
                            updateDetSql2 = string.Format(updateDetSql2, stockTransferHdrRefNo, userName);

                            qmc.Add(new QueryCommand(updateHdrSql));
                            qmc.Add(new QueryCommand(updateDetSql1));
                            qmc.Add(new QueryCommand(updateDetSql2));
                        }
                    }
                }

                DataService.ExecuteTransaction(qmc);

                isSuccess = true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                isSuccess = false;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }

        public static string StockTransferApproval(string data, string stockTransferHdrRefNo, string username, string priceLevel)
        {
            string status = "";
            StockTransferHdr sth = null;
            try
            {
                Logger.writeLog("StockTransferApproval :" + data);
                List<TransferDet> dataPODetColl = new JavaScriptSerializer().Deserialize<List<TransferDet>>(data);
                QueryCommandCollection com = new QueryCommandCollection();

                sth = new StockTransferHdr(stockTransferHdrRefNo);

                if (sth.IsNew)
                    throw new Exception("Stock Transfer not found");

                int RejectedCount = 0;

                foreach (TransferDet i in dataPODetColl)
                {
                    double GSTAmount = 0;
                    int GSTRule = 2;

                    GSTRule = GetGSTRuleDefaultWebOrdering();

                    //exclusive gst
                    double amount = (i.Quantity * i.FactoryPrice);
                    if (GSTRule == 0)
                    {
                        GSTAmount = amount * 0.07;
                    }
                    else if (GSTRule == 1)
                    {
                        // inclusive
                        GSTAmount = amount * 0.07 / 1.07;
                    }
                    else
                    {
                        // no gst
                        GSTAmount = 0;
                    }                   

                    Query qr = new Query("StockTransferDet");
                    qr.AddUpdateSetting(StockTransferDet.UserColumns.GSTRule, GSTRule);
                    qr.AddUpdateSetting(StockTransferDet.UserColumns.GSTAmount, GSTAmount);
                    qr.AddUpdateSetting(StockTransferDet.Columns.FactoryPrice, i.FactoryPrice);
                    qr.AddUpdateSetting(StockTransferDet.Columns.ModifiedBy, username);
                    qr.AddUpdateSetting(StockTransferDet.Columns.ModifiedOn, DateTime.Now);
                    qr.WHERE(StockTransferDet.Columns.StockTransferDetRefNo, i.StockTransferDetRefNo);
                    
                    com.Add(qr.BuildUpdateCommand());

                    if (i.Quantity == 0)
                        RejectedCount++;
                }

                string que = "select count(*) from stocktransferdet where StockTransferHdrRefNo = '" + stockTransferHdrRefNo + "'";
                int countDet = (int) DataService.ExecuteScalar(new QueryCommand(que));

                Query qr3 = new Query("StockTransferHdr");

                if (countDet == RejectedCount)
                    qr3.AddUpdateSetting(StockTransferHdr.Columns.Status, "Rejected");
                else
                    qr3.AddUpdateSetting(StockTransferHdr.Columns.Status, "Approved");

                qr3.AddUpdateSetting(StockTransferHdr.UserColumns.PriceLevel, priceLevel);
                qr3.AddUpdateSetting(StockTransferHdr.Columns.ModifiedBy, username);
                qr3.AddUpdateSetting(StockTransferHdr.UserColumns.ApprovedBy, username);
                qr3.AddUpdateSetting(StockTransferHdr.UserColumns.ApprovalDate, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));

                qr3.WHERE(StockTransferHdr.Columns.StockTransferHdrRefNo, stockTransferHdrRefNo);
                com.Add(qr3.BuildUpdateCommand());

                if (com.Count() > 0)
                    DataService.ExecuteTransaction(com);
            }
            catch (Exception ex) {
                status = string.Format("Error Stock Transfer Approval:" + ex.Message);
            }

            var result = new
            {
                StockTransferHdr = sth,
                status = status
            };

            return new JavaScriptSerializer().Serialize(result);
        }

        public static string StockTransferApprovalThroughWarehouse(string data, string stockTransferHdrRefNo, string username, string priceLevel, bool autoStockIn)
        {
            string status = "";
            StockTransferHdr sth = null;
            try
            {
                Logger.writeLog("StockTransferApproval :" + data);
                List<TransferDet> dataPODetColl = new JavaScriptSerializer().Deserialize<List<TransferDet>>(data);
                QueryCommandCollection com = new QueryCommandCollection();

                sth = new StockTransferHdr(stockTransferHdrRefNo);

                if (sth.IsNew)
                    throw new Exception("Stock Transfer not found");

                int RejectedCount = 0;

                foreach (TransferDet i in dataPODetColl)
                {
                    if (!string.IsNullOrEmpty(priceLevel))
                    {
                        Item item = new Item(i.ItemNo);
                        if (item != null && item.ItemNo == i.ItemNo)
                        {
                            if (priceLevel == "P1" && item.P1Price.HasValue)
                                i.FactoryPrice = (double) item.P1Price.Value;
                            else if (priceLevel == "P2" && item.P2Price.HasValue)
                                i.FactoryPrice = (double) item.P2Price.Value;
                            else if (priceLevel == "P3" && item.P3Price.HasValue)
                                i.FactoryPrice = (double) item.P3Price.Value;
                            else if (priceLevel == "P4" && item.P4Price.HasValue)
                                i.FactoryPrice = (double) item.P4Price.Value;
                            else if (priceLevel == "P5" && item.P5Price.HasValue)
                                i.FactoryPrice = (double) item.P5Price.Value;
                        }                        
                    }
                    else
                    {
                        i.FactoryPrice = 0;                        
                    }

                    double GSTAmount = 0;
                    int GSTRule = 2;
                    
                    GSTRule = GetGSTRuleDefaultWebOrdering();

                    //exclusive gst
                    double amount = (i.Quantity * i.FactoryPrice);
                    if (GSTRule == 1)
                    {
                        GSTAmount = amount * 0.07;
                    }
                    else if (GSTRule == 2)
                    {
                        // inclusive
                        GSTAmount = amount * 0.07 / 1.07;
                    }
                    else { 
                        // no gst
                        GSTAmount = 0;
                    }                   

                    Query qr = new Query("StockTransferDet");
                    qr.AddUpdateSetting(StockTransferDet.UserColumns.GSTRule, GSTRule);
                    qr.AddUpdateSetting(StockTransferDet.UserColumns.GSTAmount, GSTAmount);
                    qr.AddUpdateSetting(StockTransferDet.Columns.FactoryPrice, i.FactoryPrice);
                    qr.AddUpdateSetting(StockTransferDet.Columns.ModifiedBy, username);
                    qr.AddUpdateSetting(StockTransferDet.Columns.ModifiedOn, DateTime.Now);
                    qr.WHERE(StockTransferDet.Columns.StockTransferDetRefNo, i.StockTransferDetRefNo);

                    com.Add(qr.BuildUpdateCommand());

                    if (i.Quantity == 0)
                        RejectedCount++;
                }

                string que = "select count(*) from stocktransferdet where StockTransferHdrRefNo = '" + stockTransferHdrRefNo + "'";
                int countDet = (int)DataService.ExecuteScalar(new QueryCommand(que));

                Query qr3 = new Query("StockTransferHdr");

                bool isRejected = false;

                if (countDet == RejectedCount)
                {
                    qr3.AddUpdateSetting(StockTransferHdr.Columns.Status, "Rejected");
                    isRejected = true;
                }
                else
                    qr3.AddUpdateSetting(StockTransferHdr.Columns.Status, "Approved");

                sth.AutoStockIn = autoStockIn;

                qr3.AddUpdateSetting(StockTransferHdr.UserColumns.PriceLevel, priceLevel);
                qr3.AddUpdateSetting(StockTransferHdr.UserColumns.AutoStockIn, autoStockIn);
                qr3.AddUpdateSetting(StockTransferHdr.Columns.ModifiedBy, username);
                qr3.AddUpdateSetting(StockTransferHdr.Columns.ModifiedOn, DateTime.Now);
                qr3.AddUpdateSetting(StockTransferHdr.UserColumns.ApprovedBy, username);
                qr3.AddUpdateSetting(StockTransferHdr.UserColumns.ApprovalDate, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));

                qr3.WHERE(StockTransferHdr.Columns.StockTransferHdrRefNo, stockTransferHdrRefNo);
                com.Add(qr3.BuildUpdateCommand());

                bool isTransferToWH = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.StockTransferWillGoThroughWarehouse), false);

                decimal totalCogs = 0;
                string InventoryHdrRefFrom = "";
                string InventoryHdrRefTo = "";

                if (!isRejected)
                {
                    if (isTransferToWH)
                    {
                        QueryCommandCollection tmp1 = new QueryCommandCollection();
                        QueryCommandCollection tmp2 = new QueryCommandCollection();
                        string newInvoiceNo = "";
                        string newCreditNoteNo = "";

                        if (string.IsNullOrEmpty(sth.CreditInvoiceNo) && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.AutoGenerateInvoiceNo), false))
                        {
                            if (!GenerateInvoiceCreditNoteNo(sth.TransferFromLocationID.GetValueOrDefault(0), true, username, out tmp1, out newCreditNoteNo, out status))
                            {
                                throw new Exception(status);
                            }

                            com.AddRange(tmp1);
                            qr3.AddUpdateSetting(StockTransferHdr.UserColumns.CreditInvoiceNo, newCreditNoteNo);
                        }

                        if (string.IsNullOrEmpty(sth.InvoiceNo) && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.AutoGenerateInvoiceNo), false))
                        {
                            if (!GenerateInvoiceCreditNoteNo(sth.TransferToLocationID.GetValueOrDefault(0), false, username, out tmp2, out newInvoiceNo, out status))
                            {
                                throw new Exception(status);
                            }

                            com.AddRange(tmp2);

                            qr3.AddUpdateSetting(StockTransferHdr.UserColumns.InvoiceNo, newInvoiceNo);
                        }

                        QueryCommandCollection cmdCol = new QueryCommandCollection();

                        #region *) Transfer to warehouse first

                        PowerPOS.Container.CostingMethods CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
                        string strCostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
                        if (strCostingMethod.ToLower() == "fifo")
                            CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
                        else if (strCostingMethod.ToLower() == "fixed avg")
                            CostingMethod = PowerPOS.Container.CostingMethods.FixedAvg;
                        else
                            CostingMethod = PowerPOS.Container.CostingMethods.FIFO;

                        #region *) Return Out
                        InventoryController ctrlReturnOut = new InventoryController(CostingMethod);
                        ctrlReturnOut.SetInventoryHeaderInfo(sth.StockTransferHdrRefNo, "", "Transfer: " + sth.StockTransferHdrRefNo, 0, 0, 0);
                        #endregion

                        #region *) Return In
                        InventoryController ctrlReturnIn = new InventoryController(CostingMethod);
                        ctrlReturnIn.SetInventoryHeaderInfo(sth.StockTransferHdrRefNo, "", "Transfer: " + sth.StockTransferHdrRefNo, 0, 0, 0);
                        #endregion

                        #region *) Transfer Auto Receive
                        InventoryController invCtrlTransfer = new InventoryController();
                        invCtrlTransfer.SetInventoryHeaderInfo(sth.StockTransferHdrRefNo, "", "Transfer: " + sth.StockTransferHdrRefNo, 0, 0, 0);
                        #endregion

                        #region *) Transfer In
                        InventoryController invCtrlTransferIn = new InventoryController();
                        invCtrlTransferIn.SetInventoryHeaderInfo(sth.StockTransferHdrRefNo, "", "Transfer: " + sth.StockTransferHdrRefNo, 0, 0, 0);
                        #endregion

                        for (int i = 0; i < dataPODetColl.Count; i++)
                        {
                            string itemNo = dataPODetColl[i].ItemNo;
                            string stockTransferDetRefNo = dataPODetColl[i].StockTransferDetRefNo;
                            decimal qty = (decimal)dataPODetColl[i].Quantity;
                            decimal factoryPrice = (decimal)dataPODetColl[i].FactoryPrice;
                            decimal costOfGoods = (decimal)factoryPrice * qty;

                            if (qty > 0)
                            {
                                //return out
                                if (!ctrlReturnOut.AddItemIntoInventoryStockReturn(itemNo, qty, factoryPrice, false, out status))
                                    throw new Exception(status);

                                //return in
                                if (!ctrlReturnIn.AddItemIntoInventoryStockIn(itemNo, qty, factoryPrice, out status))
                                    throw new Exception(status);

                                //transfer auto Receive
                                if (!invCtrlTransfer.AddItemIntoInventoryStockIn(itemNo, qty, factoryPrice, out status))
                                    throw new Exception(status);

                                //transfer in
                                if (!invCtrlTransferIn.AddItemIntoInventoryStockIn(itemNo, qty, factoryPrice, out status))
                                    throw new Exception(status);

                                Query qr = new Query("StockTransferDet");
                                qr.AddUpdateSetting(StockTransferDet.Columns.FullFilledQuantity, qty);
                                qr.AddUpdateSetting(StockTransferDet.Columns.ModifiedBy, username);
                                qr.AddUpdateSetting(StockTransferDet.Columns.ModifiedOn, DateTime.Now);
                                qr.WHERE(StockTransferDet.Columns.StockTransferDetRefNo, stockTransferDetRefNo);

                                com.Add(qr.BuildUpdateCommand());

                            }
                            else {
                                Query qr = new Query("StockTransferDet");
                                qr.AddUpdateSetting(StockTransferDet.Columns.FullFilledQuantity, 0);
                                qr.AddUpdateSetting(StockTransferDet.Columns.Status, "Rejected");
                                qr.AddUpdateSetting(StockTransferDet.Columns.ModifiedBy, username);
                                qr.AddUpdateSetting(StockTransferDet.Columns.ModifiedOn, DateTime.Now);
                                qr.WHERE(StockTransferDet.Columns.StockTransferDetRefNo, stockTransferDetRefNo);

                                com.Add(qr.BuildUpdateCommand());
                            }
                        }

                        //return out
                        cmdCol.AddRange(ctrlReturnOut.CreateStockReturnQueryCommand(username, 1, sth.TransferFromLocationID.GetValueOrDefault(0), false, true));

                        cmdCol.AddRange(ctrlReturnIn.CreateReturnInQueryCommand(username, sth.ReturnToWarehouseID, false, true));

                        if (cmdCol.Count() > 0)
                            DataService.ExecuteTransaction(cmdCol);

                        if (sth.AutoStockIn)
                        {
                            if (!invCtrlTransfer.TransferOutAutoReceive(username, sth.ReturnToWarehouseID, sth.TransferToLocationID.GetValueOrDefault(0), out InventoryHdrRefFrom, out InventoryHdrRefTo, out status))
                            {
                                throw new Exception(status);
                            }

                            //after set auto receive set status = received
                            if (countDet != RejectedCount)
                                qr3.AddUpdateSetting(StockTransferHdr.Columns.Status, "Received");
                                qr3.AddUpdateSetting(StockTransferHdr.UserColumns.InventoryHdrRefNoFrom, InventoryHdrRefFrom);
                                qr3.AddUpdateSetting(StockTransferHdr.UserColumns.InventoryHdrRefNoTo, InventoryHdrRefTo);

                        }
                        else {
                            
                            if (!invCtrlTransferIn.TransferOut(username, sth.ReturnToWarehouseID, sth.TransferToLocationID.GetValueOrDefault(0), out InventoryHdrRefFrom, out status))
                            {
                                throw new Exception(status);
                            }

                            qr3.AddUpdateSetting(StockTransferHdr.UserColumns.InventoryHdrRefNoTo, InventoryHdrRefTo);
                        }


                        com.Add(qr3.BuildUpdateCommand());

                        #endregion
                    }
                }

                if (com.Count() > 0)
                    DataService.ExecuteTransaction(com);
            }
            catch (Exception ex)
            {
                status = string.Format("Error Stock Transfer Approval:" + ex.Message);
            }

            var result = new
            {
                StockTransferHdr = sth,
                status = status
            };

            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }

        public static bool ChangeCreditInvoiceNo(string stockTransferHdrRefNo, string CreditInvoiceNo, string userName, out string status)
        {
            bool isSuccess = false;
            status = "";

            try
            {
                StockTransferHdr sth = new StockTransferHdr(stockTransferHdrRefNo);
                if (sth.IsNew)
                    throw new Exception("Stock Transfer Hdr not found");

                sth.CreditInvoiceNo = CreditInvoiceNo;
                sth.Save(userName);

                isSuccess = true;
            }
            catch (Exception ex)
            {
                status = "Error : " + ex.Message;
                Logger.writeLog(ex);
                isSuccess = false;
            }

            return isSuccess;
        }

        public static bool ChangeInvoiceNo(string stockTransferHdrRefNo, string InvoiceNo, string userName, out string status)
        {
            bool isSuccess = false;
            status = "";

            try
            {
                StockTransferHdr sth = new StockTransferHdr(stockTransferHdrRefNo);
                if (sth.IsNew)
                    throw new Exception("Stock Transfer Hdr not found");

                sth.InvoiceNo = InvoiceNo;
                sth.Save(userName);

                isSuccess = true;
            }
            catch (Exception ex)
            {
                status = "Error : " + ex.Message;
                Logger.writeLog(ex);
                isSuccess = false;
            }

            return isSuccess;
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

        public static bool GenerateInvoiceCreditNoteNo(int InventoryLocationID, bool IsCreditNote, string username, out QueryCommandCollection tmp, out string newInvoiceNo, out string status)
        {
            status = "";
            newInvoiceNo = "";
            tmp = new QueryCommandCollection();
            try
            {
                int runningNumber = 0;
                string displayedName = "";
                InvoiceSequenceNoCollection col = new InvoiceSequenceNoCollection();
                string query = string.Format("select * from InvoiceSequenceNo where InventoryLocationID = {0} and ISNULL(deleted,0) = 0 and ISNULL(userflag1,0) = {1}", InventoryLocationID, IsCreditNote == true ? 1 : 0);
                col.LoadAndCloseReader(DataService.GetReader(new QueryCommand(query)));

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
                    no.IsCreditNote = IsCreditNote;
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

                string prefix = AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.AutoGenerateInvoiceNoPrefix);

                int numberLength;
                if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.AutoGenerateInvoiceLength), out numberLength))
                {
                    numberLength = 6;
                }

                runningNumber += 1;
                no.CurrentNo = runningNumber;

                newInvoiceNo = displayedName + (IsCreditNote ? "R" + prefix : prefix) + runningNumber.ToString().PadLeft(numberLength, '0');

                if (no.IsNew)
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
    }
}
