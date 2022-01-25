using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SubSonic;

namespace PowerPOS
{
    public partial class ItemSupplierMapController
    {
        /// <summary>
        /// Get a table for item quantity trigger
        /// </summary>
        public static DataTable FetchData(string _itemId)
        {
            string SQLString;
            SQLString =@"SELECT  ISM.ItemSupplierMapID
		                        ,I.ItemNo
		                        ,I.ItemName
		                        ,ISM.CostPrice
		                        ,ISM.Currency
		                        ,CASE WHEN ISM.GSTRule = '1' THEN 'Exclusive GST'
			                          WHEN ISM.GSTRule = '2' THEN 'Inclusive GST'
			                          WHEN ISM.GSTRule = '3' THEN 'Non GST'
			                          ELSE '' END GSTRule
		                        ,SP.SupplierID
		                        ,SP.SupplierName
                        FROM	ItemSupplierMap ISM
		                        INNER JOIN Item I ON I.ItemNo = ISM.ItemNo
		                        INNER JOIN Supplier SP ON SP.SupplierID = ISM.SupplierID
                        WHERE	I.ItemName LIKE '%{0}%'";
            SQLString = string.Format(SQLString, _itemId);
            DataTable DT = new DataTable();
            QueryCommand Cmd = new QueryCommand(SQLString);
            DT.Load(DataService.GetReader(Cmd));

            return DT;
        }


        public static DataTable FetchData(string itemNo, string supplierID)
        {
            string SQLString;
            SQLString = @"SELECT  ISM.ItemSupplierMapID
		                        ,I.ItemNo
		                        ,I.ItemName
		                        ,ISM.CostPrice
		                        ,ISM.Currency
		                        ,CASE WHEN ISM.GSTRule = '1' THEN 'Exclusive GST'
			                          WHEN ISM.GSTRule = '2' THEN 'Inclusive GST'
			                          WHEN ISM.GSTRule = '3' THEN 'Non GST'
			                          ELSE 'Non GST' END GSTRule
		                        ,SP.SupplierID
		                        ,SP.userfld3 AS SupplierCode
		                        ,SP.SupplierName
                        FROM	ItemSupplierMap ISM
		                        INNER JOIN Item I ON I.ItemNo = ISM.ItemNo
		                        INNER JOIN Supplier SP ON SP.SupplierID = ISM.SupplierID
                        WHERE	ISNULL(ISM.Deleted,0) = 0
                                AND (ISNULL(I.ItemNo,'') LIKE '%{0}%'
                                     OR ISNULL(I.ItemName,'') LIKE '%{0}%')
                                AND (SP.SupplierID = '{1}' OR '{1}' = '0')";
            SQLString = string.Format(SQLString, itemNo, supplierID);
            DataTable DT = new DataTable();
            QueryCommand Cmd = new QueryCommand(SQLString);
            DT.Load(DataService.GetReader(Cmd));

            return DT;
        }

        public static DataTable GetSupplierListByItemNo(string ItemNo)
        {
            DataTable dt;
            string sql = @"
                            SELECT sup.* 
                            FROM ItemSupplierMap ism
                                INNER JOIN Supplier sup ON sup.SupplierID = ism.SupplierID
                            WHERE ism.ItemNo = @ItemNo AND ism.Deleted = 0
                         ";
            QueryCommand cmd = new QueryCommand(sql, "PowerPOS");
            cmd.Parameters.Add("@ItemNo", ItemNo, DbType.String);
            dt = DataService.GetDataSet(cmd).Tables[0];
            return dt;
        }

        public static DataTable FetchSupplierList()
        {
            DataTable dt;
            string sql = @"
                            SELECT sup.SupplierID, sup.SupplierName 
                            FROM Supplier sup 
                            WHERE ISNULL(sup.Deleted, 0) = 0 
                            ORDER BY sup.SupplierName 
                         ";
            QueryCommand cmd = new QueryCommand(sql, "PowerPOS");
            //cmd.Parameters.Add("@ItemNo", ItemNo, DbType.String);
            dt = DataService.GetDataSet(cmd).Tables[0];
            DataRow emptyRow = dt.NewRow();
            emptyRow[0] = -1;
            emptyRow[1] = "";
            dt.Rows.InsertAt(emptyRow,0);
            return dt;
        }

