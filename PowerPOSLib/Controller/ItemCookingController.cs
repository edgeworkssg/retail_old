using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic;
using System.Data;
using PowerPOS.Container;

namespace PowerPOS
{
    public class ItemCookingController
    {
//        public static DataTable FetchAvailableCookItems()
//        {
//            DataTable dt = new DataTable();

//            try
//            {
//                string sql = @"SELECT   I.*
//                            FROM	Item I
//		                            INNER JOIN Category CTG ON CTG.CategoryName = I.CategoryName
//		                            INNER JOIN RecipeHeader RH ON RH.ItemNo = I.ItemNo
//                            WHERE	ISNULL(I.Deleted,0) = 0
//		                            AND ISNULL(I.IsInInventory,0) = 1
//		                            AND ISNULL(RH.Deleted,0) = 0
//                            ORDER BY I.CategoryName, I.ItemName";
//                sql = string.Format(sql);
//                dt.Load(DataService.GetReader(new QueryCommand(sql)));
//            }
//            catch (Exception ex)
//            {
//                Logger.writeLog(ex);
//            }

//            return dt;
//        }

//        public static bool CookItem(string itemNo, decimal quantity, string remarks, out string status)
//        {
//            bool isSuccess = false;
//            status = "";

//            try
//            {
//                string XMLFILENAME = "\\WS.XML";
//                string WS_URL = "http://localhost:51831/PowerPOSWeb/Synchronization/Synchronization.asmx";
//                DataSet dsURL = new DataSet();
//                dsURL.ReadXml(AppDomain.CurrentDomain.BaseDirectory + XMLFILENAME);
//                WS_URL = dsURL.Tables[0].Rows[0]["URL"].ToString();
//                if (!WS_URL.ToLower().StartsWith("http://localhost:"))
//                {
//                    PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
//                    ws.Url = WS_URL;
//                    isSuccess = ws.CookItem(itemNo, quantity, PointOfSaleInfo.PointOfSaleID,PointOfSaleInfo.InventoryLocationID, remarks, UserInfo.username, out status);
//                }
//                else
//                    isSuccess = CookItemHelper(itemNo, quantity, PointOfSaleInfo.PointOfSaleID, PointOfSaleInfo.InventoryLocationID, remarks, UserInfo.username, out status);
//            }
//            catch (Exception ex)
//            {
//                Logger.writeLog(ex);
//            }

//            return isSuccess;
//        }

