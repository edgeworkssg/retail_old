using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using SubSonic;
using PowerPOS.Container;

namespace PowerPOS
{
    public partial class PurchaseOrderHdrController
    {
        public const string XMLFILENAME = "\\WS.XML";
        public static string WS_URL;

        public static bool Load_WS_URL()
        {
            try
            {
                //if it does not exist in database, load from text file
                //this is for backward compatibility 
                if (AppSetting.GetSetting("WS_URL") != null)
                {
                    WS_URL = AppSetting.GetSetting("WS_URL").ToString();
                    return true;
                }
                else
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + XMLFILENAME);
                    WS_URL = ds.Tables[0].Rows[0]["URL"].ToString();
                    return true;
                }
            }
            catch (Exception ex) //system exception
            {
                Logger.writeLog("Load_WS_URL");
                Logger.writeLog(ex);
                return false;
            }
        }

        public const string ApprovedStatus = "Approved";
        public const string RejectedStatus = "Rejected";

        public DataTable FetchData(DateTime startDate, DateTime endDate,
            string userName, int locationID, int supplierID, 
            string remarks, string status, string orderBy, string orderDir, string PONumber)
        {

            if (string.IsNullOrEmpty(orderBy))
                orderBy = "PurchaseOrderDate";
            if (string.IsNullOrEmpty(orderDir))
                orderDir = "DESC";

            DataTable dt = new DataTable();
            string sql = "";

            try
            {
                if (status.ToLower() != "open")
                {
                    sql = @"DECLARE @StartDate DATETIME;
                        DECLARE @EndDate DATETIME;
                        DECLARE @UserName NVARCHAR(200);
                        DECLARE @InventoryLocID INT;
                        DECLARE @SupplierID INT;
                        DECLARE @Remark NVARCHAR(500);
                        DECLARE @Status NVARCHAR(200);
                        DECLARE @PONumber VARCHAR(100);
                        DECLARE @Total Decimal(20,5); 
						DECLARE @GstAmount Decimal(20,5); 

                        SET @StartDate = '{0}';
                        SET @EndDate = '{1}';
                        SET @UserName = '{2}';
                        SET @InventoryLocID = {3};
                        SET @SupplierID = {4};
                        SET @Remark = '{5}';
                        SET @Status = '{6}';
                        SET @PONumber = '{9}'; 
					
                       SELECT   
							@Total = CASE WHEN 
								ISNULL(hdr.Userfloat1,0) /*min purchase*/ > SUM(CASE WHEN isnull(det.Userfloat3,0) <= 0 THEN det.FactoryPrice * det.Quantity ELSE isnull(det.Userfloat3,0) * det.Quantity / isnull(det.Userfloat4,0)  END)
								THEN  SUM(CASE WHEN isnull(det.Userfloat3,0) <= 0 THEN det.FactoryPrice * det.Quantity ELSE det.Userfloat3 * det.Quantity / isnull(det.Userfloat4,0)  END) + isnull( hdr.Userfloat2,0) /*delivery Charge*/
								ELSE
								 SUM(CASE WHEN isnull(det.Userfloat3,0) <= 0 THEN det.FactoryPrice * det.Quantity ELSE  isnull(det.Userfloat3,0) * det.Quantity / isnull(det.Userfloat4,0)  END)
							END,
							@GstAmount = CASE WHEN isnull(hdr.Userfld10,'') = '1'  
									THEN SUM(CASE WHEN isnull(det.Userfloat3,0) <= 0 THEN det.FactoryPrice * det.Quantity ELSE  isnull(det.Userfloat3,0) * det.Quantity / isnull(det.Userfloat4,0)  END)/ 100 * 7
								WHEN isnull(hdr.Userfld10,'') = '2'
									THEN SUM(CASE WHEN isnull(det.Userfloat3,0) <= 0 THEN det.FactoryPrice * det.Quantity ELSE  isnull(det.Userfloat3,0) * det.Quantity / isnull(det.Userfloat4,0)  END)/ 100 * (7 * 1.07)
								ELSE
									0
							END
						FROM PurchaseOrderHdr Hdr
						INNER JOIN PurchaseOrderDet det
						ON Hdr.PurchaseOrderHdrRefNo =det.PurchaseOrderHdrRefNo
						WHERE (hdr.PurchaseOrderHdrRefNo LIKE '%'+@PONumber+'%' OR hdr.CustomRefNo LIKE '%'+@PONumber+'%' )
						AND ISNULL(det.userflag1, 0) = 0
						GROUP BY Hdr.PurchaseOrderHdrRefNo,hdr.Userfloat1,hdr.Userfloat2,hdr.Userfld10 

                        SELECT   ROW_NUMBER() OVER(ORDER BY {7} {8}) No
		                        ,POH.PurchaseOrderDate
		                        ,POH.PurchaseOrderHdrRefNo
		                        ,POH.UserName
		                        ,IL.InventoryLocationName
		                        ,POH.Remark
		                        ,SP.SupplierName
		                        ,CASE WHEN ISNULL(POH.Userfld7,'') = '' THEN 'Submitted' ELSE ISNULL(POH.Userfld7,'') END Status
                                ,CASE WHEN ISNULL(POH.CustomRefNo,'') = '' THEN POH.PurchaseOrderHdrRefNo ELSE POH.CustomRefNo END CustomRefNo
                                ,ISNULL(POH.Userflag5, 0) IsEmailSent
                                ,CASE WHEN isnull(POH.Userfld10,'') = '1' 
									THEN @Total + @GstAmount
								  ELSE @Total
								 END TotalAmount
                        FROM	PurchaseOrderHdr POH
		                        INNER JOIN InventoryLocation IL ON IL.InventoryLocationID = POH.InventoryLocationID
		                        INNER JOIN Supplier SP ON SP.SupplierID = POH.Supplier
                        WHERE	ISNULL(POH.Deleted,0) = 0
		                        AND CAST(POH.PurchaseOrderDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)
		                        AND (POH.UserName = @UserName OR @UserName = 'ALL')
		                        AND (POH.InventoryLocationID = @InventoryLocID OR @InventoryLocID = 0)
		                        AND (POH.Supplier = @SupplierID OR @SupplierID = 0)
		                        AND (POH.Remark LIKE '%'+@Remark+'%')
                                AND (CASE WHEN ISNULL(POH.Userfld7,'') = '' THEN 'Submitted' ELSE ISNULL(POH.Userfld7,'') END = @Status
                                     OR @Status = 'ALL')
                                AND (POH.PurchaseOrderHdrRefNo LIKE '%'+@PONumber+'%' OR POH.CustomRefNo LIKE '%'+@PONumber+'%')
                        ORDER BY {7} {8}";
                    sql = string.Format(sql, startDate.ToString("yyyy-MM-dd")
                                           , endDate.ToString("yyyy-MM-dd")
                                           , userName
                                           , locationID
                                           , supplierID
                                           , remarks
                                           , status
                                           , orderBy
                                           , orderDir
                                           , PONumber);
                    dt.Load(DataService.GetReader(new QueryCommand(sql)));
                }
                else
                {
                    sql = @"DECLARE @StartDate DATETIME;
                        DECLARE @EndDate DATETIME;
                        DECLARE @UserName NVARCHAR(200);
                        DECLARE @InventoryLocID INT;
                        DECLARE @SupplierID INT;
                        DECLARE @Remark NVARCHAR(500);
                        DECLARE @Status NVARCHAR(200);

                        SET @StartDate = '{0}';
                        SET @EndDate = '{1}';
                        SET @UserName = '{2}';
                        SET @InventoryLocID = {3};
                        SET @SupplierID = {4};
                        SET @Remark = '{5}';
                        SET @Status = '{6}';

                        SELECT   ROW_NUMBER() OVER(ORDER BY {7} {8}) No
		                        ,POH.PurchaseOrderDate
		                        ,POH.PurchaseOrderHdrRefNo
		                        ,POH.UserName
		                        ,IL.InventoryLocationName
		                        ,POH.Remark
		                        ,SP.SupplierName
		                        ,CASE WHEN ISNULL(POH.Userfld7,'') = '' THEN 'Submitted' ELSE ISNULL(POH.Userfld7,'') END Status
		                        ,CASE WHEN ISNULL(POH.CustomRefNo,'') = '' THEN POH.PurchaseOrderHdrRefNo ELSE POH.CustomRefNo END CustomRefNo
                                ,(
                                    SELECT SUM(
                                        CASE 
                                            WHEN userfloat3 <= 0 THEN FactoryPrice 
                                            WHEN userfloat3 > 0 THEN userfloat3 * userfloat5 / userfloat4 
                                        END
                                    ) FROM PurchaseOrderDet WHERE PurchaseOrderHdrRefNo = POH.PurchaseOrderHdrRefNo) as SubTotal
                        FROM	PurchaseOrderHdr POH
		                        INNER JOIN InventoryLocation IL ON IL.InventoryLocationID = POH.InventoryLocationID
		                        INNER JOIN Supplier SP ON SP.SupplierID = POH.Supplier
                        WHERE	ISNULL(POH.Deleted,0) = 0
		                        AND CAST(POH.PurchaseOrderDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)
		                        AND (POH.UserName = @UserName OR @UserName = 'ALL')
		                        AND (POH.InventoryLocationID = @InventoryLocID OR @InventoryLocID = 0)
		                        AND (POH.Supplier = @SupplierID OR @SupplierID = 0)
		                        AND (POH.Remark LIKE '%'+@Remark+'%')
                                AND (CASE WHEN ISNULL(POH.Userfld7,'') = '' THEN 'Submitted' ELSE ISNULL(POH.Userfld7,'') END = @Status
                                    )
                                AND POH.PurchaseOrderHdrRefNo not in (select PurchaseOrderNo from InventoryHdr where movementtype like 'Stock In') 
                        ORDER BY {7} {8}";
                    sql = string.Format(sql, startDate.ToString("yyyy-MM-dd")
                                           , endDate.ToString("yyyy-MM-dd")
                                           , userName
                                           , locationID
                                           , supplierID
                                           , remarks
                                           , status
                                           , orderBy
                                           , orderDir);
                    dt.Load(DataService.GetReader(new QueryCommand(sql)));
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return dt;
        }

        public bool ApprovePO(string refNo, out string status)
        {
            bool isSuccess = false;
            status = "";

            try
            {
                QueryCommandCollection qmc = new QueryCommandCollection();

                PurchaseOrderHdr ph = new PurchaseOrderHdr(PurchaseOrderHdr.Columns.PurchaseOrderHdrRefNo, refNo);
                if (ph.IsNew)
                    throw new Exception("Purchase Order Ref No Didn't Exist");
                ph.Userfld7 = ApprovedStatus;
                qmc.Add(ph.GetUpdateCommand(UserInfo.username));

                //if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.IsSellingPriceEditable), false))
                //{
                //    var poDets = ph.PurchaseOrderDetRecords();
                //    foreach (var pd in poDets)
                //    {
                //        Item theItem = new Item(pd.ItemNo);
                //        theItem.RetailPrice = pd.RetailPrice;
                //        qmc.Add(theItem.GetUpdateCommand(UserInfo.username));
                //    }
                //}

                DataService.ExecuteTransaction(qmc);

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

        public bool RejectPO(string refNo, out string status)
        {
            bool isSuccess = false;
            status = "";

            try
            {
                PurchaseOrderHdr ph = new PurchaseOrderHdr(PurchaseOrderHdr.Columns.PurchaseOrderHdrRefNo, refNo);
                if (ph.IsNew)
                    throw new Exception("Purchase Order Ref No Didn't Exist");
                ph.Userfld7 = RejectedStatus;

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.AutoApproveOrder), false) &&
                    AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.AutoApproveOrder), false))
                {
                    if (!string.IsNullOrEmpty(ph.GoodsOrderRefNo))
                    {
                        Query poh = new Query("PurchaseOrderHeader");
                        poh.AddUpdateSetting(PurchaseOrderHeader.UserColumns.Status, "Pending");
                        poh.AddUpdateSetting(PurchaseOrderHeader.Columns.ModifiedOn, DateTime.Now);
                        poh.AddWhere(PurchaseOrderHeader.Columns.PurchaseOrderHeaderRefNo, ph.GoodsOrderRefNo);

                        DataService.ExecuteQuery(poh.BuildUpdateCommand());
                    }
                }
 

                ph.Save(UserInfo.username);
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

        public static bool UpdateCostPriceHelper(string itemNo, string supplierID, decimal costPrice)
        {
            bool isSuccess = false;

            try
            {
                var qr = new Query("ItemSupplierMap");
                qr.AddWhere(ItemSupplierMap.Columns.ItemNo, Comparison.Equals, itemNo);
                qr.AddWhere(ItemSupplierMap.Columns.SupplierID, Comparison.Equals, supplierID);
                var data = new ItemSupplierMapController().FetchByQuery(qr).FirstOrDefault();
                if (data != null)
                {
                    data.CostPrice = costPrice;
                    data.Save(UserInfo.username);
                    isSuccess = true;
                }
                else
                {
                    throw new Exception("Item supplier mapping not found");
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }

        public static bool UpdateRetailPriceHelper(string itemNo, decimal retailPrice)
        {
            bool isSuccess = false;
            try
            {
                var theItem = new Item(Item.Columns.ItemNo, itemNo);
                theItem.RetailPrice = retailPrice;
                theItem.Save(UserInfo.username);
                isSuccess = true;
            }
            catch (Exception ex)
            {
                isSuccess = false;
                Logger.writeLog(ex);
            }
            return isSuccess;
        }

        public static bool UpdateCostPrice(string itemNo, string supplierID, decimal costPrice)
        {
            bool isSuccess = false;

            try
            {
                isSuccess = UpdateCostPriceHelper(itemNo, supplierID, costPrice);
                Load_WS_URL();
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = WS_URL;
                //isSuccess = ws.PO_UpdateCostPrice(itemNo, supplierID, costPrice);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return isSuccess;
        }

        public static bool UpdateRetailPrice(string itemNo, decimal costPrice)
        {
            bool isSuccess = false;

            try
            {
                isSuccess = UpdateRetailPriceHelper(itemNo, costPrice);
                Load_WS_URL();
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = WS_URL;
                //isSuccess = ws.PO_UpdateRetailPrice(itemNo, costPrice);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return isSuccess;
        }
    }
}