        public static bool SetPreferredSupplier(int supplierID, string itemNo, string username)
        {
            try
            {
                string sql = @"
                            UPDATE ItemSupplierMap
                            SET IsPreferredSupplier = 1,
                                ModifiedOn = GETDATE(),
                                ModifiedBy = '{2}'
                            WHERE SupplierID = {0} AND ItemNo = '{1}'

                            UPDATE ItemSupplierMap
                            SET IsPreferredSupplier = 0,
                                ModifiedOn = GETDATE(),
                                ModifiedBy = '{2}'
                            WHERE SupplierID <> {0} AND ItemNo = '{1}'
                          ";
                sql = string.Format(sql, supplierID, itemNo, username);
                DataService.ExecuteQuery(new QueryCommand(sql, "PowerPOS"));
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }

        public static int? GetPreferredSupplier(string itemNo)
        {
            try
            {
                string sql = @"SELECT TOP 1 SupplierID FROM ItemSupplierMap WHERE ISNULL(Deleted, 0) = 0 AND ItemNo = '{0}' ORDER BY ISNULL(IsPreferredSupplier, 0) DESC";
                sql = string.Format(sql, itemNo);
                object res = DataService.ExecuteScalar(new QueryCommand(sql, "PowerPOS"));
                if (res != null && res is int)
                    return (int)res;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        public static DataTable FetchItemSupplier(string criteria, string searchReq, string numOfDays, string supplierName, string outletName,
              int PointOfSaleID, string invLocation)
        {
            try
            {
                string searchText;
                if (criteria.ToLower() == "contains")
                    searchText = "%" + searchReq + "%";
                else if (criteria.ToLower() == "starts with")
                    searchText = searchReq + "%";
                else if (criteria.ToLower() == "ends with")
                    searchText = "%" + searchReq;
                else if (criteria.ToLower() == "exact match")
                    searchText = searchReq;
                else
                    searchText = "%" + searchReq + "%";

                
                if (string.IsNullOrEmpty(numOfDays)) numOfDays = "10";
                

                string sql = @"
                                DECLARE @days int, @PointOfSaleID int, @InventoryLocationID int, 
                                        @StartDate1 datetime, @StartDate2 datetime, @StartDate3 datetime,
                                        @EndDate1 datetime, @EndDate2 datetime, @EndDate3 datetime

                                SET @days = {2} 
                                SET @PointOfSaleID = {3} 
                                SET @InventoryLocationID = {4} 

                                SET @StartDate1 = DATEADD(day, -@days * 1, CONVERT(date, GETDATE()))
                                SET @StartDate2 = DATEADD(day, -@days * 2, CONVERT(date, GETDATE()))
                                SET @StartDate3 = DATEADD(day, -@days * 3, CONVERT(date, GETDATE()))
                                SET @EndDate1 = DATEADD(s, -1, @StartDate1 + @days)
                                SET @EndDate2 = DATEADD(s, -1, @StartDate2 + @days)
                                SET @EndDate3 = DATEADD(s, -1, @StartDate3 + @days)

                                SELECT vis.*, 
                                    ISNULL(sales1.Quantity, 0) AS Sales1, 
                                    ISNULL(sales2.Quantity, 0) AS Sales2, 
                                    ISNULL(sales3.Quantity, 0) AS Sales3, 
                                    0 AS OrderQty, 
                                    ISNULL(its.BalanceQty, 0) AS OnHandQty 
                                FROM ViewItemSupplier vis
                                    LEFT JOIN (
                                            SELECT CASE WHEN ISNULL(it.userflag7,0) = 1 THEN it.userfld8 ELSE od.ItemNo END as ItemNo, 
				                                SUM(CASE WHEN ISNULL(it.userflag7,0) = 1 THEN
						                                (
							                                CASE WHEN ISNULL(it.userflag8,0) = 1 then od.Quantity / it.Userfloat5
							                                ELSE od.Quantity * it.userfloat5 END
						                                )
						                                ELSE od.Quantity 
						                                END) Quantity
                                            FROM OrderHdr oh
                                                INNER JOIN OrderDet od ON od.OrderHdrID = oh.OrderHdrID
				                                INNER JOIN Item it ON it.ItemNo = od.ItemNo AND ISNULL(it.Deleted,0) = 0
                                            WHERE oh.IsVoided = 0 AND od.IsVoided = 0
                                                AND oh.PointOfSaleID = @PointOfSaleID
                                                AND oh.OrderDate BETWEEN @StartDate1 AND @EndDate1
                                            GROUP BY CASE WHEN ISNULL(it.userflag7,0) = 1 then it.userfld8 else od.ItemNo END
                                        ) sales1 ON sales1.ItemNo = vis.ItemNo
                                    LEFT JOIN (
                                            SELECT CASE WHEN ISNULL(it.userflag7,0) = 1 THEN it.userfld8 ELSE od.ItemNo END as ItemNo, 
				                                SUM(CASE WHEN ISNULL(it.userflag7,0) = 1 THEN
						                                (
							                                CASE WHEN ISNULL(it.userflag8,0) = 1 then od.Quantity / it.Userfloat5
							                                ELSE od.Quantity * it.userfloat5 END
						                                )
						                                ELSE od.Quantity 
						                                END) Quantity
                                            FROM OrderHdr oh
                                                INNER JOIN OrderDet od ON od.OrderHdrID = oh.OrderHdrID
				                                INNER JOIN Item it ON it.ItemNo = od.ItemNo AND ISNULL(it.Deleted,0) = 0
                                            WHERE oh.IsVoided = 0 AND od.IsVoided = 0
                                                AND oh.PointOfSaleID = @PointOfSaleID
                                                AND oh.OrderDate BETWEEN @StartDate2 AND @EndDate2
                                            GROUP BY CASE WHEN ISNULL(it.userflag7,0) = 1 then it.userfld8 else od.ItemNo END
                                        ) sales2 ON sales2.ItemNo = vis.ItemNo
                                    LEFT JOIN (
                                            SELECT CASE WHEN ISNULL(it.userflag7,0) = 1 THEN it.userfld8 ELSE od.ItemNo END as ItemNo, 
				                                SUM(CASE WHEN ISNULL(it.userflag7,0) = 1 THEN
						                                (
							                                CASE WHEN ISNULL(it.userflag8,0) = 1 then od.Quantity / it.Userfloat5
							                                ELSE od.Quantity * it.userfloat5 END
						                                )
						                                ELSE od.Quantity 
						                                END) Quantity
                                            FROM OrderHdr oh
                                                INNER JOIN OrderDet od ON od.OrderHdrID = oh.OrderHdrID
				                                INNER JOIN Item it ON it.ItemNo = od.ItemNo AND ISNULL(it.Deleted,0) = 0
                                            WHERE oh.IsVoided = 0 AND od.IsVoided = 0
                                                AND oh.PointOfSaleID = @PointOfSaleID
                                                AND oh.OrderDate BETWEEN @StartDate3 AND @EndDate3
                                            GROUP BY CASE WHEN ISNULL(it.userflag7,0) = 1 then it.userfld8 else od.ItemNo END
                                        ) sales3 ON sales3.ItemNo = vis.ItemNo
                                    LEFT JOIN ItemSummary its ON its.ItemNo = vis.ItemNo AND its.InventoryLocationID = @InventoryLocationID 
                                WHERE (vis.ItemNo LIKE N'{0}' OR vis.ItemName LIKE N'{0}' OR vis.CategoryName LIKE N'{0}' OR vis.UOM LIKE N'{0}' 
                                        OR vis.Attributes1 LIKE N'{0}' OR vis.Attributes2 LIKE N'{0}' OR vis.Attributes3 LIKE N'{0}' OR vis.Attributes4 LIKE N'{0}' 
                                        OR vis.Attributes5 LIKE N'{0}' OR vis.Attributes6 LIKE N'{0}' OR vis.Attributes7 LIKE N'{0}' OR vis.Attributes8 LIKE N'{0}') 
                                    AND vis.SupplierName = '{1}'
                                ORDER BY vis.ItemNo";
                sql = string.Format(sql, searchText, supplierName, numOfDays, PointOfSaleID, invLocation);
                DataTable dt = DataService.GetDataSet(new QueryCommand(sql, "PowerPOS")).Tables[0];
                dt.PrimaryKey = new DataColumn[] { dt.Columns["ItemNo"] };

                return dt;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        public static DataTable FetchItemSupplierMapNew(string ItemNo, int SupplierID, out string status)
        {
            status = "";
            DataTable dt = new DataTable();
            try
            {
                string query = @"
                    declare @MainItemNo varchar(255)

                    select @MainitemNo = CASE WHEN ISNULL(userflag7,0) = 1 then userfld8 else ItemNo END from item
	                    where itemNo ='{0}'

                    select *
                    FROM(
                    select 1 as OrderItem, i.itemno, i.ItemName, i.RetailPrice, i.FactoryPrice, i.Userfld1 as UOM, i.Userfld8 as DeductedItemNo, 1 as DeductConvRate,
	                    0 as DeductConvType, ISNULL(im.CostPrice,0) as CostPrice,ISNULL(im.PackingSizeUOM10,0) as MOQ,
	                    im.SupplierID, i.itemno as MainItem
                    from item i
                    left join itemsuppliermap im on i.itemno = im.itemno and im.deleted = 0 and im.SupplierID = {1}
                    where i.Deleted = 0 and ISNULL(i.userflag7,0) = 0
                    UNION
                    select 2 as OrderItem, i.itemno, i.ItemName, i.RetailPrice, i.FactoryPrice, i.Userfld1 as UOM, i.Userfld8 as DeductedItemNo, i.Userfloat5 as DeductConvRate,
	                    ISNULL(i.Userflag8, 0) as DeductConvType, ISNULL(im.CostPrice,0) as CostPrice,ISNULL(im.PackingSizeUOM10,0) as MOQ,
	                    im.SupplierID, i.userfld8 as MainItem
                    from item i
                    left join itemsuppliermap im on i.itemno = im.itemno and im.deleted = 0 and im.SupplierID = {1}
                    where i.Deleted = 0 and ISNULL(i.userflag7,0) = 1
                    ) ab 
                    where MainItem = @MainitemNo
                    order by OrderItem, DeductConvRate
                ";
                query = string.Format(query, ItemNo, SupplierID.ToString());

                dt = DataService.GetDataSet(new QueryCommand(query)).Tables[0];
            }
            catch (Exception ex)
            {
                Logger.writeLog("ERROR get Mapping Item Supplier: " + ex.Message);
                status = "ERROR get Mapping Item Supplier: " + ex.Message;
            }

            return dt;
        }

        public static string FetchMainItemNonInvProduct(string ItemNo, out string status)
        {
            status = "";
            try
            {
                Item it = new Item(ItemNo);

                if (it.IsNew)
                    throw new Exception("Item doesn't exist");

                if (it.NonInventoryProduct)
                    return it.DeductedItem;
                else
                    return it.ItemNo;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error get Main ItemNo" + ex.Message);
                status = "Error get Main ItemNo" + ex.Message;

                return "";
            }
        }

        public static bool IsUnderMinQtyOrder(string ItemNo, string SupplierName, decimal qty, int InventoryLocationID, out decimal minorderqty)
        {
            int supplierID = 0;
            IDataReader reader = null;
            reader = Supplier.FetchByParameter("SupplierName", SupplierName);
            if (reader.Read())
            {
                supplierID = reader["SupplierID"].ToString().GetIntValue();
            }

            ItemSupplierMapCollection itm = new ItemSupplierMapCollection();
            itm.Where(ItemSupplierMap.Columns.ItemNo, ItemNo);
            itm.Where(ItemSupplierMap.Columns.SupplierID, supplierID);
            itm.Where(ItemSupplierMap.Columns.Deleted, false);
            itm.Load();

            minorderqty = 0;

            if (itm.Count() == 0)
            {
                ItemQuantityTriggerCollection col = new ItemQuantityTriggerCollection();
                col.Where(ItemQuantityTrigger.Columns.ItemNo, ItemNo);
                col.Where(ItemQuantityTrigger.Columns.InventoryLocationID, InventoryLocationID);
                col.Where(ItemQuantityTrigger.Columns.Deleted, false);
                col.Load();

                if (col.Count == 0)
                    return false;
                else {
                    if (qty < col[0].TriggerQuantity)
                        return true;
                    else
                        return false;
                }
            }
            else
            {
                if (itm[0].PackingSizeUOM10 > qty)
                {
                    minorderqty = itm[0].PackingSizeUOM10.GetValueOrDefault(0);
                    return true;
                }
                else
                    return false;
            }

            

        }
    }
}