        public static bool CookItemHelper(string itemNo, decimal quantity, int pointOfSaleID, int inventoryLocationID, string remarks, string userName, out string status)
        {
            bool isSuccess = false;
            status = "";

            try
            {
                Item mainItem = new Item(itemNo);
                if (mainItem.IsNew)
                    throw new Exception("Item Not found");
                
                if (quantity <= 0)
                    throw new Exception("Quantity must greater than zero");

                InventoryLocation il = new InventoryLocation(inventoryLocationID);
                if(il.IsNew)
                    throw new Exception("Inventory location  not found");

                ItemCookHistory ich = new ItemCookHistory();
                ich.ItemNo = itemNo;
                ich.Quantity = quantity;
                ich.CookDate = DateTime.Now;
                if (pointOfSaleID != 0)
                    ich.PointOfSaleID = pointOfSaleID;
                else
                    ich.PointOfSaleID = null;

                ich.InventoryLocationID = inventoryLocationID;
                ich.UniqueID = Guid.NewGuid();
                ich.Deleted = false;
                decimal totalCOGS = 0;
                QueryCommandCollection qmc = new QueryCommandCollection();
                qmc.Add(ich.GetInsertCommand(userName));

                #region *) Iterate Troughout recipe detail and do stock out
                bool isFetchRecipeSuccess = false;
                var recipeData = RecipeController.GetRecipeDeductionList(itemNo, quantity, inventoryLocationID, false, false, out isFetchRecipeSuccess, out status);
                if (!isFetchRecipeSuccess)
                    throw new Exception(status);
                InventoryController itCtr = new InventoryController();
                itCtr.SetInventoryDate(ich.CookDate.GetValueOrDefault(DateTime.Now));
                foreach (var rd in recipeData)
                    itCtr.AddItemIntoInventory(ich.UniqueID.ToString(), rd.ItemNo, rd.Quantity, out status);
                if (itCtr.InvDet != null && itCtr.InvDet.Count > 0)
                {
                    var stockOutCmd = itCtr.StockOutCommandServer(userName, 0, inventoryLocationID, pointOfSaleID, 0, out status, out totalCOGS);
                    qmc.AddRange(stockOutCmd);
                }

                if (mainItem.IsInInventory)
                {
                    InventoryController itCtrIN = new InventoryController();
                    itCtrIN.SetInventoryDate(ich.CookDate.GetValueOrDefault(DateTime.Now));
                    itCtrIN.AddItemIntoInventory(ich.UniqueID.ToString(), mainItem.ItemNo, quantity, out status);
                    itCtrIN.ChangeFactoryPriceByItemNo(mainItem.ItemNo, totalCOGS/quantity, out status);
                    var stockInCmd = itCtrIN.StockInCommandServer(userName, inventoryLocationID, pointOfSaleID, false, 1, out status);
                    qmc.AddRange(stockInCmd);
                }

                DataService.ExecuteTransaction(qmc);
                isSuccess = true;
                status = "";

                #endregion
            }
            catch (Exception ex)
            {
                isSuccess = false;
                status = ex.Message;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }

        public static bool CookItemWithDetail(int ItemCookHistoryID, int pointOfSaleID, string userName, out string status)
        {
            bool isSuccess = false;
            status = "";

            try
            {
                ItemCookHistory ich = new ItemCookHistory(ItemCookHistoryID);

                if (ich.IsNew)
                    throw new Exception("Item Cook History not found");

                Item mainItem = new Item(ich.ItemNo);
                if (mainItem.IsNew)
                    throw new Exception("Item doesn't exist");

                Query qr = ItemCookDetail.CreateQuery();
                qr.AddWhere(ItemCookDetail.Columns.ItemCookHistoryID, ich.ItemCookHistoryID);
                qr.AddWhere(ItemCookDetail.Columns.Deleted, false);

                ItemCookDetailCollection icdCol = new ItemCookDetailCollection();
                icdCol.LoadAndCloseReader(DataService.GetReader(qr.BuildCommand()));

                QueryCommandCollection qmc = new QueryCommandCollection();

                decimal totalCOGS = 0;

                #region *) Iterate Troughout recipe detail and do stock out
                if (icdCol.Count > 0)
                {
                    InventoryController itCtr = new InventoryController();
                    itCtr.SetInventoryDate(ich.CookDate.GetValueOrDefault(DateTime.Now));

                    foreach (ItemCookDetail icd in icdCol)
                    {
                        bool isFetchRecipeSuccess = false;
                        var recipeData = RecipeController.GetRecipeDeductionList(icd.ItemNo, icd.Qty.GetValueOrDefault(0), ich.InventoryLocationID.GetValueOrDefault(0), true, false, out isFetchRecipeSuccess, out status);
                        if (!isFetchRecipeSuccess)
                            throw new Exception(status);
                        
                        foreach (var rd in recipeData)
                            itCtr.AddItemIntoInventory(ich.UniqueID.ToString(), rd.ItemNo, rd.Quantity, out status);
                    }

                    if (itCtr.InvDet != null && itCtr.InvDet.Count > 0)
                    {
                        var stockOutCmd = itCtr.StockOutCommandServer(userName, 0, ich.InventoryLocationID.GetValueOrDefault(0), pointOfSaleID, 0, out status, out totalCOGS);
                        qmc.AddRange(stockOutCmd);
                    }
                }

                if (mainItem.IsInInventory)
                {
                    InventoryController itCtrIN = new InventoryController();
                    itCtrIN.SetInventoryDate(ich.CookDate.GetValueOrDefault(DateTime.Now));
                    itCtrIN.AddItemIntoInventory(ich.UniqueID.ToString(), mainItem.ItemNo, ich.Quantity.GetValueOrDefault(0), out status);
                    itCtrIN.ChangeFactoryPriceByItemNo(mainItem.ItemNo, ich.COG, out status);
                    var stockInCmd = itCtrIN.StockInCommandServer(userName, ich.InventoryLocationID.GetValueOrDefault(0), pointOfSaleID, false, 1, out status);
                    qmc.AddRange(stockInCmd);

                    //ich.COG = ich.Quantity.GetValueOrDefault(0) == 0 ? 0 :totalCOGS / ich.Quantity.GetValueOrDefault(0);
                    ich.PointOfSaleID = pointOfSaleID;
                    ich.Status = "Completed";
                    qmc.Add(ich.GetSaveCommand(userName));
                }

                DataService.ExecuteTransaction(qmc);
                isSuccess = true;
                status = "";

                #endregion
            }
            catch (Exception ex)
            {
                isSuccess = false;
                status = ex.Message;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }

        public static string getNewDocumentNo(string InventoryLocationName)
        {
            int runningNo = 0;
            string PurchaseOrderRefNo;
            string header = "PO";

            header += InventoryLocationName.Left(3) + DateTime.Now.ToString("yyMMdd");

            Query qr = ItemCookHistory.CreateQuery();
            qr.AddWhere(ItemCookHistory.UserColumns.DocumentNo, Comparison.Like, header + "%");
            qr.QueryType = QueryType.Select;
            qr.Top = "1";
            qr.SetSelectList("Userfld1");
            qr.OrderBy = OrderBy.Desc("Userfld1");

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
            PurchaseOrderRefNo = header + runningNo.ToString().PadLeft(5, '0');

            return PurchaseOrderRefNo;
        }
    }
}
