using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using SubSonic;
using System.Data;
using System.Collections;
using PowerPOS.Container;
using System.IO;
using System.Transactions;
using System.Drawing;
using System.Drawing.Imaging;

namespace PowerPOS
{
    public partial class ItemController
    {
        public static string FIXEDCOG_PERCENTAGE = "MARGIN_PERCENTAGE";
        public static string FIXEDCOG_VALUE = "VALUE";

        public static bool SetOutletPrice(string itemNo, bool isToProductMaster, string userName)
        {
            bool isSuccess = false;

            try
            {
                QueryCommandCollection qmc = new QueryCommandCollection();
                Query qr = new Query("OutletGroupItemMap");
                qr.AddWhere(OutletGroupItemMap.Columns.ItemNo, itemNo);
                OutletGroupItemMapCollection ogimColl = new OutletGroupItemMapController().FetchByQuery(qr);
                for (int i = 0; i < ogimColl.Count; i++)
                {
                    ogimColl[i].Deleted = isToProductMaster;
                    qmc.Add(ogimColl[i].GetUpdateCommand(userName));
                }
                Item theItem = new Item(itemNo);
                qmc.Add(theItem.GetUpdateCommand(userName));
                DataService.ExecuteTransaction(qmc);
                isSuccess = true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                isSuccess = false;
            }

            return isSuccess;
        }

        public static decimal FetchCostPrice(int CurrencyID, string ItemNo)
        {
            //pull out..
            Query qr = ItemCostPrice.CreateQuery();
            qr.QueryType = QueryType.Select;
            qr.Top = "1";
            qr.SelectList = "CostPrice";
            Object obj = qr.WHERE(ItemCostPrice.Columns.ItemNo, ItemNo).
                AND(ItemCostPrice.Columns.CurrencyId, CurrencyID).
                AND(ItemCostPrice.Columns.Deleted, 0).
                ORDER_BY(ItemCostPrice.Columns.CostPrice, "desc").ExecuteScalar();
            if (obj == null)
            {
                qr = Item.CreateQuery();
                qr.SelectList = "FactoryPrice";
                qr.QueryType = QueryType.Select;
                qr.Top = "1";
                obj = qr.WHERE(Item.Columns.ItemNo, ItemNo).AND(Item.Columns.Deleted, false).ExecuteScalar();
                if (obj != null) return (decimal)obj;
            }
            else
            {
                return (decimal)obj;
            }
            return 0;
        }

        public ViewItemCollection SearchItem(object name, bool IsInventoryItemOnly, bool ShowSystemItem)
        {
            return SearchItem(name, IsInventoryItemOnly, ShowSystemItem, false);
        }

        public ViewItemCollection SearchItem(object name, bool IsInventoryItemOnly, bool ShowSystemItem, bool onlySearchItemNoItemName)
        {
            return SearchItem(name, IsInventoryItemOnly, ShowSystemItem, onlySearchItemNoItemName, "");
        }

        public ViewItemCollection SearchItem(object name, bool IsInventoryItemOnly, bool ShowSystemItem, bool onlySearchItemNoItemName, string criteria)
        {
            if (string.IsNullOrEmpty(criteria)) criteria = "contains"; // contains | starts with | ends with | exact match

            ViewItemCollection coll1 = new ViewItemCollection();
            string sql = "SELECT Category.IsForSale, Category.IsDiscountable, Category.CategoryName, "
                            + "Category.Category_ID, Category.IsGST, Category.AccountCategory, "
                            + "Item.ItemNo, Item.ItemName, Item.Barcode, Item.RetailPrice, Item.FactoryPrice, "
                            + "Item.MinimumPrice, Item.ItemDesc, Item.IsInInventory, Item.IsNonDiscountable, "
                            + "Item.Brand, Item.ProductLine, Item.Remark, Item.Deleted, Item.Attributes1, "
                            + "Item.Attributes2, Item.Attributes3, Item.Attributes4, Item.Attributes5, "
                            + "Item.Attributes6, Item.Attributes8, Item.Attributes7, Category.ItemDepartmentId, "
                            + "ItemDepartment.DepartmentName, Item.IsServiceItem, Item.IsCourse, Item.CourseTypeID, "
                            + "Item.ProductionDate, Item.IsGST AS Expr1, Item.hasWarranty, Item.IsDelivery, "
                            + "Item.GSTRule, Item.IsVitaMix, Item.IsWaterFilter, Item.IsYoung, Item.IsJuicePlus, "
                            + "Item.IsCommission, ISNULL(dbo.Item.userflag1,'false') AS Userflag1, ISNULL(dbo.Item.userfld1, '') AS Uom "
                        + "FROM Category INNER JOIN "
                            + "Item ON Category.CategoryName = Item.CategoryName INNER JOIN "
                            + "ItemDepartment ON Category.ItemDepartmentId = ItemDepartment.ItemDepartmentID ";

            string searchText;
            if (criteria.ToLower() == "contains")
                searchText = "%" + name.ToString() + "%";
            else if (criteria.ToLower() == "starts with")
                searchText = name.ToString() + "%";
            else if (criteria.ToLower() == "ends with")
                searchText = "%" + name.ToString();
            else if (criteria.ToLower() == "exact match")
                searchText = name.ToString();
            else
                searchText = "%" + name.ToString() + "%";

            if (onlySearchItemNoItemName)
            {
                sql = sql + "WHERE (Item.ItemNo like N'" + searchText.Replace("'", "''") + "' OR Item.ItemName like N'" + searchText + "') AND Item.Deleted = 0";
            }
            else
            {
                sql = sql + "WHERE (Item.ItemNo like N'" + searchText.Replace("'", "''")
                          + "' OR Item.ItemName like N'" + searchText.Replace("'", "''")
                          + "' OR Item.CategoryName like N'" + searchText.Replace("'", "''")
                          + "' OR ISNULL(ItemDepartment.DepartmentName, '') like N'" + searchText.Replace("'", "''")
                          + "' OR ISNULL(Item.ItemDesc, '') like N'" + searchText.Replace("'", "''")
                          + "' OR ISNULL(Item.Barcode, '') like N'" + searchText.Replace("'", "''")
                          + "' OR ISNULL(Item.Attributes1, '') like N'" + searchText.Replace("'", "''")
                          + "' OR ISNULL(Item.Attributes2, '') like N'" + searchText.Replace("'", "''")
                          + "' OR ISNULL(Item.Attributes3, '') like N'" + searchText.Replace("'", "''")
                          + "' OR ISNULL(Item.Attributes4, '') like N'" + searchText.Replace("'", "''")
                          + "' OR ISNULL(Item.Attributes5, '') like N'" + searchText.Replace("'", "''")
                          + "' OR ISNULL(Item.Attributes6, '') like N'" + searchText.Replace("'", "''")
                          + "' OR ISNULL(Item.Attributes8, '') like N'" + searchText.Replace("'", "''")
                          + "' OR ISNULL(Item.Attributes7, '') like N'" + searchText.Replace("'", "''")
                          + "' OR Item.ItemNo in (select ItemNo from alternateBarcode where barcode like N'" + searchText.Replace("'", "''") + "') "
                          + ") AND Item.Deleted = 0";
            }

            if (IsInventoryItemOnly)
            {
                sql += "AND (Item.IsInInventory = 1 OR ISNULL(Item.Userflag7,0) = 1)";                
            }
            if (!ShowSystemItem)
            {
                sql += "AND Item.CategoryName <> 'SYSTEM' ";
            }

            IDataReader Rdr = SubSonic.DataService.GetReader(new SubSonic.QueryCommand(sql));

            coll1.LoadAndCloseReader(Rdr);

            return coll1;
        }

        public DataTable SearchItemUserPortal(object name, bool isSupplier, bool isRestrictedSupplier, int supplierID, string username,
            int inventoryLocationID, bool IsInventoryItemOnly, bool ShowSystemItem, bool onlySearchItemNoItemName, string criteria)
        {
            bool isUseUserPortal = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.User.UseSupplierPortal), false);
            if (string.IsNullOrEmpty(criteria)) criteria = "contains"; // contains | starts with | ends with | exact match

            string sql = "";

            string queryW = "Select count(*) from Supplier where ISNULL(Userflag1,0) = 1 and ISNULL(Deleted,0) = 0";

            int countWarehouse = (int) DataService.ExecuteScalar(new QueryCommand(queryW));

            DataTable coll1 = new DataTable();

                sql = @"SELECT Category.IsForSale, Category.IsDiscountable, Category.CategoryName, 
                             Category.Category_ID, Category.IsGST, Category.AccountCategory, 
                             Item.ItemNo, Item.ItemName, Item.Barcode, Item.RetailPrice, Item.FactoryPrice, 
                             Item.MinimumPrice, Item.ItemDesc, Item.IsInInventory, Item.IsNonDiscountable,
                             Item.Brand, Item.ProductLine, Item.Remark, Item.Deleted, Item.Attributes1, 
                             Item.Attributes2, Item.Attributes3, Item.Attributes4, Item.Attributes5, 
                             Item.Attributes6, Item.Attributes8, Item.Attributes7, Category.ItemDepartmentId, 
                             ItemDepartment.DepartmentName, Item.IsServiceItem, Item.IsCourse, Item.CourseTypeID, 
                             Item.ProductionDate, Item.IsGST AS ItemIsGST, Item.hasWarranty, Item.IsDelivery, 
                             Item.GSTRule, Item.IsVitaMix, Item.IsWaterFilter, Item.IsYoung, Item.IsJuicePlus, 
                             Item.IsCommission, ISNULL(dbo.Item.userflag1,'false') AS Userflag1,
                             ISNULL(its.BalanceQty,0) as OnHand 
                         FROM Category INNER JOIN 
                             Item ON Category.CategoryName = Item.CategoryName INNER JOIN 
                             ItemDepartment ON Category.ItemDepartmentId = ItemDepartment.ItemDepartmentID LEFT JOIN
                             ItemSummary its ON its.Itemno = Item.ItemNo and its.InventoryLocationID = {1} {0} ";
            
            string supplier = "";

            if (isUseUserPortal && isSupplier && isRestrictedSupplier)
            {
                List<string> supID = new List<string>();
                string supp = "";
                UserMst us = new UserMst(UserMst.Columns.UserName, username);

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

                supplier = "INNER JOIN ItemSupplierMap im on im.ItemNo = Item.ItemNo and im.SupplierID in (" + supp + ") ";

            }
            else if(supplierID != 0 && countWarehouse > 1)
            {
                supplier = "INNER JOIN ItemSupplierMap im on im.ItemNo = Item.ItemNo and im.SupplierID in (" + supplierID.ToString() + ") ";
            }

            sql = string.Format(sql, supplier, inventoryLocationID);


            string searchText;
            if (criteria.ToLower() == "contains")
                searchText = "%" + name.ToString() + "%";
            else if (criteria.ToLower() == "starts with")
                searchText = name.ToString() + "%";
            else if (criteria.ToLower() == "ends with")
                searchText = "%" + name.ToString();
            else if (criteria.ToLower() == "exact match")
                searchText = name.ToString();
            else
                searchText = "%" + name.ToString() + "%";

            if (onlySearchItemNoItemName)
            {
                sql = sql + "WHERE (Item.ItemNo like N'" + searchText.Replace("'", "''") + "' OR Item.ItemName like N'" + searchText + "') AND Item.Deleted = 0";
            }
            else
            {
                sql = sql + "WHERE (Item.ItemNo like N'" + searchText.Replace("'", "''")
                          + "' OR Item.ItemName like N'" + searchText.Replace("'", "''")
                          + "' OR Item.CategoryName like N'" + searchText.Replace("'", "''")
                          + "' OR ISNULL(ItemDepartment.DepartmentName, '') like N'" + searchText.Replace("'", "''")
                          + "' OR ISNULL(Item.ItemDesc, '') like N'" + searchText.Replace("'", "''")
                          + "' OR ISNULL(Item.Barcode, '') like N'" + searchText.Replace("'", "''")
                          + "' OR ISNULL(Item.Attributes1, '') like N'" + searchText.Replace("'", "''")
                          + "' OR ISNULL(Item.Attributes2, '') like N'" + searchText.Replace("'", "''")
                          + "' OR ISNULL(Item.Attributes3, '') like N'" + searchText.Replace("'", "''")
                          + "' OR ISNULL(Item.Attributes4, '') like N'" + searchText.Replace("'", "''")
                          + "' OR ISNULL(Item.Attributes5, '') like N'" + searchText.Replace("'", "''")
                          + "' OR ISNULL(Item.Attributes6, '') like N'" + searchText.Replace("'", "''")
                          + "' OR ISNULL(Item.Attributes8, '') like N'" + searchText.Replace("'", "''")
                          + "' OR ISNULL(Item.Attributes7, '') like N'" + searchText.Replace("'", "''")
                          + "' OR Item.ItemNo in (select ItemNo from alternateBarcode where barcode like N'" + searchText.Replace("'", "''") + "') " 
                          + ") AND Item.Deleted = 0";
            }

            if (IsInventoryItemOnly)
            {
                sql += "AND (Item.IsInInventory = 1 OR ISNULL(Item.Userflag7,0) = 1)";
                //sql += "AND Item.IsInInventory = 1 ";
            }
            if (!ShowSystemItem)
            {
                sql += "AND Item.CategoryName <> 'SYSTEM' ";
            }
            
            //sql += "AND ISNULL(Item.Userflag7,0) = 0 ";
            

            coll1 = DataService.GetDataSet(new SubSonic.QueryCommand(sql)).Tables[0];

            return coll1;
        }

             public DataTable SearchDeletedItem_PlusPointInfo(object name, bool IsInventoryItemOnly)
        {
            string QueryStr =
                "SELECT     dbo.Category.IsForSale, dbo.Category.IsDiscountable, dbo.Category.CategoryName, dbo.Category.Category_ID, dbo.Category.IsGST, dbo.Category.AccountCategory, " +
                      "dbo.Item.ItemNo, dbo.Item.ItemName, dbo.Item.Barcode, dbo.Item.RetailPrice, dbo.Item.FactoryPrice, dbo.Item.MinimumPrice, dbo.Item.ItemDesc, " +
                      "dbo.Item.IsInInventory, dbo.Item.IsNonDiscountable, dbo.Item.Brand, dbo.Item.ProductLine, dbo.Item.Remark, dbo.Item.Deleted, dbo.Item.Attributes1, " +
                      "dbo.Item.Attributes2, dbo.Item.Attributes3, dbo.Item.Attributes4, dbo.Item.Attributes5, dbo.Item.Attributes6, dbo.Item.Attributes8, dbo.Item.Attributes7, " +
                      "dbo.Category.ItemDepartmentId, dbo.ItemDepartment.DepartmentName, " +
                      "dbo.Item.IsServiceItem, dbo.Item.IsCourse, dbo.Item.CourseTypeID, dbo.Item.ProductionDate, dbo.Item.IsGST AS Expr1, dbo.Item.hasWarranty, " +
                      "dbo.Item.IsDelivery, dbo.Item.GSTRule, dbo.Item.IsVitaMix, dbo.Item.IsWaterFilter, dbo.Item.IsYoung, dbo.Item.IsJuicePlus, dbo.Item.IsCommission, ISNULL(dbo.Item.userfld10,'N') AS PointType, ISNULL(dbo.Item.userfloat1,0) AS PointAmount " +
                "FROM         dbo.Category INNER JOIN " +
                      "dbo.Item ON dbo.Category.CategoryName = dbo.Item.CategoryName INNER JOIN " +
                      "dbo.ItemDepartment ON dbo.Category.ItemDepartmentId = dbo.ItemDepartment.ItemDepartmentID " +
                "WHERE dbo.Item.Deleted = 1  AND " +
                      "(dbo.Item.ItemNo + ' ' + dbo.Item.ItemName + ' ' + ISNULL(dbo.Item.Barcode, '') + ' ' + dbo.Item.CategoryName + ' ' + ISNULL(dbo.ItemDepartment.DepartmentName, '') + ' ' + ISNULL(dbo.Item.ItemDesc, '')) LIKE '%' + @Search + '%' ";

            if (IsInventoryItemOnly)
            {
                QueryStr += "AND dbo.Item.IsInInventory = 1 ";
            }

            QueryCommand cmd = new QueryCommand(QueryStr);

            if (name == null) name = "";

            cmd.Parameters.Add("@Search", name);


            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(cmd));

            //mark true false to be yes no....
            dt = CommonUILib.DataTableConvertBoolColumnToYesOrNo(dt);
            dt = ConvertGSTRuleToString(dt);
            return dt;

        }

        public DataTable SearchItem_PlusPointInfo(object name, bool IsInventoryItemOnly)
        {
            string QueryStr =
                "SELECT     dbo.Category.IsForSale, dbo.Category.IsDiscountable, dbo.Category.CategoryName, dbo.Category.Category_ID, dbo.Category.IsGST, dbo.Category.AccountCategory, " +
                      "dbo.Item.ItemNo, dbo.Item.ItemName, dbo.Item.Barcode, dbo.Item.RetailPrice, dbo.Item.FactoryPrice, dbo.Item.MinimumPrice, dbo.Item.ItemDesc, " +
                      "dbo.Item.IsInInventory, dbo.Item.IsNonDiscountable, dbo.Item.Brand, dbo.Item.ProductLine, dbo.Item.Remark, dbo.Item.Deleted, dbo.Item.Attributes1, " +
                      "dbo.Item.Attributes2, dbo.Item.Attributes3, dbo.Item.Attributes4, dbo.Item.Attributes5, dbo.Item.Attributes6, dbo.Item.Attributes8, dbo.Item.Attributes7, " +
                      "dbo.Category.ItemDepartmentId, dbo.ItemDepartment.DepartmentName, " +
                      "dbo.Item.IsServiceItem, dbo.Item.IsCourse, dbo.Item.CourseTypeID, dbo.Item.ProductionDate, dbo.Item.IsGST AS Expr1, dbo.Item.hasWarranty, " +
                      "dbo.Item.IsDelivery, dbo.Item.GSTRule, dbo.Item.IsVitaMix, dbo.Item.IsWaterFilter, dbo.Item.IsYoung, dbo.Item.IsJuicePlus, dbo.Item.IsCommission, ISNULL(dbo.Item.userfld10,'N') AS PointType, ISNULL(dbo.Item.userfloat1,0) AS PointAmount " +
                "FROM         dbo.Category INNER JOIN " +
                      "dbo.Item ON dbo.Category.CategoryName = dbo.Item.CategoryName INNER JOIN " +
                      "dbo.ItemDepartment ON dbo.Category.ItemDepartmentId = dbo.ItemDepartment.ItemDepartmentID " +
                "WHERE (dbo.Item.Deleted = 0 OR dbo.Item.Deleted IS NULL) AND " +
                      "(dbo.Item.ItemNo + ' ' + dbo.Item.ItemName + ' ' + ISNULL(dbo.Item.Barcode, '') + ' ' + dbo.Item.CategoryName + ' ' + ISNULL(dbo.ItemDepartment.DepartmentName, '') + ' ' + ISNULL(dbo.Item.ItemDesc, '')) LIKE '%' + @Search + '%' ";

            if (IsInventoryItemOnly)
                QueryStr += "AND dbo.Item.IsInInventory = 1 ";

            QueryStr += "ORDER BY ItemNo ";

            QueryCommand cmd = new QueryCommand(QueryStr);

            if (name == null) name = "";

            cmd.Parameters.Add("@Search", name);


            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(cmd));

            //mark true false to be yes no....
            dt = CommonUILib.DataTableConvertBoolColumnToYesOrNo(dt);
            dt = ConvertGSTRuleToString(dt);
            return dt;

        }

        public DataTable SearchItem_PlusPointInfoMatrix(object name, bool IsInventoryItemOnly)
        {
            string QueryStr =
                "SELECT     dbo.Category.IsForSale, dbo.Category.IsDiscountable, dbo.Category.CategoryName, dbo.Category.Category_ID, dbo.Category.IsGST, dbo.Category.AccountCategory, " +
                      "dbo.Item.ItemNo, dbo.Item.ItemName, dbo.Item.Barcode, dbo.Item.RetailPrice, dbo.Item.FactoryPrice, dbo.Item.MinimumPrice, dbo.Item.ItemDesc, " +
                      "dbo.Item.IsInInventory, dbo.Item.IsNonDiscountable, dbo.Item.Brand, dbo.Item.ProductLine, dbo.Item.Remark, dbo.Item.Deleted, dbo.Item.Attributes1, " +
                      "dbo.Item.Attributes2, dbo.Item.Attributes3, dbo.Item.Attributes4, dbo.Item.Attributes5, dbo.Item.Attributes6, dbo.Item.Attributes8, dbo.Item.Attributes7, " +
                      "dbo.Category.ItemDepartmentId, dbo.ItemDepartment.DepartmentName, " +
                      "dbo.Item.IsServiceItem, dbo.Item.IsCourse, dbo.Item.CourseTypeID, dbo.Item.ProductionDate, dbo.Item.IsGST AS Expr1, dbo.Item.hasWarranty, " +
                      "dbo.Item.IsDelivery, dbo.Item.GSTRule, dbo.Item.IsVitaMix, dbo.Item.IsWaterFilter, dbo.Item.IsYoung, dbo.Item.IsJuicePlus, dbo.Item.IsCommission, " +
                      "ISNULL(dbo.Item.userfld10,'N') AS PointType, ISNULL(dbo.Item.userfloat1,0) AS PointAmount " +
                "FROM         dbo.Category INNER JOIN " +
                      "dbo.Item ON dbo.Category.CategoryName = dbo.Item.CategoryName INNER JOIN " +
                      "dbo.ItemDepartment ON dbo.Category.ItemDepartmentId = dbo.ItemDepartment.ItemDepartmentID " +
                "WHERE (dbo.Item.Deleted = 0 OR dbo.Item.Deleted IS NULL) AND " +
                      "(dbo.Item.ItemNo + ' ' + dbo.Item.ItemName + ' ' + ISNULL(dbo.Item.Barcode, '') + ' ' + dbo.Item.CategoryName + ' ' + ISNULL(dbo.ItemDepartment.DepartmentName, '') + ' ' + ISNULL(dbo.Item.ItemDesc, '')) LIKE '%' + @Search + '%' ";

            if (IsInventoryItemOnly)
                QueryStr += "AND dbo.Item.IsInInventory = 1 ";

            QueryStr += "ORDER BY ItemNo ";

            QueryCommand cmd = new QueryCommand(QueryStr);

            if (name == null) name = "";

            cmd.Parameters.Add("@Search", name);


            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(cmd));

            //mark true false to be yes no....
            dt = CommonUILib.DataTableConvertBoolColumnToYesOrNo(dt);
            dt = ConvertGSTRuleToString(dt);
            return dt;

        }

        public DataTable SearchItem_ProductSetup(object name, bool IsInventoryItemOnly)
        {
            string QueryStr =
                "SELECT * " +
                "FROM( " +
                "SELECT dbo.Category.IsForSale, dbo.Category.IsDiscountable, dbo.Category.CategoryName, dbo.Category.Category_ID, dbo.Category.IsGST, dbo.Category.AccountCategory, " +
                      "dbo.Item.ItemNo, dbo.Item.ItemName, dbo.Item.Barcode, dbo.Item.RetailPrice, dbo.Item.FactoryPrice, dbo.Item.ItemDesc, ISNULL(dbo.Item.userflag1,0) as Userflag1,  " +
                      "dbo.Item.IsInInventory, dbo.Item.IsNonDiscountable, dbo.Item.Deleted, dbo.Category.ItemDepartmentId, dbo.ItemDepartment.DepartmentName, dbo.Item.GSTRule " +
                "FROM  dbo.Category INNER JOIN  " +
                      "dbo.Item ON dbo.Category.CategoryName = dbo.Item.CategoryName INNER JOIN   " +
                      "dbo.ItemDepartment ON dbo.Category.ItemDepartmentId = dbo.ItemDepartment.ItemDepartmentID  " +
                "WHERE (dbo.Item.Deleted = 0 OR dbo.Item.Deleted IS NULL) AND ISNULL(dbo.Item.userflag1,0) = 0  " +
                "UNION " +
                "SELECT distinct dbo.Category.IsForSale, dbo.Category.IsDiscountable, dbo.Category.CategoryName, dbo.Category.Category_ID, dbo.Category.IsGST, dbo.Category.AccountCategory,   " +
                      "dbo.Item.Attributes1 AS ItemNo, dbo.Item.Attributes2 as ItemName, dbo.Item.Barcode, dbo.Item.RetailPrice, dbo.Item.FactoryPrice, dbo.Item.ItemDesc, ISNULL(dbo.Item.userflag1,0) as Userflag1, " +
                      "dbo.Item.IsInInventory, dbo.Item.IsNonDiscountable, dbo.Item.Deleted, dbo.Category.ItemDepartmentId, dbo.ItemDepartment.DepartmentName, dbo.Item.GSTRule " +
                "FROM  dbo.Category INNER JOIN   " +
                      "dbo.Item ON dbo.Category.CategoryName = dbo.Item.CategoryName INNER JOIN  " +
                      "dbo.ItemDepartment ON dbo.Category.ItemDepartmentId = dbo.ItemDepartment.ItemDepartmentID   " +
                "WHERE (dbo.Item.Deleted = 0 OR dbo.Item.Deleted IS NULL) AND ISNULL(dbo.Item.userflag1,0) = 1 " +
                ") AS A  " +
                "WHERE  (A.ItemNo + ' ' + A.ItemName + ' ' + ISNULL(A.Barcode, '') + ' ' + A.CategoryName + ' ' + ISNULL(A.DepartmentName, '') + ' ' + ISNULL(A.ItemDesc, '')) LIKE '%' + @Search + '%'  ";

            if (IsInventoryItemOnly)
                QueryStr += "AND dbo.Item.IsInInventory = 1 ";

            QueryStr += "ORDER BY ItemNo ";

            QueryCommand cmd = new QueryCommand(QueryStr);

            if (name == null) name = "";

            cmd.Parameters.Add("@Search", name);


            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(cmd));

            //mark true false to be yes no....
            dt = CommonUILib.DataTableConvertBoolColumnToYesOrNo(dt);
            dt = ConvertGSTRuleToString(dt);
            return dt;

        }

        public DataTable SearchItem_PlusPointInfoMultipleCategorySearchWithAttributesFilter(object name, bool IsInventoryItemOnly, string[] category, ArrayList ItemNames, ArrayList attr1, ArrayList attr2, ArrayList attr3, ArrayList attr4, ArrayList attr5, ArrayList attr6, ArrayList attr7)
        {
            string QueryStr =
                "SELECT     dbo.Category.IsForSale, dbo.Category.IsDiscountable, dbo.Category.CategoryName, dbo.Category.Category_ID, dbo.Category.IsGST, dbo.Category.AccountCategory, " +
                      "dbo.Item.ItemNo, dbo.Item.ItemName, dbo.Item.Barcode, dbo.Item.RetailPrice, dbo.Item.FactoryPrice, dbo.Item.MinimumPrice, dbo.Item.ItemDesc, " +
                      "dbo.Item.IsInInventory, dbo.Item.IsNonDiscountable, dbo.Item.Brand, dbo.Item.ProductLine, dbo.Item.Remark, dbo.Item.Attributes1, " +
                      "dbo.Item.Attributes2, dbo.Item.Attributes3, dbo.Item.Attributes4, dbo.Item.Attributes5, dbo.Item.Attributes6, dbo.Item.Attributes8, dbo.Item.Attributes7, " +
                      "dbo.Category.ItemDepartmentId, dbo.ItemDepartment.DepartmentName, " +
                      "dbo.Item.IsServiceItem, dbo.Item.IsCourse, dbo.Item.CourseTypeID, dbo.Item.ProductionDate, dbo.Item.IsGST AS Expr1, dbo.Item.hasWarranty, " +
                      "dbo.Item.IsDelivery, dbo.Item.GSTRule, dbo.Item.IsVitaMix, dbo.Item.IsWaterFilter, dbo.Item.IsYoung, dbo.Item.IsJuicePlus, dbo.Item.IsCommission, ISNULL(dbo.Item.userfld10,'N') AS PointType, ISNULL(dbo.Item.userfloat1,0) AS PointAmount, dbo.Item.Deleted " +
                "FROM         dbo.Category INNER JOIN " +
                      "dbo.Item ON dbo.Category.CategoryName = dbo.Item.CategoryName INNER JOIN " +
                      "dbo.ItemDepartment ON dbo.Category.ItemDepartmentId = dbo.ItemDepartment.ItemDepartmentID " +
                "WHERE (dbo.Item.Deleted = 0 OR dbo.Item.Deleted IS NULL) AND " +
                      "(dbo.Item.ItemNo + ' ' + dbo.Item.ItemName + ' ' + ISNULL(dbo.Item.Barcode, '') + ' ' + dbo.Item.CategoryName + ' ' + ISNULL(dbo.ItemDepartment.DepartmentName, '') + ' ' + ISNULL(dbo.Item.ItemDesc, '')) LIKE '%' + @Search + '%' ";

            if (ItemNames != null && ItemNames.Count > 0)
            {
                string criteria = "";
                for (int i = 0; i < ItemNames.Count; i++)
                    criteria += "N" + string.Format("'{0}',", ItemNames[i].ToString());

                criteria = criteria.Remove(criteria.Length - 1);
                QueryStr += string.Format(" AND ISNULL(Item.ItemName,'') IN ({0}) ", criteria);
            }
            if (attr1 != null && attr1.Count > 0)
            {
                string criteria = "";
                for (int i = 0; i < attr1.Count; i++)
                    criteria += string.Format("'{0}',", attr1[i].ToString());

                criteria = criteria.Remove(criteria.Length - 1);
                QueryStr += string.Format(" AND ISNULL(Item.Attributes1,'') IN ({0}) ", criteria);
            }
            if (attr2 != null && attr2.Count > 0)
            {
                string criteria = "";
                for (int i = 0; i < attr2.Count; i++)
                    criteria += string.Format("'{0}',", attr2[i].ToString());

                criteria = criteria.Remove(criteria.Length - 1);
                QueryStr += string.Format(" AND ISNULL(Item.Attributes2,'') IN ({0}) ", criteria);
            }
            if (attr3 != null && attr3.Count > 0)
            {
                string criteria = "";
                for (int i = 0; i < attr3.Count; i++)
                    criteria += string.Format("'{0}',", attr3[i].ToString());

                criteria = criteria.Remove(criteria.Length - 1);
                QueryStr += string.Format(" AND ISNULL(Item.Attributes3,'') IN ({0}) ", criteria);
            }
            if (attr4 != null && attr4.Count > 0)
            {
                string criteria = "";
                for (int i = 0; i < attr4.Count; i++)
                    criteria += string.Format("'{0}',", attr4[i].ToString());

                criteria = criteria.Remove(criteria.Length - 1);
                QueryStr += string.Format(" AND ISNULL(Item.Attributes4,'') IN ({0}) ", criteria);
            }
            if (attr5 != null && attr5.Count > 0)
            {
                string criteria = "";
                for (int i = 0; i < attr5.Count; i++)
                    criteria += string.Format("'{0}',", attr5[i].ToString());

                criteria = criteria.Remove(criteria.Length - 1);
                QueryStr += string.Format(" AND ISNULL(Item.Attributes5,'') IN ({0}) ", criteria);
            }
            if (attr6 != null && attr6.Count > 0)
            {
                string criteria = "";
                for (int i = 0; i < attr6.Count; i++)
                    criteria += string.Format("'{0}',", attr6[i].ToString());

                criteria = criteria.Remove(criteria.Length - 1);
                QueryStr += string.Format(" AND ISNULL(Item.Attributes6,'') IN ({0}) ", criteria);
            }
            //if (attribute != "")
            //{
            //    QueryStr += " AND ISNULL(Item.Attributes1,'') + ISNULL(Item.Attributes2,'') +  ISNULL(Item.Attributes3,'') + ISNULL(Item.Attributes4,'') + ISNULL(Item.Attributes5,'') + ISNULL(Item.Attributes6,'') + ISNULL(Item.Attributes7,'') LIKE '%" + attribute + "%' ";
            //}         

            QueryParameterCollection parameters = new QueryParameterCollection();
            if (category != null && category.Length != 0)
            {
                string categoryCriteria = "";
                for (int i = 0; i < category.Length; i++)
                {
                    QueryParameter param = new QueryParameter();
                    param.DataType = DbType.String;
                    param.ParameterName = string.Format("@tag{0}", i);
                    param.ParameterValue = string.Format("{0}", category[i].Trim());

                    categoryCriteria += string.Format("{0},", param.ParameterName);

                    parameters.Add(param);
                }
                categoryCriteria = categoryCriteria.Remove(categoryCriteria.Length - 1);
                QueryStr += string.Format(" AND Item.CategoryName IN({0})", categoryCriteria);
                //

            }


            if (IsInventoryItemOnly)
                QueryStr += "AND dbo.Item.IsInInventory = 1 ";

            QueryStr += "ORDER BY ItemNo ";

            QueryCommand cmd = new QueryCommand(QueryStr);

            if (name == null) name = "";

            cmd.Parameters.Add("@Search", name, DbType.String);

            if (parameters.Count > 0)
            {
                for (int i = 0; i < parameters.Count; i++)
                    cmd.Parameters.Add(parameters[i]);
            }


            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(cmd));

            //mark true false to be yes no....
            dt = CommonUILib.DataTableConvertBoolColumnToYesOrNo(dt);
            dt = ConvertGSTRuleToString(dt);
            return dt;

        }

        public DataTable SearchItemWithPaging(string search, bool IsInventoryItemOnly, string[] category,
            string applicableTo, string outlet, ArrayList ItemNames,
            ArrayList attr1, ArrayList attr2, ArrayList attr3, ArrayList attr4, ArrayList attr5,
            ArrayList attr6, ArrayList attr7, ArrayList attr8, ArrayList attr9, ArrayList attr10,
            int pageNo, out int totalPageNo, bool HideDeletedItem)
        {
            totalPageNo = 0;
            int pageSize = (AppSetting.GetSetting(AppSetting.SettingsName.Item.ProductSetupPageSize) + "").GetIntValue();
            if (pageSize <= 0)
                pageSize = 20;

            string innerQuery = "";
            string countQuery = "";

            if (applicableTo == "Product Master")
            {
                innerQuery = @"SELECT ROW_NUMBER() OVER(ORDER BY dbo.Item.ItemNo) No     
                                    ,dbo.Category.IsForSale
		                            ,dbo.Category.IsDiscountable
		                            ,dbo.Category.CategoryName
		                            ,dbo.Category.Category_ID
		                            ,dbo.Category.IsGST
		                            ,dbo.Category.AccountCategory
		                            ,dbo.Item.ItemNo
		                            ,dbo.Item.ItemName
		                            ,dbo.Item.Barcode
		                            ,dbo.Item.RetailPrice
		                            ,dbo.Item.FactoryPrice
		                            ,dbo.Item.MinimumPrice
		                            ,dbo.Item.ItemDesc
		                            ,dbo.Item.IsInInventory
		                            ,dbo.Item.IsNonDiscountable
		                            ,dbo.Item.Brand
		                            ,dbo.Item.ProductLine
		                            ,dbo.Item.Remark
		                            ,dbo.Item.Attributes1
		                            ,dbo.Item.Attributes2
		                            ,dbo.Item.Attributes3
		                            ,dbo.Item.Attributes4
		                            ,dbo.Item.Attributes5
		                            ,dbo.Item.Attributes6
		                            ,dbo.Item.Attributes7		
		                            ,dbo.Item.Attributes8
		                            ,dbo.Category.ItemDepartmentId
		                            ,dbo.ItemDepartment.DepartmentName
		                            ,dbo.Item.IsServiceItem
		                            ,dbo.Item.IsCourse
		                            ,dbo.Item.CourseTypeID
		                            ,dbo.Item.ProductionDate
		                            ,dbo.Item.IsGST AS Expr1
		                            ,dbo.Item.hasWarranty
		                            ,dbo.Item.IsDelivery
		                            ,dbo.Item.GSTRule
		                            ,dbo.Item.IsVitaMix
		                            ,dbo.Item.IsWaterFilter
		                            ,dbo.Item.IsYoung
		                            ,dbo.Item.IsJuicePlus
		                            ,dbo.Item.IsCommission
		                            ,ISNULL(dbo.Item.userfld10,'N') AS PointType
		                            ,ISNULL(dbo.Item.userfloat1,0) AS PointAmount
		                            ,dbo.Item.Deleted
		                            ,ISNULL(dbo.Item.Userflag1,'false') as Userflag1  
                                    , ISNULL(dbo.Item.Userfld1,'') as UOM
                                    , dbo.Item.CreatedOn 
                                    , dbo.Item.CreatedBy 
                                    , ISNULL(SUPP.Supplier,'') Supplier
                            FROM    dbo.Category 
		                            INNER JOIN dbo.Item ON dbo.Category.CategoryName = dbo.Item.CategoryName 
		                            INNER JOIN dbo.ItemDepartment ON dbo.Category.ItemDepartmentId = dbo.ItemDepartment.ItemDepartmentID
		                            LEFT JOIN (
			                            SELECT	AB.ItemNo,
					                            STUFF((
						                            SELECT ', ' + Barcode
						                            FROM	AlternateBarcode
						                            WHERE	ISNULL(Deleted,0) = 0 
								                            AND ItemNo = AB.ItemNo
								                            AND Barcode LIKE '%' + @Search + '%'  
						                            FOR XML PATH(''),TYPE).value('(./text())[1]','NVARCHAR(MAX)')
					                              ,1,2,'') AS AlternateBarcode
			                            FROM	AlternateBarcode AB
			                            WHERE	ISNULL(AB.Deleted,0) = 0
					                            AND AB.Barcode LIKE '%' + @Search + '%' 
			                            GROUP BY AB.ItemNo		
		                            ) ALT ON ALT.ItemNo = dbo.Item.ItemNo
                                    LEFT JOIN (
                                        SELECT  ISM.ItemNo,
		                                        STUFF((
			                                        SELECT  ', ' + SUPP.SupplierName
			                                        FROM	ItemSupplierMap IMAP
					                                        INNER JOIN Supplier SUPP ON SUPP.SupplierID = IMAP.SupplierID
			                                        WHERE	ISNULL(IMAP.Deleted,0) = 0 
					                                        AND ISNULL(SUPP.Deleted,0) = 0
					                                        AND IMAP.ItemNo = ISM.ItemNo
			                                        FOR XML PATH(''),TYPE).value('(./text())[1]','NVARCHAR(MAX)')
			                                        ,1,2,'') AS Supplier
                                        FROM	ItemSupplierMap ISM
                                        GROUP BY ISM.ItemNo
                                    ) SUPP ON SUPP.ItemNo = dbo.Item.ItemNo    ";
                countQuery = @"SELECT  @TotalPageSize = COUNT(*)
                               FROM    dbo.Category 
		                               INNER JOIN dbo.Item ON dbo.Category.CategoryName = dbo.Item.CategoryName 
		                               INNER JOIN dbo.ItemDepartment ON dbo.Category.ItemDepartmentId = dbo.ItemDepartment.ItemDepartmentID 
		                                LEFT JOIN (
			                                SELECT	AB.ItemNo,
					                                STUFF((
						                                SELECT ', ' + Barcode
						                                FROM	AlternateBarcode
						                                WHERE	ISNULL(Deleted,0) = 0 
								                                AND ItemNo = AB.ItemNo
								                                AND Barcode LIKE '%' + @Search + '%'  
						                                FOR XML PATH(''),TYPE).value('(./text())[1]','NVARCHAR(MAX)')
					                                  ,1,2,'') AS AlternateBarcode
			                                FROM	AlternateBarcode AB
			                                WHERE	ISNULL(AB.Deleted,0) = 0
					                                AND AB.Barcode LIKE '%' + @Search + '%' 
			                                GROUP BY AB.ItemNo		
		                                ) ALT ON ALT.ItemNo = dbo.Item.ItemNo		
                                        LEFT JOIN (
                                            SELECT  ISM.ItemNo,
		                                            STUFF((
			                                            SELECT  ', ' + SUPP.SupplierName
			                                            FROM	ItemSupplierMap IMAP
					                                            INNER JOIN Supplier SUPP ON SUPP.SupplierID = IMAP.SupplierID
			                                            WHERE	ISNULL(IMAP.Deleted,0) = 0 
					                                            AND ISNULL(SUPP.Deleted,0) = 0
					                                            AND IMAP.ItemNo = ISM.ItemNo
			                                            FOR XML PATH(''),TYPE).value('(./text())[1]','NVARCHAR(MAX)')
			                                            ,1,2,'') AS Supplier
                                            FROM	ItemSupplierMap ISM
                                            GROUP BY ISM.ItemNo
                                        ) SUPP ON SUPP.ItemNo = dbo.Item.ItemNo    ";
            }
            else if (applicableTo == "Outlet Group")
            {
                innerQuery = @"SELECT ROW_NUMBER() OVER(ORDER BY dbo.Item.ItemNo) No    
                                    ,dbo.Category.IsForSale
		                            ,dbo.Category.IsDiscountable
		                            ,dbo.Category.CategoryName
		                            ,dbo.Category.Category_ID
		                            ,dbo.Category.IsGST
		                            ,dbo.Category.AccountCategory
		                            ,dbo.Item.ItemNo
		                            ,{1}
		                            ,dbo.Item.Barcode
		                            ,CASE WHEN OGIM.OutletGroupItemMapID IS NULL THEN dbo.Item.RetailPrice ELSE OGIM.RetailPrice END RetailPrice
		                            ,CASE WHEN OGIM.OutletGroupItemMapID IS NULL THEN dbo.Item.FactoryPrice ELSE OGIM.CostPrice END FactoryPrice
		                            ,dbo.Item.MinimumPrice
		                            ,dbo.Item.ItemDesc
		                            ,dbo.Item.IsInInventory
		                            ,dbo.Item.IsNonDiscountable
		                            ,dbo.Item.Brand
		                            ,dbo.Item.ProductLine
		                            ,dbo.Item.Remark
		                            ,dbo.Item.Attributes1
		                            ,dbo.Item.Attributes2
		                            ,dbo.Item.Attributes3
		                            ,dbo.Item.Attributes4
		                            ,dbo.Item.Attributes5
		                            ,dbo.Item.Attributes6
		                            ,dbo.Item.Attributes7		
		                            ,dbo.Item.Attributes8
		                            ,dbo.Category.ItemDepartmentId
		                            ,dbo.ItemDepartment.DepartmentName
		                            ,dbo.Item.IsServiceItem
		                            ,dbo.Item.IsCourse
		                            ,dbo.Item.CourseTypeID
		                            ,dbo.Item.ProductionDate
		                            ,dbo.Item.IsGST AS Expr1
		                            ,dbo.Item.hasWarranty
		                            ,dbo.Item.IsDelivery
		                            ,dbo.Item.GSTRule
		                            ,dbo.Item.IsVitaMix
		                            ,dbo.Item.IsWaterFilter
		                            ,dbo.Item.IsYoung
		                            ,dbo.Item.IsJuicePlus
		                            ,dbo.Item.IsCommission
		                            ,ISNULL(dbo.Item.userfld10,'N') AS PointType
		                            ,ISNULL(dbo.Item.userfloat1,0) AS PointAmount
		                            ,CASE WHEN OGIM.OutletGroupItemMapID IS NULL THEN dbo.Item.Deleted ELSE OGIM.Deleted END Deleted
		                            ,ISNULL(dbo.Item.Userflag1,'false') as Userflag1  
                                    ,ISNULL(dbo.Item.Userfld1,'') as UOM
                                    , dbo.Item.CreatedOn 
                                    , dbo.Item.CreatedBy
                                    , ISNULL(SUPP.Supplier,'') Supplier
                            FROM    dbo.Category 
		                            INNER JOIN dbo.Item ON dbo.Category.CategoryName = dbo.Item.CategoryName 
		                            INNER JOIN dbo.ItemDepartment ON dbo.Category.ItemDepartmentId = dbo.ItemDepartment.ItemDepartmentID
		                            LEFT OUTER JOIN dbo.OutletGroupItemMap OGIM ON OGIM.ItemNo = dbo.Item.ItemNo AND OGIM.OutletGroupID = '{0}' 
		                            LEFT JOIN (
			                            SELECT	AB.ItemNo,
					                            STUFF((
						                            SELECT ', ' + Barcode
						                            FROM	AlternateBarcode
						                            WHERE	ISNULL(Deleted,0) = 0 
								                            AND ItemNo = AB.ItemNo
								                            AND Barcode LIKE '%' + @Search + '%'  
						                            FOR XML PATH(''),TYPE).value('(./text())[1]','NVARCHAR(MAX)')
					                              ,1,2,'') AS AlternateBarcode
			                            FROM	AlternateBarcode AB
			                            WHERE	ISNULL(AB.Deleted,0) = 0
					                            AND AB.Barcode LIKE '%' + @Search + '%' 
			                            GROUP BY AB.ItemNo		
		                            ) ALT ON ALT.ItemNo = dbo.Item.ItemNo		
                                    LEFT JOIN (
                                        SELECT  ISM.ItemNo,
		                                        STUFF((
			                                        SELECT  ', ' + SUPP.SupplierName
			                                        FROM	ItemSupplierMap IMAP
					                                        INNER JOIN Supplier SUPP ON SUPP.SupplierID = IMAP.SupplierID
			                                        WHERE	ISNULL(IMAP.Deleted,0) = 0 
					                                        AND ISNULL(SUPP.Deleted,0) = 0
					                                        AND IMAP.ItemNo = ISM.ItemNo
			                                        FOR XML PATH(''),TYPE).value('(./text())[1]','NVARCHAR(MAX)')
			                                        ,1,2,'') AS Supplier
                                        FROM	ItemSupplierMap ISM
                                        GROUP BY ISM.ItemNo
                                    ) SUPP ON SUPP.ItemNo = dbo.Item.ItemNo    ";
                countQuery = @"SELECT  @TotalPageSize = COUNT(*)
                                FROM    dbo.Category 
		                                INNER JOIN dbo.Item ON dbo.Category.CategoryName = dbo.Item.CategoryName 
		                                INNER JOIN dbo.ItemDepartment ON dbo.Category.ItemDepartmentId = dbo.ItemDepartment.ItemDepartmentID
		                                LEFT JOIN (
			                                SELECT	AB.ItemNo,
					                                STUFF((
						                                SELECT ', ' + Barcode
						                                FROM	AlternateBarcode
						                                WHERE	ISNULL(Deleted,0) = 0 
								                                AND ItemNo = AB.ItemNo
								                                AND Barcode LIKE '%' + @Search + '%'  
						                                FOR XML PATH(''),TYPE).value('(./text())[1]','NVARCHAR(MAX)')
					                                  ,1,2,'') AS AlternateBarcode
			                                FROM	AlternateBarcode AB
			                                WHERE	ISNULL(AB.Deleted,0) = 0
					                                AND AB.Barcode LIKE '%' + @Search + '%' 
			                                GROUP BY AB.ItemNo		
		                                ) ALT ON ALT.ItemNo = dbo.Item.ItemNo		
                                        LEFT JOIN (
                                            SELECT  ISM.ItemNo,
		                                            STUFF((
			                                            SELECT  ', ' + SUPP.SupplierName
			                                            FROM	ItemSupplierMap IMAP
					                                            INNER JOIN Supplier SUPP ON SUPP.SupplierID = IMAP.SupplierID
			                                            WHERE	ISNULL(IMAP.Deleted,0) = 0 
					                                            AND ISNULL(SUPP.Deleted,0) = 0
					                                            AND IMAP.ItemNo = ISM.ItemNo
			                                            FOR XML PATH(''),TYPE).value('(./text())[1]','NVARCHAR(MAX)')
			                                            ,1,2,'') AS Supplier
                                            FROM	ItemSupplierMap ISM
                                            GROUP BY ISM.ItemNo
                                        ) SUPP ON SUPP.ItemNo = dbo.Item.ItemNo    
		                                LEFT OUTER JOIN dbo.OutletGroupItemMap OGIM ON OGIM.ItemNo = dbo.Item.ItemNo AND OGIM.OutletGroupID = '{0}'";

                string itemNameQ = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.AllowOverrideItemNameOutlet), false) ? "CASE WHEN OGIM.OutletGroupItemMapID IS NULL THEN dbo.Item.ItemName ELSE ISNULL(OGIM.Userfld1, dbo.Item.ItemName) END ItemName" : "dbo.Item.ItemName";
                innerQuery = string.Format(innerQuery, outlet, itemNameQ);
                countQuery = string.Format(countQuery, outlet);
            }
            else if (applicableTo == "Outlet")
            {
                innerQuery = @"SELECT ROW_NUMBER() OVER(ORDER BY dbo.Item.ItemNo) No   
                                    ,dbo.Category.IsForSale
		                            ,dbo.Category.IsDiscountable
		                            ,dbo.Category.CategoryName
		                            ,dbo.Category.Category_ID
		                            ,dbo.Category.IsGST
		                            ,dbo.Category.AccountCategory
		                            ,dbo.Item.ItemNo
		                            ,{1}
		                            ,dbo.Item.Barcode
		                            ,CASE WHEN OGIM.OutletGroupItemMapID IS NULL THEN (CASE WHEN OGIM2.OutletGroupItemMapID IS NULL THEN dbo.Item.RetailPrice ELSE OGIM2.RetailPrice END) ELSE OGIM.RetailPrice END RetailPrice
		                            ,CASE WHEN OGIM.OutletGroupItemMapID IS NULL THEN (CASE WHEN OGIM2.OutletGroupItemMapID IS NULL THEN dbo.Item.FactoryPrice ELSE OGIM2.CostPrice END) ELSE OGIM.CostPrice END FactoryPrice
		                            ,dbo.Item.MinimumPrice
		                            ,dbo.Item.ItemDesc
		                            ,dbo.Item.IsInInventory
		                            ,dbo.Item.IsNonDiscountable
		                            ,dbo.Item.Brand
		                            ,dbo.Item.ProductLine
		                            ,dbo.Item.Remark
		                            ,dbo.Item.Attributes1
		                            ,dbo.Item.Attributes2
		                            ,dbo.Item.Attributes3
		                            ,dbo.Item.Attributes4
		                            ,dbo.Item.Attributes5
		                            ,dbo.Item.Attributes6
		                            ,dbo.Item.Attributes7		
		                            ,dbo.Item.Attributes8
		                            ,dbo.Category.ItemDepartmentId
		                            ,dbo.ItemDepartment.DepartmentName
		                            ,dbo.Item.IsServiceItem
		                            ,dbo.Item.IsCourse
		                            ,dbo.Item.CourseTypeID
		                            ,dbo.Item.ProductionDate
		                            ,dbo.Item.IsGST AS Expr1
		                            ,dbo.Item.hasWarranty
		                            ,dbo.Item.IsDelivery
		                            ,dbo.Item.GSTRule
		                            ,dbo.Item.IsVitaMix
		                            ,dbo.Item.IsWaterFilter
		                            ,dbo.Item.IsYoung
		                            ,dbo.Item.IsJuicePlus
		                            ,dbo.Item.IsCommission
		                            ,ISNULL(dbo.Item.userfld10,'N') AS PointType
		                            ,ISNULL(dbo.Item.userfloat1,0) AS PointAmount
		                            ,CASE WHEN OGIM.OutletGroupItemMapID IS NULL THEN (CASE WHEN OGIM2.OutletGroupItemMapID IS NULL THEN dbo.Item.Deleted ELSE OGIM2.Deleted END) ELSE OGIM.Deleted END Deleted
		                            ,ISNULL(dbo.Item.Userflag1,'false') as Userflag1  
                                    , ISNULL(dbo.Item.Userfld1,'') as UOM
                                    , dbo.Item.CreatedOn 
                                    , dbo.Item.CreatedBy
                                    , ISNULL(SUPP.Supplier,'') Supplier
                            FROM    dbo.Category 
		                            INNER JOIN dbo.Item ON dbo.Category.CategoryName = dbo.Item.CategoryName 
		                            INNER JOIN dbo.ItemDepartment ON dbo.Category.ItemDepartmentId = dbo.ItemDepartment.ItemDepartmentID
		                            LEFT JOIN (
			                            SELECT	AB.ItemNo,
					                            STUFF((
						                            SELECT ', ' + Barcode
						                            FROM	AlternateBarcode
						                            WHERE	ISNULL(Deleted,0) = 0 
								                            AND ItemNo = AB.ItemNo
								                            AND Barcode LIKE '%' + @Search + '%'  
						                            FOR XML PATH(''),TYPE).value('(./text())[1]','NVARCHAR(MAX)')
					                              ,1,2,'') AS AlternateBarcode
			                            FROM	AlternateBarcode AB
			                            WHERE	ISNULL(AB.Deleted,0) = 0
					                            AND AB.Barcode LIKE '%' + @Search + '%' 
			                            GROUP BY AB.ItemNo		
		                            ) ALT ON ALT.ItemNo = dbo.Item.ItemNo		
                                    LEFT JOIN (
                                        SELECT  ISM.ItemNo,
		                                        STUFF((
			                                        SELECT  ', ' + SUPP.SupplierName
			                                        FROM	ItemSupplierMap IMAP
					                                        INNER JOIN Supplier SUPP ON SUPP.SupplierID = IMAP.SupplierID
			                                        WHERE	ISNULL(IMAP.Deleted,0) = 0 
					                                        AND ISNULL(SUPP.Deleted,0) = 0
					                                        AND IMAP.ItemNo = ISM.ItemNo
			                                        FOR XML PATH(''),TYPE).value('(./text())[1]','NVARCHAR(MAX)')
			                                        ,1,2,'') AS Supplier
                                        FROM	ItemSupplierMap ISM
                                        GROUP BY ISM.ItemNo
                                    ) SUPP ON SUPP.ItemNo = dbo.Item.ItemNo    
		                            LEFT OUTER JOIN dbo.OutletGroupItemMap OGIM ON OGIM.ItemNo = dbo.Item.ItemNo AND OGIM.OutletName = '{0}'
                                    LEFT OUTER JOIN (
                                        SELECT   OG.OutletGroupID
		                                        ,OU.OutletName
		                                        ,OGIM.ItemNo		
		                                        ,OGIM.RetailPrice
                                                ,OGIM.CostPrice
		                                        ,OGIM.Deleted
                                                ,OGIM.OutletGroupItemMapID
                                                ,OGIM.Userfld1
                                        FROM	OutletGroupItemMap OGIM
		                                        INNER JOIN OutletGroup OG ON OG.OutletGroupID = OGIM.OutletGroupID
		                                        INNER JOIN Outlet OU ON OU.OutletGroupID = OG.OutletGroupID
                                    ) OGIM2 ON OGIM2.ItemNo = dbo.Item.ItemNo AND OGIM2.OutletName = '{0}'";
                countQuery = @"SELECT  @TotalPageSize = COUNT(*)
                                FROM    dbo.Category 
		                                                            INNER JOIN dbo.Item ON dbo.Category.CategoryName = dbo.Item.CategoryName 
		                                                            INNER JOIN dbo.ItemDepartment ON dbo.Category.ItemDepartmentId = dbo.ItemDepartment.ItemDepartmentID
		                                                            LEFT JOIN (
			                                                            SELECT	AB.ItemNo,
					                                                            STUFF((
						                                                            SELECT ', ' + Barcode
						                                                            FROM	AlternateBarcode
						                                                            WHERE	ISNULL(Deleted,0) = 0 
								                                                            AND ItemNo = AB.ItemNo
								                                                            AND Barcode LIKE '%' + @Search + '%'  
						                                                            FOR XML PATH(''),TYPE).value('(./text())[1]','NVARCHAR(MAX)')
					                                                              ,1,2,'') AS AlternateBarcode
			                                                            FROM	AlternateBarcode AB
			                                                            WHERE	ISNULL(AB.Deleted,0) = 0
					                                                            AND AB.Barcode LIKE '%' + @Search + '%' 
			                                                            GROUP BY AB.ItemNo		
		                                                            ) ALT ON ALT.ItemNo = dbo.Item.ItemNo		
                                                                    LEFT JOIN (
                                                                        SELECT  ISM.ItemNo,
		                                                                        STUFF((
			                                                                        SELECT  ', ' + SUPP.SupplierName
			                                                                        FROM	ItemSupplierMap IMAP
					                                                                        INNER JOIN Supplier SUPP ON SUPP.SupplierID = IMAP.SupplierID
			                                                                        WHERE	ISNULL(IMAP.Deleted,0) = 0 
					                                                                        AND ISNULL(SUPP.Deleted,0) = 0
					                                                                        AND IMAP.ItemNo = ISM.ItemNo
			                                                                        FOR XML PATH(''),TYPE).value('(./text())[1]','NVARCHAR(MAX)')
			                                                                        ,1,2,'') AS Supplier
                                                                        FROM	ItemSupplierMap ISM
                                                                        GROUP BY ISM.ItemNo
                                                                    ) SUPP ON SUPP.ItemNo = dbo.Item.ItemNo    
		                                                            LEFT OUTER JOIN dbo.OutletGroupItemMap OGIM ON OGIM.ItemNo = dbo.Item.ItemNo AND ISNULL(OGIM.Deleted,0) = 0 AND OGIM.OutletName = '{0}'
                                                                    LEFT OUTER JOIN (
                                                                        SELECT   OG.OutletGroupID
		                                                                        ,OU.OutletName
		                                                                        ,OGIM.ItemNo		
		                                                                        ,OGIM.RetailPrice
		                                                                        ,OGIM.Deleted
                                                                                ,OGIM.OutletGroupItemMapID
                                                                                ,OGIM.Userfld1 
                                                                        FROM	OutletGroupItemMap OGIM
		                                                                        INNER JOIN OutletGroup OG ON OG.OutletGroupID = OGIM.OutletGroupID
		                                                                        INNER JOIN Outlet OU ON OU.OutletGroupID = OG.OutletGroupID
                                                                    ) OGIM2 ON OGIM2.ItemNo = dbo.Item.ItemNo AND OGIM2.OutletName = '{0}'";
                string itemNameQ = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.AllowOverrideItemNameOutlet), false) ? "CASE WHEN OGIM.OutletGroupItemMapID IS NULL THEN (CASE WHEN OGIM2.OutletGroupItemMapID IS NULL THEN dbo.Item.ItemName ELSE ISNULL(OGIM2.Userfld1,dbo.Item.ItemName) END) ELSE ISNULL(OGIM.Userfld1,dbo.Item.ItemName) END ItemName" : "dbo.Item.ItemName";
                innerQuery = string.Format(innerQuery, outlet, itemNameQ);
                countQuery = string.Format(countQuery, outlet);
            }

            innerQuery += @"    WHERE	1 = 1 
                                        AND (dbo.Item.ItemNo LIKE @Search 
		                                OR ISNULL(dbo.Item.ItemName,'') LIKE @Search 
		                                OR ISNULL(dbo.Item.Barcode, '') LIKE @Search
		                                OR dbo.Item.CategoryName LIKE @Search
		                                OR ISNULL(dbo.ItemDepartment.DepartmentName, '') LIKE @Search 
		                                OR ISNULL(dbo.Item.ItemDesc, '') LIKE @Search
		                                OR ISNULL(SUPP.Supplier,'') LIKE @Search
                                        OR ISNULL(ALT.AlternateBarcode,'')like '%' + @Search +'%') ";

            countQuery += @"    WHERE	1 = 1 
                                        AND (dbo.Item.ItemNo LIKE @Search 
		                                OR ISNULL(dbo.Item.ItemName,'') LIKE @Search 
		                                OR ISNULL(dbo.Item.Barcode, '') LIKE @Search
		                                OR dbo.Item.CategoryName LIKE @Search
		                                OR ISNULL(dbo.ItemDepartment.DepartmentName, '') LIKE @Search 
		                                OR ISNULL(dbo.Item.ItemDesc, '') LIKE @Search
		                                OR ISNULL(SUPP.Supplier,'') LIKE @Search
                                        OR ISNULL(ALT.AlternateBarcode,'') like '%' + @Search +'%' ) ";
            if (ItemNames != null && ItemNames.Count > 0)
            {
                string criteria = "";
                for (int i = 0; i < ItemNames.Count; i++)
                    criteria += "N" + string.Format("'{0}',", ItemNames[i].ToString());

                criteria = criteria.Remove(criteria.Length - 1);
                innerQuery += string.Format(" AND ISNULL(Item.ItemName,'') IN ({0}) ", criteria);
                countQuery += string.Format(" AND ISNULL(Item.ItemName,'') IN ({0}) ", criteria);
            }
            if (attr1 != null && attr1.Count > 0)
            {
                string criteria = "";
                for (int i = 0; i < attr1.Count; i++)
                    criteria += string.Format("'{0}',", attr1[i].ToString());

                criteria = criteria.Remove(criteria.Length - 1);
                innerQuery += string.Format(" AND ISNULL(Item.Attributes1,'') IN ({0}) ", criteria);
                countQuery += string.Format(" AND ISNULL(Item.Attributes1,'') IN ({0}) ", criteria);
            }
            if (attr2 != null && attr2.Count > 0)
            {
                string criteria = "";
                for (int i = 0; i < attr2.Count; i++)
                    criteria += string.Format("'{0}',", attr2[i].ToString());

                criteria = criteria.Remove(criteria.Length - 1);
                innerQuery += string.Format(" AND ISNULL(Item.Attributes2,'') IN ({0}) ", criteria);
                countQuery += string.Format(" AND ISNULL(Item.Attributes2,'') IN ({0}) ", criteria);
            }
            if (attr3 != null && attr3.Count > 0)
            {
                string criteria = "";
                for (int i = 0; i < attr3.Count; i++)
                    criteria += string.Format("'{0}',", attr3[i].ToString());

                criteria = criteria.Remove(criteria.Length - 1);
                innerQuery += string.Format(" AND ISNULL(Item.Attributes3,'') IN ({0}) ", criteria);
                countQuery += string.Format(" AND ISNULL(Item.Attributes3,'') IN ({0}) ", criteria);
            }
            if (attr4 != null && attr4.Count > 0)
            {
                string criteria = "";
                for (int i = 0; i < attr4.Count; i++)
                    criteria += string.Format("'{0}',", attr4[i].ToString());

                criteria = criteria.Remove(criteria.Length - 1);
                innerQuery += string.Format(" AND ISNULL(Item.Attributes4,'') IN ({0}) ", criteria);
                countQuery += string.Format(" AND ISNULL(Item.Attributes4,'') IN ({0}) ", criteria);
            }
            if (attr5 != null && attr5.Count > 0)
            {
                string criteria = "";
                for (int i = 0; i < attr5.Count; i++)
                    criteria += string.Format("'{0}',", attr5[i].ToString());

                criteria = criteria.Remove(criteria.Length - 1);
                innerQuery += string.Format(" AND ISNULL(Item.Attributes5,'') IN ({0}) ", criteria);
                countQuery += string.Format(" AND ISNULL(Item.Attributes5,'') IN ({0}) ", criteria);
            }
            if (attr6 != null && attr6.Count > 0)
            {
                string criteria = "";
                for (int i = 0; i < attr6.Count; i++)
                    criteria += string.Format("'{0}',", attr6[i].ToString());

                criteria = criteria.Remove(criteria.Length - 1);
                innerQuery += string.Format(" AND ISNULL(Item.Attributes6,'') IN ({0}) ", criteria);
                countQuery += string.Format(" AND ISNULL(Item.Attributes6,'') IN ({0}) ", criteria);
            }
            if (attr7 != null && attr7.Count > 0)
            {
                string criteria = "";
                for (int i = 0; i < attr7.Count; i++)
                    criteria += string.Format("'{0}',", attr7[i].ToString());

                criteria = criteria.Remove(criteria.Length - 1);
                innerQuery += string.Format(" AND ISNULL(Item.Attributes7,'') IN ({0}) ", criteria);
                countQuery += string.Format(" AND ISNULL(Item.Attributes7,'') IN ({0}) ", criteria);
            }
            if (attr8 != null && attr8.Count > 0)
            {
                string criteria = "";
                for (int i = 0; i < attr8.Count; i++)
                    criteria += string.Format("'{0}',", attr8[i].ToString());

                criteria = criteria.Remove(criteria.Length - 1);
                innerQuery += string.Format(" AND ISNULL(Item.Attributes8,'') IN ({0}) ", criteria);
                countQuery += string.Format(" AND ISNULL(Item.Attributes8,'') IN ({0}) ", criteria);
            }
            if (attr9 != null && attr9.Count > 0)
            {
                string criteria = "";
                for (int i = 0; i < attr9.Count; i++)
                    criteria += string.Format("'{0}',", attr9[i].ToString());

                criteria = criteria.Remove(criteria.Length - 1);
                innerQuery += string.Format(" AND ISNULL(Item.Attributes9,'') IN ({0}) ", criteria);
                countQuery += string.Format(" AND ISNULL(Item.Attributes9,'') IN ({0}) ", criteria);
            }

            QueryParameterCollection parameters = new QueryParameterCollection();
            if (category != null && category.Length != 0)
            {
                string categoryCriteria = "";
                for (int i = 0; i < category.Length; i++)
                {
                    QueryParameter param = new QueryParameter();
                    param.DataType = DbType.String;
                    param.ParameterName = string.Format("@tag{0}", i);
                    param.ParameterValue = string.Format("{0}", category[i].Trim());

                    categoryCriteria += string.Format("{0},", param.ParameterName);

                    parameters.Add(param);
                }
                categoryCriteria = categoryCriteria.Remove(categoryCriteria.Length - 1);
                innerQuery += string.Format(" AND Item.CategoryName IN({0})", categoryCriteria);
                countQuery += string.Format(" AND Item.CategoryName IN({0})", categoryCriteria);
            }


            if (IsInventoryItemOnly)
            {
                innerQuery += "AND dbo.Item.IsInInventory = 1 ";
                countQuery += "AND dbo.Item.IsInInventory = 1 ";

            }

            if (HideDeletedItem)
            {
                innerQuery += " AND ISNULL(dbo.Item.Deleted, 0) = 0 ";
                countQuery += " AND ISNULL(dbo.Item.Deleted, 0) = 0 ";
            }

            string mainQuery = @"DECLARE @PageSize INT;
                                DECLARE @PageNo INT;
                                DECLARE @StartPage INT;
                                DECLARE @EndPage INT;
                                DECLARE @TotalPageSize INT;

                                SET @PageSize = {0};
                                SET @PageNo = {1};

                                SET @StartPage = ((@PageNo-1) * @PageSize);
                                SET @EndPage = (@PageNo * @PageSize);
                                PRINT @StartPage;
                                PRINT @EndPage;

                                {2}
                                
                                SELECT  IT.*
		                                ,(@TotalPageSize/@PageSize)+1 TotalPageSize 
                                FROM	( 

                                {3}

                                ) IT 
                                WHERE	IT.No > @StartPage 
		                                AND IT.No <= @EndPage
                                ORDER BY IT.ItemNo  ";
            //string deletedQuery = "";
            //if (HideDeletedItem)
            //    deletedQuery = " AND ISNULL(IT.Deleted, 0) = 0 ";
            mainQuery = string.Format(mainQuery, pageSize
                                               , pageNo
                                               , countQuery
                                               , innerQuery
                                               );
            Logger.writeLog(">> Query Term of Product Setup : " + mainQuery);
            QueryCommand cmd = new QueryCommand(mainQuery);

            if (search == null) search = "";

            cmd.Parameters.Add("@Search", search, DbType.String);
            if (parameters.Count > 0)
            {
                for (int i = 0; i < parameters.Count; i++)
                    cmd.Parameters.Add(parameters[i]);
            }


            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(cmd));

            dt = CommonUILib.DataTableConvertBoolColumnToYesOrNo(dt);
            dt = ConvertGSTRuleToString(dt);

            if (dt.Rows.Count > 0)
                totalPageNo = (dt.Rows[0]["TotalPageSize"] + "").GetIntValue();

            return dt;
        }

        public DataTable SearchItem(object name, bool IsInventoryItemOnly, string[] category, string applicableTo, string outlet, ArrayList ItemNames, ArrayList attr1, ArrayList attr2, ArrayList attr3, ArrayList attr4, ArrayList attr5, ArrayList attr6, ArrayList attr7, ArrayList attr8, ArrayList attr9, ArrayList attr10)
        {
            string innerQuery = "";

            if (applicableTo == "Product Master")
            {
                innerQuery = @"SELECT   dbo.Category.IsForSale
		                            ,dbo.Category.IsDiscountable
		                            ,dbo.Category.CategoryName
		                            ,dbo.Category.Category_ID
		                            ,dbo.Category.IsGST
		                            ,dbo.Category.AccountCategory
		                            ,dbo.Item.ItemNo
		                            ,dbo.Item.ItemName
		                            ,dbo.Item.Barcode
		                            ,dbo.Item.RetailPrice
		                            ,dbo.Item.FactoryPrice
		                            ,dbo.Item.MinimumPrice
		                            ,dbo.Item.ItemDesc
		                            ,dbo.Item.IsInInventory
		                            ,dbo.Item.IsNonDiscountable
		                            ,dbo.Item.Brand
		                            ,dbo.Item.ProductLine
		                            ,dbo.Item.Remark
		                            ,dbo.Item.Attributes1
		                            ,dbo.Item.Attributes2
		                            ,dbo.Item.Attributes3
		                            ,dbo.Item.Attributes4
		                            ,dbo.Item.Attributes5
		                            ,dbo.Item.Attributes6
		                            ,dbo.Item.Attributes7		
		                            ,dbo.Item.Attributes8
		                            ,dbo.Category.ItemDepartmentId
		                            ,dbo.ItemDepartment.DepartmentName
		                            ,dbo.Item.IsServiceItem
		                            ,dbo.Item.IsCourse
		                            ,dbo.Item.CourseTypeID
		                            ,dbo.Item.ProductionDate
		                            ,dbo.Item.IsGST AS Expr1
		                            ,dbo.Item.hasWarranty
		                            ,dbo.Item.IsDelivery
		                            ,dbo.Item.GSTRule
		                            ,dbo.Item.IsVitaMix
		                            ,dbo.Item.IsWaterFilter
		                            ,dbo.Item.IsYoung
		                            ,dbo.Item.IsJuicePlus
		                            ,dbo.Item.IsCommission
		                            ,ISNULL(dbo.Item.userfld10,'N') AS PointType
		                            ,ISNULL(dbo.Item.userfloat1,0) AS PointAmount
		                            ,dbo.Item.Deleted
		                            ,ISNULL(dbo.Item.Userflag1,'false') as Userflag1  
                                    , ISNULL(dbo.Item.Userfld1,'') as UOM
                                    , dbo.Item.CreatedOn 
                                    , dbo.Item.CreatedBy 
                                    , ISNULL(SUPP.Supplier,'') Supplier
                            FROM    dbo.Category 
		                            INNER JOIN dbo.Item ON dbo.Category.CategoryName = dbo.Item.CategoryName 
		                            INNER JOIN dbo.ItemDepartment ON dbo.Category.ItemDepartmentId = dbo.ItemDepartment.ItemDepartmentID
                                    LEFT JOIN (
                                        SELECT  ISM.ItemNo,
		                                        STUFF((
			                                        SELECT  ', ' + SUPP.SupplierName
			                                        FROM	ItemSupplierMap IMAP
					                                        INNER JOIN Supplier SUPP ON SUPP.SupplierID = IMAP.SupplierID
			                                        WHERE	ISNULL(IMAP.Deleted,0) = 0 
					                                        AND ISNULL(SUPP.Deleted,0) = 0
					                                        AND IMAP.ItemNo = ISM.ItemNo
			                                        FOR XML PATH(''),TYPE).value('(./text())[1]','NVARCHAR(MAX)')
			                                        ,1,2,'') AS Supplier
                                        FROM	ItemSupplierMap ISM
                                        GROUP BY ISM.ItemNo
                                    ) SUPP ON SUPP.ItemNo = dbo.Item.ItemNo    ";
            }
            else if (applicableTo == "Outlet Group")
            {
                innerQuery = @"SELECT   dbo.Category.IsForSale
		                            ,dbo.Category.IsDiscountable
		                            ,dbo.Category.CategoryName
		                            ,dbo.Category.Category_ID
		                            ,dbo.Category.IsGST
		                            ,dbo.Category.AccountCategory
		                            ,dbo.Item.ItemNo
		                            ,dbo.Item.ItemName
		                            ,dbo.Item.Barcode
		                            ,CASE WHEN OGIM.OutletGroupItemMapID IS NULL THEN dbo.Item.RetailPrice ELSE OGIM.RetailPrice END RetailPrice
		                            ,dbo.Item.FactoryPrice
		                            ,dbo.Item.MinimumPrice
		                            ,dbo.Item.ItemDesc
		                            ,dbo.Item.IsInInventory
		                            ,dbo.Item.IsNonDiscountable
		                            ,dbo.Item.Brand
		                            ,dbo.Item.ProductLine
		                            ,dbo.Item.Remark
		                            ,dbo.Item.Attributes1
		                            ,dbo.Item.Attributes2
		                            ,dbo.Item.Attributes3
		                            ,dbo.Item.Attributes4
		                            ,dbo.Item.Attributes5
		                            ,dbo.Item.Attributes6
		                            ,dbo.Item.Attributes7		
		                            ,dbo.Item.Attributes8
		                            ,dbo.Category.ItemDepartmentId
		                            ,dbo.ItemDepartment.DepartmentName
		                            ,dbo.Item.IsServiceItem
		                            ,dbo.Item.IsCourse
		                            ,dbo.Item.CourseTypeID
		                            ,dbo.Item.ProductionDate
		                            ,dbo.Item.IsGST AS Expr1
		                            ,dbo.Item.hasWarranty
		                            ,dbo.Item.IsDelivery
		                            ,dbo.Item.GSTRule
		                            ,dbo.Item.IsVitaMix
		                            ,dbo.Item.IsWaterFilter
		                            ,dbo.Item.IsYoung
		                            ,dbo.Item.IsJuicePlus
		                            ,dbo.Item.IsCommission
		                            ,ISNULL(dbo.Item.userfld10,'N') AS PointType
		                            ,ISNULL(dbo.Item.userfloat1,0) AS PointAmount
		                            ,CASE WHEN OGIM.OutletGroupItemMapID IS NULL THEN dbo.Item.Deleted ELSE OGIM.IsItemDeleted END Deleted
		                            ,ISNULL(dbo.Item.Userflag1,'false') as Userflag1  
                                    ,ISNULL(dbo.Item.Userfld1,'') as UOM
                                    , dbo.Item.CreatedOn 
                                    , dbo.Item.CreatedBy 
                                    , ISNULL(SUPP.Supplier,'') Supplier
                            FROM    dbo.Category 
		                            INNER JOIN dbo.Item ON dbo.Category.CategoryName = dbo.Item.CategoryName 
		                            INNER JOIN dbo.ItemDepartment ON dbo.Category.ItemDepartmentId = dbo.ItemDepartment.ItemDepartmentID
		                            LEFT OUTER JOIN dbo.OutletGroupItemMap OGIM ON OGIM.ItemNo = dbo.Item.ItemNo AND OGIM.OutletGroupID = '{0}'
                                    LEFT JOIN (
                                        SELECT  ISM.ItemNo,
		                                        STUFF((
			                                        SELECT  ', ' + SUPP.SupplierName
			                                        FROM	ItemSupplierMap IMAP
					                                        INNER JOIN Supplier SUPP ON SUPP.SupplierID = IMAP.SupplierID
			                                        WHERE	ISNULL(IMAP.Deleted,0) = 0 
					                                        AND ISNULL(SUPP.Deleted,0) = 0
					                                        AND IMAP.ItemNo = ISM.ItemNo
			                                        FOR XML PATH(''),TYPE).value('(./text())[1]','NVARCHAR(MAX)')
			                                        ,1,2,'') AS Supplier
                                        FROM	ItemSupplierMap ISM
                                        GROUP BY ISM.ItemNo
                                    ) SUPP ON SUPP.ItemNo = dbo.Item.ItemNo    ";
                innerQuery = string.Format(innerQuery, outlet);
            }
            else if (applicableTo == "Outlet")
            {
                innerQuery = @"SELECT   dbo.Category.IsForSale
		                            ,dbo.Category.IsDiscountable
		                            ,dbo.Category.CategoryName
		                            ,dbo.Category.Category_ID
		                            ,dbo.Category.IsGST
		                            ,dbo.Category.AccountCategory
		                            ,dbo.Item.ItemNo
		                            ,dbo.Item.ItemName
		                            ,dbo.Item.Barcode
		                            ,CASE WHEN OGIM.OutletGroupItemMapID IS NULL THEN (CASE WHEN OGIM2.OutletGroupItemMapID IS NULL THEN dbo.Item.RetailPrice ELSE OGIM2.RetailPrice END) ELSE OGIM.RetailPrice END RetailPrice
		                            ,dbo.Item.FactoryPrice
		                            ,dbo.Item.MinimumPrice
		                            ,dbo.Item.ItemDesc
		                            ,dbo.Item.IsInInventory
		                            ,dbo.Item.IsNonDiscountable
		                            ,dbo.Item.Brand
		                            ,dbo.Item.ProductLine
		                            ,dbo.Item.Remark
		                            ,dbo.Item.Attributes1
		                            ,dbo.Item.Attributes2
		                            ,dbo.Item.Attributes3
		                            ,dbo.Item.Attributes4
		                            ,dbo.Item.Attributes5
		                            ,dbo.Item.Attributes6
		                            ,dbo.Item.Attributes7		
		                            ,dbo.Item.Attributes8
		                            ,dbo.Category.ItemDepartmentId
		                            ,dbo.ItemDepartment.DepartmentName
		                            ,dbo.Item.IsServiceItem
		                            ,dbo.Item.IsCourse
		                            ,dbo.Item.CourseTypeID
		                            ,dbo.Item.ProductionDate
		                            ,dbo.Item.IsGST AS Expr1
		                            ,dbo.Item.hasWarranty
		                            ,dbo.Item.IsDelivery
		                            ,dbo.Item.GSTRule
		                            ,dbo.Item.IsVitaMix
		                            ,dbo.Item.IsWaterFilter
		                            ,dbo.Item.IsYoung
		                            ,dbo.Item.IsJuicePlus
		                            ,dbo.Item.IsCommission
		                            ,ISNULL(dbo.Item.userfld10,'N') AS PointType
		                            ,ISNULL(dbo.Item.userfloat1,0) AS PointAmount
		                            ,CASE WHEN OGIM.OutletGroupItemMapID IS NULL THEN (CASE WHEN OGIM2.OutletGroupItemMapID IS NULL THEN dbo.Item.Deleted ELSE OGIM2.IsItemDeleted END) ELSE OGIM.IsItemDeleted END Deleted
		                            ,ISNULL(dbo.Item.Userflag1,'false') as Userflag1  
                                    , ISNULL(dbo.Item.Userfld1,'') as UOM
                                    , dbo.Item.CreatedOn 
                                    , dbo.Item.CreatedBy 
                                    , ISNULL(SUPP.Supplier,'') Supplier
                            FROM    dbo.Category 
		                            INNER JOIN dbo.Item ON dbo.Category.CategoryName = dbo.Item.CategoryName 
		                            INNER JOIN dbo.ItemDepartment ON dbo.Category.ItemDepartmentId = dbo.ItemDepartment.ItemDepartmentID
                                    LEFT JOIN (
                                        SELECT  ISM.ItemNo,
		                                        STUFF((
			                                        SELECT  ', ' + SUPP.SupplierName
			                                        FROM	ItemSupplierMap IMAP
					                                        INNER JOIN Supplier SUPP ON SUPP.SupplierID = IMAP.SupplierID
			                                        WHERE	ISNULL(IMAP.Deleted,0) = 0 
					                                        AND ISNULL(SUPP.Deleted,0) = 0
					                                        AND IMAP.ItemNo = ISM.ItemNo
			                                        FOR XML PATH(''),TYPE).value('(./text())[1]','NVARCHAR(MAX)')
			                                        ,1,2,'') AS Supplier
                                        FROM	ItemSupplierMap ISM
                                        GROUP BY ISM.ItemNo
                                    ) SUPP ON SUPP.ItemNo = dbo.Item.ItemNo    
		                            LEFT OUTER JOIN dbo.OutletGroupItemMap OGIM ON OGIM.ItemNo = dbo.Item.ItemNo AND OGIM.OutletName = '{0}'
                                    LEFT OUTER JOIN (
                                        SELECT   OG.OutletGroupID
		                                        ,OU.OutletName
		                                        ,OGIM.ItemNo		
		                                        ,OGIM.RetailPrice
		                                        ,OGIM.IsItemDeleted
                                                ,OGIM.OutletGroupItemMapID
                                        FROM	OutletGroupItemMap OGIM
		                                        INNER JOIN OutletGroup OG ON OG.OutletGroupID = OGIM.OutletGroupID
		                                        INNER JOIN Outlet OU ON OU.OutletGroupID = OG.OutletGroupID
                                    ) OGIM2 ON OGIM2.ItemNo = dbo.Item.ItemNo AND OGIM2.OutletName = '{0}'";
                innerQuery = string.Format(innerQuery, outlet);
            }

            innerQuery += "  WHERE 1 = 1 AND " +
                              "(dbo.Item.ItemNo + ' ' + dbo.Item.ItemName + ' ' + ISNULL(dbo.Item.Barcode, '') + ' ' + dbo.Item.CategoryName + ' ' + ISNULL(dbo.ItemDepartment.DepartmentName, '') + ' ' + ISNULL(dbo.Item.ItemDesc, '')) LIKE '%' + @Search + '%' ";

            if (ItemNames != null && ItemNames.Count > 0)
            {
                string criteria = "";
                for (int i = 0; i < ItemNames.Count; i++)
                    criteria += "N" + string.Format("'{0}',", ItemNames[i].ToString());

                criteria = criteria.Remove(criteria.Length - 1);
                innerQuery += string.Format(" AND ISNULL(Item.ItemName,'') IN ({0}) ", criteria);
            }
            if (attr1 != null && attr1.Count > 0)
            {
                string criteria = "";
                for (int i = 0; i < attr1.Count; i++)
                    criteria += string.Format("'{0}',", attr1[i].ToString());

                criteria = criteria.Remove(criteria.Length - 1);
                innerQuery += string.Format(" AND ISNULL(Item.Attributes1,'') IN ({0}) ", criteria);
            }
            if (attr2 != null && attr2.Count > 0)
            {
                string criteria = "";
                for (int i = 0; i < attr2.Count; i++)
                    criteria += string.Format("'{0}',", attr2[i].ToString());

                criteria = criteria.Remove(criteria.Length - 1);
                innerQuery += string.Format(" AND ISNULL(Item.Attributes2,'') IN ({0}) ", criteria);
            }
            if (attr3 != null && attr3.Count > 0)
            {
                string criteria = "";
                for (int i = 0; i < attr3.Count; i++)
                    criteria += string.Format("'{0}',", attr3[i].ToString());

                criteria = criteria.Remove(criteria.Length - 1);
                innerQuery += string.Format(" AND ISNULL(Item.Attributes3,'') IN ({0}) ", criteria);
            }
            if (attr4 != null && attr4.Count > 0)
            {
                string criteria = "";
                for (int i = 0; i < attr4.Count; i++)
                    criteria += string.Format("'{0}',", attr4[i].ToString());

                criteria = criteria.Remove(criteria.Length - 1);
                innerQuery += string.Format(" AND ISNULL(Item.Attributes4,'') IN ({0}) ", criteria);
            }
            if (attr5 != null && attr5.Count > 0)
            {
                string criteria = "";
                for (int i = 0; i < attr5.Count; i++)
                    criteria += string.Format("'{0}',", attr5[i].ToString());

                criteria = criteria.Remove(criteria.Length - 1);
                innerQuery += string.Format(" AND ISNULL(Item.Attributes5,'') IN ({0}) ", criteria);
            }
            if (attr6 != null && attr6.Count > 0)
            {
                string criteria = "";
                for (int i = 0; i < attr6.Count; i++)
                    criteria += string.Format("'{0}',", attr6[i].ToString());

                criteria = criteria.Remove(criteria.Length - 1);
                innerQuery += string.Format(" AND ISNULL(Item.Attributes6,'') IN ({0}) ", criteria);
            }
            if (attr7 != null && attr7.Count > 0)
            {
                string criteria = "";
                for (int i = 0; i < attr7.Count; i++)
                    criteria += string.Format("'{0}',", attr7[i].ToString());

                criteria = criteria.Remove(criteria.Length - 1);
                innerQuery += string.Format(" AND ISNULL(Item.Attributes7,'') IN ({0}) ", criteria);
            }
            if (attr8 != null && attr8.Count > 0)
            {
                string criteria = "";
                for (int i = 0; i < attr8.Count; i++)
                    criteria += string.Format("'{0}',", attr8[i].ToString());

                criteria = criteria.Remove(criteria.Length - 1);
                innerQuery += string.Format(" AND ISNULL(Item.Attributes8,'') IN ({0}) ", criteria);
            }
            if (attr9 != null && attr9.Count > 0)
            {
                string criteria = "";
                for (int i = 0; i < attr9.Count; i++)
                    criteria += string.Format("'{0}',", attr9[i].ToString());

                criteria = criteria.Remove(criteria.Length - 1);
                innerQuery += string.Format(" AND ISNULL(Item.Attributes9,'') IN ({0}) ", criteria);
            }

            QueryParameterCollection parameters = new QueryParameterCollection();
            if (category != null && category.Length != 0)
            {
                string categoryCriteria = "";
                for (int i = 0; i < category.Length; i++)
                {
                    QueryParameter param = new QueryParameter();
                    param.DataType = DbType.String;
                    param.ParameterName = string.Format("@tag{0}", i);
                    param.ParameterValue = string.Format("{0}", category[i].Trim());

                    categoryCriteria += string.Format("{0},", param.ParameterName);

                    parameters.Add(param);
                }
                categoryCriteria = categoryCriteria.Remove(categoryCriteria.Length - 1);
                innerQuery += string.Format(" AND Item.CategoryName IN({0})", categoryCriteria);
            }


            if (IsInventoryItemOnly)
                innerQuery += "AND dbo.Item.IsInInventory = 1 ";

            innerQuery += "ORDER BY ItemNo ";

            QueryCommand cmd = new QueryCommand(innerQuery);

            if (name == null) name = "";

            cmd.Parameters.Add("@Search", name, DbType.String);

            if (parameters.Count > 0)
            {
                for (int i = 0; i < parameters.Count; i++)
                    cmd.Parameters.Add(parameters[i]);
            }


            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(cmd));

            dt = CommonUILib.DataTableConvertBoolColumnToYesOrNo(dt);
            dt = ConvertGSTRuleToString(dt);
            return dt;
        }

        public DataTable SearchItem_FromOutletGroup(object name, bool IsInventoryItemOnly, string[] category, ArrayList ItemNames, ArrayList attr1, ArrayList attr2, ArrayList attr3, ArrayList attr4, ArrayList attr5, ArrayList attr6, ArrayList attr7, ArrayList attr8, ArrayList attr9, ArrayList attr10)
        {
            string QueryStr =
                "SELECT     dbo.Category.IsForSale, dbo.Category.IsDiscountable, dbo.Category.CategoryName, dbo.Category.Category_ID, dbo.Category.IsGST, dbo.Category.AccountCategory, " +
                      "dbo.Item.ItemNo, dbo.Item.ItemName, dbo.Item.Barcode, dbo.Item.RetailPrice, dbo.Item.FactoryPrice, dbo.Item.MinimumPrice, dbo.Item.ItemDesc, " +
                      "dbo.Item.IsInInventory, dbo.Item.IsNonDiscountable, dbo.Item.Brand, dbo.Item.ProductLine, dbo.Item.Remark, dbo.Item.Attributes1, " +
                      "dbo.Item.Attributes2, dbo.Item.Attributes3, dbo.Item.Attributes4, dbo.Item.Attributes5, dbo.Item.Attributes6, dbo.Item.Attributes8, dbo.Item.Attributes7, " +
                      "dbo.Category.ItemDepartmentId, dbo.ItemDepartment.DepartmentName, " +
                      "dbo.Item.IsServiceItem, dbo.Item.IsCourse, dbo.Item.CourseTypeID, dbo.Item.ProductionDate, dbo.Item.IsGST AS Expr1, dbo.Item.hasWarranty, " +
                      "dbo.Item.IsDelivery, dbo.Item.GSTRule, dbo.Item.IsVitaMix, dbo.Item.IsWaterFilter, dbo.Item.IsYoung, dbo.Item.IsJuicePlus, dbo.Item.IsCommission, ISNULL(dbo.Item.userfld10,'N') AS PointType, ISNULL(dbo.Item.userfloat1,0) AS PointAmount, dbo.Item.Deleted, ISNULL(dbo.Item.Userflag1,'false') as Userflag1, ISNULL(dbo.Item.Userfld1,'') as UOM " +
                "FROM         dbo.Category INNER JOIN " +
                      "dbo.Item ON dbo.Category.CategoryName = dbo.Item.CategoryName INNER JOIN " +
                      "dbo.ItemDepartment ON dbo.Category.ItemDepartmentId = dbo.ItemDepartment.ItemDepartmentID " +
                "WHERE (dbo.Item.Deleted = 0 OR dbo.Item.Deleted IS NULL) AND " +
                      "(dbo.Item.ItemNo + ' ' + dbo.Item.ItemName + ' ' + ISNULL(dbo.Item.Barcode, '') + ' ' + dbo.Item.CategoryName + ' ' + ISNULL(dbo.ItemDepartment.DepartmentName, '') + ' ' + ISNULL(dbo.Item.ItemDesc, '')) LIKE '%' + @Search + '%' ";

            if (ItemNames != null && ItemNames.Count > 0)
            {
                string criteria = "";
                for (int i = 0; i < ItemNames.Count; i++)
                    criteria += "N" + string.Format("'{0}',", ItemNames[i].ToString());

                criteria = criteria.Remove(criteria.Length - 1);
                QueryStr += string.Format(" AND ISNULL(Item.ItemName,'') IN ({0}) ", criteria);
            }
            if (attr1 != null && attr1.Count > 0)
            {
                string criteria = "";
                for (int i = 0; i < attr1.Count; i++)
                    criteria += string.Format("'{0}',", attr1[i].ToString());

                criteria = criteria.Remove(criteria.Length - 1);
                QueryStr += string.Format(" AND ISNULL(Item.Attributes1,'') IN ({0}) ", criteria);
            }
            if (attr2 != null && attr2.Count > 0)
            {
                string criteria = "";
                for (int i = 0; i < attr2.Count; i++)
                    criteria += string.Format("'{0}',", attr2[i].ToString());

                criteria = criteria.Remove(criteria.Length - 1);
                QueryStr += string.Format(" AND ISNULL(Item.Attributes2,'') IN ({0}) ", criteria);
            }
            if (attr3 != null && attr3.Count > 0)
            {
                string criteria = "";
                for (int i = 0; i < attr3.Count; i++)
                    criteria += string.Format("'{0}',", attr3[i].ToString());

                criteria = criteria.Remove(criteria.Length - 1);
                QueryStr += string.Format(" AND ISNULL(Item.Attributes3,'') IN ({0}) ", criteria);
            }
            if (attr4 != null && attr4.Count > 0)
            {
                string criteria = "";
                for (int i = 0; i < attr4.Count; i++)
                    criteria += string.Format("'{0}',", attr4[i].ToString());

                criteria = criteria.Remove(criteria.Length - 1);
                QueryStr += string.Format(" AND ISNULL(Item.Attributes4,'') IN ({0}) ", criteria);
            }
            if (attr5 != null && attr5.Count > 0)
            {
                string criteria = "";
                for (int i = 0; i < attr5.Count; i++)
                    criteria += string.Format("'{0}',", attr5[i].ToString());

                criteria = criteria.Remove(criteria.Length - 1);
                QueryStr += string.Format(" AND ISNULL(Item.Attributes5,'') IN ({0}) ", criteria);
            }
            if (attr6 != null && attr6.Count > 0)
            {
                string criteria = "";
                for (int i = 0; i < attr6.Count; i++)
                    criteria += string.Format("'{0}',", attr6[i].ToString());

                criteria = criteria.Remove(criteria.Length - 1);
                QueryStr += string.Format(" AND ISNULL(Item.Attributes6,'') IN ({0}) ", criteria);
            }
            if (attr7 != null && attr7.Count > 0)
            {
                string criteria = "";
                for (int i = 0; i < attr7.Count; i++)
                    criteria += string.Format("'{0}',", attr7[i].ToString());

                criteria = criteria.Remove(criteria.Length - 1);
                QueryStr += string.Format(" AND ISNULL(Item.Attributes7,'') IN ({0}) ", criteria);
            }
            if (attr8 != null && attr8.Count > 0)
            {
                string criteria = "";
                for (int i = 0; i < attr8.Count; i++)
                    criteria += string.Format("'{0}',", attr8[i].ToString());

                criteria = criteria.Remove(criteria.Length - 1);
                QueryStr += string.Format(" AND ISNULL(Item.Attributes8,'') IN ({0}) ", criteria);
            }
            if (attr9 != null && attr9.Count > 0)
            {
                string criteria = "";
                for (int i = 0; i < attr9.Count; i++)
                    criteria += string.Format("'{0}',", attr9[i].ToString());

                criteria = criteria.Remove(criteria.Length - 1);
                QueryStr += string.Format(" AND ISNULL(Item.Attributes9,'') IN ({0}) ", criteria);
            }

            QueryParameterCollection parameters = new QueryParameterCollection();
            if (category != null && category.Length != 0)
            {
                string categoryCriteria = "";
                for (int i = 0; i < category.Length; i++)
                {
                    QueryParameter param = new QueryParameter();
                    param.DataType = DbType.String;
                    param.ParameterName = string.Format("@tag{0}", i);
                    param.ParameterValue = string.Format("{0}", category[i].Trim());

                    categoryCriteria += string.Format("{0},", param.ParameterName);

                    parameters.Add(param);
                }
                categoryCriteria = categoryCriteria.Remove(categoryCriteria.Length - 1);
                QueryStr += string.Format(" AND Item.CategoryName IN({0})", categoryCriteria);
            }


            if (IsInventoryItemOnly)
                QueryStr += "AND dbo.Item.IsInInventory = 1 ";

            QueryStr += "ORDER BY ItemNo ";

            QueryCommand cmd = new QueryCommand(QueryStr);

            if (name == null) name = "";

            cmd.Parameters.Add("@Search", name, DbType.String);

            if (parameters.Count > 0)
            {
                for (int i = 0; i < parameters.Count; i++)
                    cmd.Parameters.Add(parameters[i]);
            }


            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(cmd));

            dt = CommonUILib.DataTableConvertBoolColumnToYesOrNo(dt);
            dt = ConvertGSTRuleToString(dt);
            return dt;
        }

        public static ListItem[] FetchCategoryNamesInListItem()
        {
            ListItemCollection ar = new ListItemCollection();
            ListItem[] lrList;
            ListItem lr = new ListItem();

            lr.Text = "ALL";
            lr.Value = "";
            ar.Add(lr);

            CategoryCollection col = new CategoryCollection();
            col.Where(Category.Columns.Deleted, false);
            col.Load();
            /*qr.SelectList = Category.Columns.CategoryName;
            qr.WHERE("Deleted = 0");*/

            /*IDataReader rdr = qr.ExecuteReader();
            while (rdr.Read())
            {
                lr = new ListItem();
                lr.Text = rdr.GetValue(0).ToString();
                lr.Value = rdr.GetValue(0).ToString();
                ar.Add(lr);
            }*/
            foreach (Category cat in col)
            {
                lr = new ListItem();
                lr.Text = cat.CategoryName;
                lr.Value = cat.CategoryName;
                ar.Add(lr);
            }
            lrList = new ListItem[ar.Count];
            for (int i = 0; i < ar.Count; i++)
            {
                lrList[i] = ar[i];
            }
            //rdr.Close();
            return lrList;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public ViewItem FetchByBarcode(object barcode)
        {
            string SQL = "select top 1 isnull(itemno,'') from item where barcode = '" + barcode + "' and deleted=0";
            QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
            object obj = DataService.ExecuteScalar(cmd);
            if (obj != null && obj.ToString() != "")
            {
                return new ViewItem(ViewItem.Columns.ItemNo, obj.ToString());
            }
            else
            {
                SQL = "select top 1 isnull(itemno,'') from item where itemno = '" + barcode + "' and deleted=0";
                cmd = new QueryCommand(SQL, "PowerPOS");
                obj = DataService.ExecuteScalar(cmd);
                if (obj != null && obj.ToString() != "")
                {
                    return new ViewItem(ViewItem.Columns.ItemNo, obj.ToString());
                }
                else
                {
                    return new ViewItem();
                }
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public ViewItem FetchByBarcode(object barcode, out bool IsItemNo)
        {

            IsItemNo = false;
            string SQL = "select top 1 isnull(itemno,'') from item where barcode = '" + barcode + "' and deleted=0";
            QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
            object obj = DataService.ExecuteScalar(cmd);
            if (obj != null && obj.ToString() != "")
            {
                return new ViewItem(ViewItem.Columns.ItemNo, obj.ToString());
            }
            else
            {
                SQL = "select top 1 isnull(itemno,'') from item where itemno = '" + barcode + "' and deleted=0";
                cmd = new QueryCommand(SQL, "PowerPOS");
                obj = DataService.ExecuteScalar(cmd);
                if (obj != null && obj.ToString() != "")
                {
                    IsItemNo = true;
                    return new ViewItem(ViewItem.Columns.ItemNo, obj.ToString());
                }
                else
                {
                    return new ViewItem();
                }
            }
        }

        public bool CheckIfBarcodeExists(string Barcode, string ExcludedItemNo)
        {
            if (Barcode == null || ExcludedItemNo == null || Barcode == "")
                return false;

            string sqlString =
                "DECLARE @Barcode VARCHAR(50); " +
                "DECLARE @ItemNo VARCHAR(50); " +
                "SET @Barcode = '" + Barcode + "' " +
                "SET @ItemNo = '" + ExcludedItemNo + "' " +
                "SELECT DISTINCT * FROM (" +
                    "SELECT Barcode FROM AlternateBarcode WHERE ItemNo <> @ItemNo AND Barcode = @Barcode AND ISNULL(Deleted,0) = 0 " +
                    "UNION ALL " +
                    "SELECT Barcode FROM Item WHERE ItemNo <> @ItemNo AND Barcode = @Barcode AND Deleted = 0 " +
                    "UNION ALL " +
                    "SELECT Barcode FROM PromoCampaignHdr WHERE Barcode = @Barcode and ISNULL(Deleted,0) = 0 " +
                    "UNION ALL " +
                    "SELECT PromoCode FROM PromoCampaignHdr WHERE PromoCode = @Barcode and ISNULL(Deleted,0) = 0 " +
                    "UNION ALL " +
                    "SELECT barcode FROM ItemGroup WHERE Barcode = @Barcode and ISNULL(Deleted,0) = 0 " +
                ") A ";

            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(new QueryCommand(sqlString)));

            Logger.writeLog(sqlString);

            return dt.Rows.Count > 0;
        }

        public bool CheckIfBarcodeExists(string Barcode, string ExcludedItemNo, out string ItemNo)
        {
            ItemNo = "";
            if (Barcode == null || ExcludedItemNo == null || Barcode == "")
                return false;

            string sqlString =
                "DECLARE @Barcode VARCHAR(50); " +
                "DECLARE @ItemNo VARCHAR(50); " +
                "SET @Barcode = '" + Barcode + "' " +
                "SET @ItemNo = '" + ExcludedItemNo + "' " +
                "SELECT DISTINCT * FROM (" +
                    "SELECT Barcode, ItemNo FROM AlternateBarcode WHERE ItemNo <> @ItemNo AND Barcode = @Barcode AND ISNULL(Deleted,0) = 0 " +
                    "UNION ALL " +
                    "SELECT Barcode, ItemNo FROM Item WHERE ItemNo <> @ItemNo AND Barcode = @Barcode AND Deleted = 0 " +
                    "UNION ALL " +
                    "SELECT Barcode, PromoCampaignName as ItemNo FROM PromoCampaignHdr WHERE Barcode = @Barcode and ISNULL(Deleted,0) = 0 " +
                    "UNION ALL " +
                    "SELECT PromoCode, PromoCampaignName as ItemNo FROM PromoCampaignHdr WHERE PromoCode = @Barcode and ISNULL(Deleted,0) = 0 " +
                    "UNION ALL " +
                    "SELECT barcode, ItemGroupName as ItemNo FROM ItemGroup WHERE Barcode = @Barcode and ISNULL(Deleted,0) = 0 " +
                ") A ";

            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(new QueryCommand(sqlString)));

            if (dt.Rows.Count > 0)
                ItemNo = (string) dt.Rows[0]["ItemNo"];

            return dt.Rows.Count > 0;
        }

        public bool CheckIfBarcodeExistsMatrix(string Barcode, string ExcludedItemNo)
        {
            if (Barcode == null || ExcludedItemNo == null || Barcode == "")
                return false;

            string sqlString =
                "DECLARE @Barcode VARCHAR(50); " +
                "DECLARE @ItemNo VARCHAR(50); " +
                "SET @Barcode = '" + Barcode + "' " +
                "SET @ItemNo = '" + ExcludedItemNo + "%' " +
                "SELECT DISTINCT * FROM (" +
                    "SELECT ItemNo FROM AlternateBarcode WHERE ItemNo NOT LIKE @ItemNo AND Barcode = @Barcode AND ISNULL(Deleted,0) = 0 " +
                    "UNION ALL " +
                    "SELECT ItemNo FROM Item WHERE ItemNo NOT LIKE @ItemNo AND Barcode = @Barcode AND Deleted = 0 " +
                ") A ";

            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(new QueryCommand(sqlString)));

            return dt.Rows.Count > 0;
        }

        public bool CheckIfItemNoExists(string ExcludedItemNo)
        {
            if (ExcludedItemNo == null || ExcludedItemNo == "")
                return false;

            string sqlString =
                "DECLARE @ItemNo VARCHAR(50); " +
                "SET @ItemNo = '" + ExcludedItemNo + "' " +
                "SELECT DISTINCT * FROM (" +
                    "SELECT ItemNo FROM Item WHERE ItemNo = @ItemNo" +
                ") A ";

            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(new QueryCommand(sqlString)));

            return dt.Rows.Count > 0;
        }

        public bool CheckIfUOMExist(string UOM, string MainItemNo)
        {
            string query = @"Select * from item where Userfld1 = '{0}' and ISNULL(Deleted,0) = 0 and (itemno = '{1}' or ISNULL(userfld8,'') = '{1}')";
            query = string.Format(query, UOM, MainItemNo);

            DataTable dt = DataService.GetDataSet(new QueryCommand(query)).Tables[0];

            return dt.Rows.Count > 0;
        }

        public bool CheckIfMatrixItemExist(string ParentItemNo, string Attributes3, string Attributes4)
        {
            string query = @"SELECT * FROM Item WHERE ISNULL(Deleted,0) = 0 AND Attributes1 = '{0}' and Attributes3 = '{1}' and Attributes4 = '{2}'";
            query = string.Format(query, ParentItemNo, Attributes3, Attributes4);

            DataTable dt = DataService.GetDataSet(new QueryCommand(query)).Tables[0];

            return dt.Rows.Count > 0;
        }

        public bool IsAlternateBarcode(string barcode, string itemno)
        {
            AlternateBarcodeCollection col = new AlternateBarcodeCollection();
            col.Where(AlternateBarcode.Columns.ItemNo, itemno);
            col.Where(AlternateBarcode.Columns.Barcode, barcode);
            col.Where(AlternateBarcode.Columns.Deleted, false);

            col.Load();

            return col.Count > 0;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public Item FetchByItemNo(object itemno)
        {
            Item myItem = new Item(itemno);

            return myItem;
        }

        public ItemCollection FetchInventoryByBarcode(object barcode)
        {
            ItemCollection coll = new ItemCollection().
                Where(Item.Columns.Barcode, barcode).
                Where(Item.Columns.IsInInventory, true).
                Where(Item.Columns.Deleted, false).Load();

            return coll;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public ViewItemCollection FetchByName(object name)
        {
            ArrayList itemNos = new ArrayList();

            ViewItemCollection coll1 = new ViewItemCollection().
                Where(ViewItem.Columns.ItemName, Comparison.Equals, name).
                Where(ViewItem.Columns.IsForSale, true).
                Where(ViewItem.Columns.Deleted, false).Load();
            coll1.Sort(ViewItem.Columns.ItemName, true);

            for (int i = 0; i < coll1.Count; i++)
            {
                itemNos.Add(coll1[i].ItemNo);
            }

            Query qr = ViewItem.CreateQuery();
            qr.AddWhere(ViewItem.Columns.ItemName, Comparison.Like, name + "%");
            qr.AddWhere(ViewItem.Columns.IsForSale, true);
            qr.AddWhere(ViewItem.Columns.Deleted, false);
            DataTable dt;
            if (itemNos.Count > 0)
            {
                dt = qr.NOT_IN(Item.Columns.ItemNo, itemNos).ExecuteDataSet().Tables[0];
            }
            else
            {
                dt = qr.ExecuteDataSet().Tables[0];
            }
            coll1.Load(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                itemNos.Add(dt.Rows[i]["ItemNo"]);
            }

            qr = ViewItem.CreateQuery();
            qr.AddWhere(ViewItem.Columns.ItemName, Comparison.Like, "%" + name.ToString().Replace(" ", "%") + "%");
            qr.AddWhere(ViewItem.Columns.IsForSale, true);
            qr.AddWhere(ViewItem.Columns.Deleted, false);
            if (itemNos.Count > 0)
            {
                dt = qr.NOT_IN(Item.Columns.ItemNo, itemNos).ORDER_BY(ViewItem.Columns.ItemName, "ASC").ExecuteDataSet().Tables[0];
            }
            else
            {
                dt = qr.ORDER_BY(ViewItem.Columns.ItemName, "ASC").ExecuteDataSet().Tables[0];
            }
            coll1.Load(dt);

            return coll1;
        }

        public ItemCollection FetchInventoryItemsByName(object name)
        {
            ItemCollection coll = new ItemCollection().Where(Item.Columns.ItemName, Comparison.Like, name + "%").
                Where(Item.Columns.IsInInventory, true).Where(Item.Columns.Deleted, false).Load();
            return coll;
        }

        public static ArrayList FetchItemNames()
        {
            ArrayList ar = new ArrayList();
            ar.Add(" ");
            Query qr = Item.CreateQuery();
            qr.QueryType = QueryType.Select;
            qr.AddWhere("Deleted", Comparison.NotEquals, true);
            qr.SelectList = Item.Columns.ItemName;
            qr.OrderBy = OrderBy.Asc("ItemName");
            IDataReader rdr = qr.ExecuteReader();
            while (rdr.Read())
            {
                ar.Add(rdr.GetValue(0).ToString());
            }
            rdr.Close();
            return ar;
        }

        public static ArrayList FetchInventoryItemNames()
        {
            ArrayList ar = new ArrayList();
            ar.Add("");

            /*IDataReader rdr = SPs.FetchItemNameListByPointOfSaleID(PointOfSaleID).GetReader();*/

            Query qr = Item.CreateQuery();
            qr.QueryType = QueryType.Select;
            qr.AddWhere("Deleted", false);
            qr.SelectList = Item.Columns.ItemName;

            IDataReader rdr = qr.WHERE(Item.Columns.IsInInventory, true).ExecuteReader();
            while (rdr.Read())
            {
                ar.Add(rdr.GetValue(0).ToString());
            }
            rdr.Close();
            return ar;
        }

        public static Item GetDataItemMatrix(string itemno)
        {
            DataSet rdr = SPs.GetItemMatrix(itemno).GetDataSet();
            DataRow row = rdr.Tables[0].Rows[0];

            var objreturn = new Item();

            objreturn.ItemNo = row["ItemNo"].ToString();
            objreturn.ItemName = row["ItemName"].ToString();
            objreturn.Barcode = row["Barcode"].ToString();
            objreturn.CategoryName = row["CategoryName"].ToString();
            objreturn.RetailPrice = Convert.ToDecimal(row["RetailPrice"] == DBNull.Value ? "0" : row["RetailPrice"]);
            objreturn.FactoryPrice = Convert.ToDecimal(row["FactoryPrice"] == DBNull.Value ? "0" : row["FactoryPrice"]);
            objreturn.MinimumPrice = Convert.ToDecimal(row["MinimumPrice"] == DBNull.Value ? "0" : row["MinimumPrice"]);
            objreturn.ItemDesc = row["ItemDesc"].ToString();
            objreturn.IsServiceItem = Convert.ToBoolean(row["IsServiceItem"] == DBNull.Value ? "false" : row["IsServiceItem"]);
            objreturn.IsInInventory = Convert.ToBoolean(row["IsInInventory"] == DBNull.Value ? "false" : row["IsInInventory"]);
            objreturn.IsNonDiscountable = Convert.ToBoolean(row["IsNonDiscountable"] == DBNull.Value ? "false" : row["IsNonDiscountable"]);
            objreturn.IsCourse = Convert.ToBoolean(row["IsCourse"] == DBNull.Value ? "false" : row["IsCourse"]);
            objreturn.Deleted = Convert.ToBoolean(row["Deleted"] == DBNull.Value ? "false" : row["Deleted"]);
            objreturn.CourseTypeID = row["CourseTypeID"].ToString();
            objreturn.Brand = row["Brand"].ToString();
            objreturn.ProductLine = row["ProductLine"].ToString();
            objreturn.Attributes1 = row["Attributes1"].ToString();
            objreturn.Attributes2 = row["Attributes2"].ToString();
            objreturn.Attributes3 = row["Attributes3"].ToString();
            objreturn.Attributes4 = row["Attributes4"].ToString();
            objreturn.Attributes5 = row["Attributes5"].ToString();
            objreturn.Attributes6 = row["Attributes6"].ToString();
            objreturn.Attributes7 = row["Attributes7"].ToString();
            objreturn.Attributes8 = row["Attributes8"].ToString();
            objreturn.Remark = row["Remark"].ToString();
            objreturn.Userflag1 = Convert.ToBoolean(row["Userflag1"] == DBNull.Value ? "false" : row["Userflag1"]);
            objreturn.IsDelivery = Convert.ToBoolean(row["IsDelivery"] == DBNull.Value ? "false" : row["IsDelivery"]);
            objreturn.GSTRule = Convert.ToInt32(row["GSTRule"] == DBNull.Value ? "0" : row["GSTRule"]);
            objreturn.IsVitaMix = Convert.ToBoolean(row["IsVitaMix"] == DBNull.Value ? "false" : row["IsVitaMix"]);
            objreturn.IsWaterFilter = Convert.ToBoolean(row["IsWaterFilter"] == DBNull.Value ? "false" : row["IsWaterFilter"]);
            objreturn.IsYoung = Convert.ToBoolean(row["IsYoung"] == DBNull.Value ? "false" : row["IsYoung"]);
            objreturn.IsJuicePlus = Convert.ToBoolean(row["IsJuicePlus"] == DBNull.Value ? "false" : row["IsJuicePlus"]);
            objreturn.IsCommission = Convert.ToBoolean(row["IsCommission"] == DBNull.Value ? "false" : row["IsCommission"]);

            return objreturn;
        }

        public static Item GetDataItemMatrixWithAttributes1(string attributes1)
        {
            ItemController itemlogic = new ItemController();
            Query qr = Item.CreateQuery();
            qr.QueryType = QueryType.Select;
            qr.AddWhere("Attributes1", Comparison.Equals, attributes1);
            qr.AddWhere("Deleted", Comparison.Equals, false);
            qr.OrderBy = OrderBy.Asc("ItemNo");
            ItemCollection itemCollect = itemlogic.FetchByQuery(qr);
            string itemno = "";
            if (itemCollect != null && itemCollect.Count > 0)
                itemno = itemCollect[0].ItemNo;

            DataSet rdr = SPs.GetItemMatrix(itemno).GetDataSet();
            DataRow row = rdr.Tables[0].Rows[0];

            var objreturn = new Item();

            objreturn.ItemNo = row["ItemNo"].ToString();
            objreturn.ItemName = row["ItemName"].ToString();
            objreturn.Barcode = row["Barcode"].ToString();
            objreturn.CategoryName = row["CategoryName"].ToString();
            objreturn.RetailPrice = Convert.ToDecimal(row["RetailPrice"] == DBNull.Value ? "0" : row["RetailPrice"]);
            objreturn.FactoryPrice = Convert.ToDecimal(row["FactoryPrice"] == DBNull.Value ? "0" : row["FactoryPrice"]);
            objreturn.MinimumPrice = Convert.ToDecimal(row["MinimumPrice"] == DBNull.Value ? "0" : row["MinimumPrice"]);
            objreturn.ItemDesc = row["ItemDesc"].ToString();
            objreturn.IsServiceItem = Convert.ToBoolean(row["IsServiceItem"] == DBNull.Value ? "false" : row["IsServiceItem"]);
            objreturn.IsInInventory = Convert.ToBoolean(row["IsInInventory"] == DBNull.Value ? "false" : row["IsInInventory"]);
            objreturn.IsNonDiscountable = Convert.ToBoolean(row["IsNonDiscountable"] == DBNull.Value ? "false" : row["IsNonDiscountable"]);
            objreturn.IsCourse = Convert.ToBoolean(row["IsCourse"] == DBNull.Value ? "false" : row["IsCourse"]);
            objreturn.CourseTypeID = row["CourseTypeID"].ToString();
            objreturn.Brand = row["Brand"].ToString();
            objreturn.ProductLine = row["ProductLine"].ToString();
            objreturn.Attributes1 = row["Attributes1"].ToString();
            objreturn.Attributes2 = row["Attributes2"].ToString();
            objreturn.Attributes3 = row["Attributes3"].ToString();
            objreturn.Attributes4 = row["Attributes4"].ToString();
            objreturn.Attributes5 = row["Attributes5"].ToString();
            objreturn.Attributes6 = row["Attributes6"].ToString();
            objreturn.Attributes7 = row["Attributes7"].ToString();
            objreturn.Attributes8 = row["Attributes8"].ToString();
            objreturn.Remark = row["Remark"].ToString();
            objreturn.Userflag1 = Convert.ToBoolean(row["Userflag1"] == DBNull.Value ? "false" : row["Userflag1"]);
            objreturn.IsDelivery = Convert.ToBoolean(row["IsDelivery"] == DBNull.Value ? "false" : row["IsDelivery"]);
            objreturn.GSTRule = Convert.ToInt32(row["GSTRule"] == DBNull.Value ? "0" : row["GSTRule"]);
            objreturn.IsVitaMix = Convert.ToBoolean(row["IsVitaMix"] == DBNull.Value ? "false" : row["IsVitaMix"]);
            objreturn.IsWaterFilter = Convert.ToBoolean(row["IsWaterFilter"] == DBNull.Value ? "false" : row["IsWaterFilter"]);
            objreturn.IsYoung = Convert.ToBoolean(row["IsYoung"] == DBNull.Value ? "false" : row["IsYoung"]);
            objreturn.IsJuicePlus = Convert.ToBoolean(row["IsJuicePlus"] == DBNull.Value ? "false" : row["IsJuicePlus"]);
            objreturn.IsCommission = Convert.ToBoolean(row["IsCommission"] == DBNull.Value ? "false" : row["IsCommission"]);

            return objreturn;
        }

        public static ArrayList FetchCategoryNames(string DeptID)
        {
            ArrayList ar = new ArrayList();
            ar.Add("");
            Query qr = Category.CreateQuery();
            qr.QueryType = QueryType.Select;
            qr.SelectList = Category.Columns.CategoryName;
            qr.AddWhere(Category.Columns.Deleted, false);
            qr.AddWhere(Category.Columns.ItemDepartmentId, DeptID);
            IDataReader rdr = qr.ExecuteReader();
            while (rdr.Read())
            {
                ar.Add(rdr.GetValue(0).ToString());
            }
            rdr.Close();
            return ar;
        }

        public static ArrayList FetchCategoryNamesNonDeleted()
        {
            ArrayList ar = new ArrayList();
            ar.Add("");
            Query qr = Category.CreateQuery();
            qr.QueryType = QueryType.Select;
            qr.AddWhere(Category.Columns.Deleted, false);
            qr.SelectList = Category.Columns.CategoryName;
            IDataReader rdr = qr.ExecuteReader();
            while (rdr.Read())
            {
                ar.Add(rdr.GetValue(0).ToString());
            }
            rdr.Close();
            return ar;
        }

        public static ArrayList FetchCategoryNames()
        {
            ArrayList ar = new ArrayList();
            ar.Add("");
            Query qr = Category.CreateQuery();
            qr.QueryType = QueryType.Select;
            qr.SelectList = Category.Columns.CategoryName;
            qr.AddWhere(Category.Columns.Deleted, false);
            IDataReader rdr = qr.ExecuteReader();
            while (rdr.Read())
            {
                ar.Add(rdr.GetValue(0).ToString());
            }
            rdr.Close();
            return ar;
        }

        public static DataTable FetchItems
            (string myItemNo, string myItemName, string myCategoryName,
             string myBarcode, string myBrand, string myItemDesc,
             string Attributes1, string Attributes2, string Attributes4, string Attributes3,
             string Attributes5)
        {
            ItemCollection myItem = new ItemCollection();
            if (myItemNo != "")
            {
                myItem.Where(Item.Columns.ItemNo, myItemNo);
            }
            else
            {
                if (myCategoryName != "")
                {
                    myItem.Where(Item.Columns.CategoryName, SubSonic.Comparison.Equals, myCategoryName);
                }

                if (myItemName != "")
                {
                    myItem.Where(Item.Columns.ItemName, SubSonic.Comparison.Like, "%" + myItemName + "%");
                }
                if (Attributes1 != "")
                {
                    myItem.Where(Item.Columns.Attributes1, SubSonic.Comparison.Like, "%" + Attributes1 + "%");
                }
                if (Attributes2 != "")
                {
                    myItem.Where(Item.Columns.Attributes2, SubSonic.Comparison.Like, "%" + Attributes2 + "%");
                }
                if (Attributes4 != "")
                {
                    myItem.Where(Item.Columns.Attributes3, SubSonic.Comparison.Like, "%" + Attributes4 + "%");
                }
                if (Attributes3 != "")
                {
                    myItem.Where(Item.Columns.Attributes4, SubSonic.Comparison.Like, "%" + Attributes3 + "%");
                }
                if (Attributes5 != "")
                {
                    myItem.Where(Item.Columns.Attributes5, SubSonic.Comparison.Like, "%" + Attributes5 + "%");
                }

                if (myBrand != "")
                {
                    myItem.Where(Item.Columns.Brand, SubSonic.Comparison.Like, "%" + myBrand + "%");
                }

                if (myBarcode != "")
                {
                    myItem.Where(Item.Columns.Barcode, SubSonic.Comparison.Equals, myBarcode);
                }

                if (myItemDesc != "")
                {
                    myItem.Where(Item.Columns.ItemDesc, SubSonic.Comparison.Like, "%" + myItemDesc + "%");
                }
            }
            myItem.Where(Item.Columns.CategoryName, Comparison.NotEquals, "SYSTEM");
            myItem.OrderByDesc(Item.Columns.ModifiedOn);
            return myItem.Load().ToDataTable();
        }

        public static DataTable FetchItemsContainsItemNo
            (string myItemNo)
        {
            ItemCollection myItem = new ItemCollection();
            if (myItemNo != "")
            {
                myItem.Where(Item.Columns.ItemNo, SubSonic.Comparison.Like, "%" + myItemNo + "%");
            }

            myItem.Where(Item.Columns.Deleted, Comparison.Equals, false);
            myItem.Where(Item.Columns.CategoryName, Comparison.NotEquals, "SYSTEM");
            myItem.OrderByDesc(Item.Columns.ModifiedOn);
            return myItem.Load().ToDataTable();
        }

        public static string getNewItemRefNo()
        {
            double runningNo = 0; //runningNo =  (int)SPs.GetMaxItemRefNo().ExecuteScalar();            

            IDataReader rdr = SPs.GetMaxItemRefNo(PointOfSaleInfo.PointOfSaleID).GetReader();
            while (rdr.Read())
            {
                if (!double.TryParse(rdr.GetValue(0).ToString(), out runningNo))
                {
                    runningNo = 0;
                }
            }
            rdr.Close();
            runningNo += 1;
            return 'I' + PointOfSaleInfo.PointOfSaleID.ToString().PadLeft(3, '0') + runningNo.ToString().PadLeft(4, '0');
        }

        public static int getNewRunningNumberMatrix(string Attributes1)
        {
            int runningNo = 0; //runningNo =  (int)SPs.GetMaxItemRefNo().ExecuteScalar();            

            IDataReader rdr = SPs.GetMaxItemNoMatrix(Attributes1).GetReader();
            while (rdr.Read())
            {
                if (!int.TryParse(rdr.GetValue(0).ToString(), out runningNo))
                {
                    runningNo = 0;
                }
            }
            rdr.Close();
            runningNo += 1;
            return runningNo;
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public void Update(string ItemNo, string ItemName,
            string Barcode, string CategoryName, decimal RetailPrice,
            decimal FactoryPrice, string ItemDesc, bool? IsInInventory, string Brand,
            string Remark, string ModifiedBy,
            bool IsNonDiscountable, string Attributes1, string Attributes2,
            string Attributes3, string Attributes4,
            string Attributes5, string ProductLine)
        {
            Query qry = new Query("Item");
            qry.AddUpdateSetting(Item.Columns.ItemName, ItemName);
            qry.AddUpdateSetting(Item.Columns.Barcode, Barcode);
            qry.AddUpdateSetting(Item.Columns.CategoryName, CategoryName);
            qry.AddUpdateSetting(Item.Columns.RetailPrice, RetailPrice);
            qry.AddUpdateSetting(Item.Columns.FactoryPrice, FactoryPrice);
            qry.AddUpdateSetting(Item.Columns.ItemDesc, ItemDesc);
            qry.AddUpdateSetting(Item.Columns.IsInInventory, IsInInventory);
            qry.AddUpdateSetting(Item.Columns.Brand, Brand);
            qry.AddUpdateSetting(Item.Columns.Remark, Remark);
            qry.AddUpdateSetting(Item.Columns.IsNonDiscountable, IsNonDiscountable);
            qry.AddUpdateSetting(Item.Columns.Attributes1, Attributes1);
            qry.AddUpdateSetting(Item.Columns.Attributes2, Attributes2);
            qry.AddUpdateSetting(Item.Columns.Attributes4, Attributes4);
            qry.AddUpdateSetting(Item.Columns.Attributes3, Attributes3);
            qry.AddUpdateSetting(Item.Columns.Attributes5, Attributes5);
            qry.AddUpdateSetting(Item.Columns.ProductLine, ProductLine);
            qry.AddUpdateSetting(Item.Columns.ModifiedOn, DateTime.Now);
            qry.AddWhere(Item.Columns.ItemNo, ItemNo);
            qry.Execute();

        }

        public void UploadPicture(string itemno, byte[] picture, string PictureExtention)
        {
            /*
            Item item = new Item(itemno);
            item.Picture = picture;
            item.HasPicture = true;
            item.PictureExtension = PictureExtention;
            item.Save();*/
        }

        /// <summary>
        /// Update Item Table when Get Point and Break Dowm Price  Value reach at 1
        /// </summary>
        /// <param name="TimeGet"> Time get point value</param>
        /// <param name="BreakDownPrice"> Break Down Price value</param>
        /// <param name="ItemNo"> Item No Value</param>
        /// <returns></returns>
        public string UpDateItemPoint(Decimal TimeGet, Decimal BreakDownPrice, string ItemNo)
        {
            String MyUpDateQuery = "UpDate Item Set [userfloat1]=@userfloat1,userfloat3=@userfloat3 Where [ItemNo]=@ItemNo";
            QueryCommand Qcmd = new QueryCommand(MyUpDateQuery);
            Qcmd.AddParameter("@userfloat1", TimeGet, DbType.Decimal);
            Qcmd.AddParameter("@userfloat3", BreakDownPrice, DbType.Decimal);
            Qcmd.AddParameter("@ItemNo", ItemNo, DbType.String);
            String str = DataService.ExecuteQuery(Qcmd).ToString();
            return str;
        }

        public string Insert(string itemno, string ItemName, string Barcode,
            string CategoryName, decimal RetailPrice,
            decimal FactoryPrice, string ItemDesc,
            bool IsInInventory, string Brand, string Remark,
            bool IsNonDiscountable, string Attributes1, string Attributes2,
            string Attributes3, string Attributes4,
            string Attributes5, string ProductLine, bool AutoGenerateID)
        {
            Item item = new Item();
            if (AutoGenerateID)
            {
                item.ItemNo = ItemController.getNewItemRefNo();
            }
            else
            {
                item.ItemNo = itemno;
            }
            item.ItemName = ItemName;
            item.Barcode = Barcode;
            item.CategoryName = CategoryName;
            item.RetailPrice = RetailPrice;
            item.FactoryPrice = FactoryPrice;
            item.ItemDesc = ItemDesc;
            item.IsInInventory = IsInInventory;
            item.Brand = Brand;
            item.Remark = Remark;
            item.IsNonDiscountable = IsNonDiscountable;
            item.Deleted = false;
            item.Attributes1 = Attributes1;
            item.Attributes2 = Attributes2;
            item.Attributes3 = Attributes3;
            item.Attributes4 = Attributes4;
            item.Attributes5 = Attributes5;
            item.ProductLine = ProductLine;
            item.ModifiedOn = DateTime.Now;
            item.UniqueID = Guid.NewGuid();
            item.Save(userName);

            return item.ItemNo;
        }

        public static DataTable ConvertGSTRuleToString(DataTable dt)
        {
            dt.Columns["GSTRule"].ColumnName = "GSTRuleTmp";
            dt.Columns.Add("GSTRule");
            //change GST rule table
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["GSTRuleTmp"].ToString() == "1")
                {
                    dt.Rows[i]["GSTRule"] = "GST Exclusive";
                }
                else if (dt.Rows[i]["GSTRuleTmp"].ToString() == "2")
                {
                    dt.Rows[i]["GSTRule"] = "GST Inclusive";
                }
                else if (dt.Rows[i]["GSTRuleTmp"].ToString() == "3")
                {
                    dt.Rows[i]["GSTRule"] = "NON GST";
                }
            }
            dt.Columns.Remove("GSTRuleTmp");
            return dt;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public DataTable FetchAllNonDeletedItems()
        {
            ViewItemCollection coll = new ViewItemCollection();
            coll.Where("Deleted", false);
            coll.Load();


            DataTable dt = coll.ToDataTable();
            //mark true false to be yes no....

            dt = CommonUILib.DataTableConvertBoolColumnToYesOrNo(dt);
            dt = ConvertGSTRuleToString(dt);
            return dt;
        }

        #region Newly Added - And not used - Waiting confirmation to delete
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public ViewItemCollection FetchByName_forOrderTake(object name)
        {
            ArrayList itemNos = new ArrayList();

            ViewItemCollection coll1 = new ViewItemCollection().
                Where(ViewItem.Columns.ItemName, Comparison.Equals, name).
                Where(ViewItem.Columns.IsForSale, true).
                Where(ViewItem.Columns.Deleted, false).Load();
            coll1.Sort(ViewItem.Columns.ItemName, true);

            for (int i = 0; i < coll1.Count; i++)
            {
                itemNos.Add(coll1[i].ItemNo);
            }

            Query qr = ViewItem.CreateQuery();
            qr.AddWhere(ViewItem.Columns.ItemName, Comparison.Like, name + "%");
            qr.AddWhere(ViewItem.Columns.IsForSale, true);
            qr.AddWhere(ViewItem.Columns.Deleted, false);
            DataTable dt;
            if (itemNos.Count > 0)
            {
                dt = qr.NOT_IN(Item.Columns.ItemNo, itemNos).ExecuteDataSet().Tables[0];
            }
            else
            {
                dt = qr.ExecuteDataSet().Tables[0];
            }
            coll1.Load(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                itemNos.Add(dt.Rows[i]["ItemNo"]);
            }

            qr = ViewItem.CreateQuery();
            qr.AddWhere(ViewItem.Columns.ItemName, Comparison.Like, "%" + name.ToString().Replace(" ", "%") + "%");
            qr.AddWhere(ViewItem.Columns.IsForSale, true);
            qr.AddWhere(ViewItem.Columns.Deleted, false);
            if (itemNos.Count > 0)
            {
                dt = qr.NOT_IN(Item.Columns.ItemNo, itemNos).ORDER_BY(ViewItem.Columns.ItemName, "ASC").ExecuteDataSet().Tables[0];
            }
            else
            {
                dt = qr.ORDER_BY(ViewItem.Columns.ItemName, "ASC").ExecuteDataSet().Tables[0];
            }
            coll1.Load(dt);

            return coll1;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public DataTable FetchAllNonDeletedItems_PlusPointInfo()
        {
            string QueryStr =
                "SELECT     dbo.Category.IsForSale, dbo.Category.IsDiscountable, dbo.Category.CategoryName, dbo.Category.Category_ID, dbo.Category.IsGST, dbo.Category.AccountCategory, " +
                      "dbo.Item.ItemNo, dbo.Item.ItemName, dbo.Item.Barcode, dbo.Item.RetailPrice, dbo.Item.FactoryPrice, dbo.Item.MinimumPrice, dbo.Item.ItemDesc, " +
                      "dbo.Item.IsInInventory, dbo.Item.IsNonDiscountable, dbo.Item.Brand, dbo.Item.ProductLine, dbo.Item.Remark, dbo.Item.Deleted, dbo.Item.Attributes1, " +
                      "dbo.Item.Attributes2, dbo.Item.Attributes3, dbo.Item.Attributes4, dbo.Item.Attributes5, dbo.Item.Attributes6, dbo.Item.Attributes8, dbo.Item.Attributes7, " +
                      "dbo.Category.ItemDepartmentId, dbo.ItemDepartment.DepartmentName, " +
                      "dbo.Item.ItemNo + ' ' + dbo.Item.ItemName + ' ' + dbo.Item.CategoryName + ' ' + ISNULL(dbo.ItemDepartment.DepartmentName, '') + ' ' + ISNULL(dbo.Item.ItemDesc, '') " +
                      "AS search, dbo.Item.IsServiceItem, dbo.Item.IsCourse, dbo.Item.CourseTypeID, dbo.Item.ProductionDate, dbo.Item.IsGST AS Expr1, dbo.Item.hasWarranty, " +
                      "dbo.Item.IsDelivery, dbo.Item.GSTRule, dbo.Item.IsVitaMix, dbo.Item.IsWaterFilter, dbo.Item.IsYoung, dbo.Item.IsJuicePlus, dbo.Item.IsCommission, ISNULL(dbo.Item.userfld10,'N') AS PointType, ISNULL(dbo.Item.userfloat1,0) AS PointAmount " +
                "FROM         dbo.Category INNER JOIN " +
                      "dbo.Item ON dbo.Category.CategoryName = dbo.Item.CategoryName INNER JOIN " +
                      "dbo.ItemDepartment ON dbo.Category.ItemDepartmentId = dbo.ItemDepartment.ItemDepartmentID " +
                "WHERE dbo.Item.Deleted = 0 OR dbo.Item.Deleted IS NULL";

            QueryCommand cmd = new QueryCommand(QueryStr);

            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(cmd));

            //mark true false to be yes no....
            dt = CommonUILib.DataTableConvertBoolColumnToYesOrNo(dt);
            dt = ConvertGSTRuleToString(dt);
            return dt;
        }
        #endregion

        public static bool IsMatrixAttributes1(string attributes)
        {
            bool objreturn = false;
            ItemController itemLogic = new ItemController();
            Query qr = Item.Query();
            qr.WHERE("Attributes1", Comparison.Equals, attributes);
            qr.WHERE("userflag1", Comparison.Equals, true);
            qr.WHERE("Deleted", Comparison.Equals, false);

            ItemCollection col = itemLogic.FetchByQuery(qr);

            if (col != null && col.Count > 0)
            {
                objreturn = true;
            }


            return objreturn;
        }

        public static bool IsInventoryItem(string ItemNo)
        {
            //check if item no exist
            Query qr = Item.CreateQuery();

            int count = qr.WHERE(Item.Columns.Deleted, false).
                AND(Item.Columns.IsInInventory, true).
                AND(Item.Columns.ItemNo, ItemNo).
                GetCount(Item.Columns.ItemNo);

            if (count > 0)
            {
                return true;
            }
            return false;
        }

        public static bool IsItemGroup(string ItemNo)
        {
            //check if item no exist
            Query qr = ItemGroup.CreateQuery();

            int count = qr.WHERE(Item.Columns.Deleted, false).
                AND(ItemGroup.Columns.Barcode, ItemNo).
                GetCount(ItemGroup.Columns.Barcode);

            if (count > 0)
            {
                return true;
            }
            return false;
        }

        public static bool IsInventoryItemBarcode(string barcode, out string itemno)
        {
            itemno = "";

            //check if item no exist
            Query qr = Item.CreateQuery();

            qr.SelectList = "ItemNo";
            qr.QueryType = QueryType.Select;

            itemno = (string)qr.WHERE(Item.Columns.Deleted, false).
                AND(Item.Columns.IsInInventory, true).
                AND(Item.Columns.Barcode, barcode).ExecuteScalar();


            if (itemno != "" && itemno != null)
            {
                return true;
            }

            //Try to fetch from alternate barcode
            qr = AlternateBarcode.CreateQuery();
            qr.SelectList = "ItemNo";
            qr.QueryType = QueryType.Select;

            itemno = (string)qr.WHERE(AlternateBarcode.Columns.Barcode, barcode).
                AND(AlternateBarcode.Columns.Deleted, false).
                ExecuteScalar();

            if (itemno != "" && itemno != null)
            {
                return true;
            }
            return false;
        }

        public static bool IsNonInventoryItem(string input, out string itemno) {
            //check if item no exist
            Query qr = Item.CreateQuery();

            qr.SelectList = "ItemNo";
            qr.QueryType = QueryType.Select;

            itemno = (string)qr.WHERE(Item.Columns.Deleted, false).
                AND(Item.Columns.IsInInventory, false).
                AND(Item.Columns.IsServiceItem, false).
                AND(Item.UserColumns.NonInventoryProduct, true).
                AND(Item.Columns.ItemNo, input).ExecuteScalar();


            if (itemno != "" && itemno != null)
            {
                return true;
            }

            itemno = (string)qr.WHERE(Item.Columns.Deleted, false).
                AND(Item.Columns.IsInInventory, false).
                AND(Item.Columns.IsServiceItem, false).
                AND(Item.UserColumns.NonInventoryProduct, true).
                AND(Item.Columns.Barcode, input).ExecuteScalar();


            if (itemno != "" && itemno != null)
            {
                return true;
            }

            //Try to fetch from alternate barcode
            qr = AlternateBarcode.CreateQuery();
            qr.SelectList = "ItemNo";
            qr.QueryType = QueryType.Select;

            itemno = (string)qr.WHERE(AlternateBarcode.Columns.Barcode, input).
                AND(AlternateBarcode.Columns.Deleted, false).
                ExecuteScalar();

            if (itemno != "" && itemno != null)
            {
                return true;
            }
            return false;
        }

        public static bool IsPromoBarcode(string barcode)
        {
            //check if item no exist
            PromoCampaignHdr it = new PromoCampaignHdr(PromoCampaignHdr.Columns.Barcode, barcode);

            if (it.IsLoaded && !it.IsNew && it.Deleted.HasValue && !it.Deleted.Value)
            {
                if (DateTime.Now <= it.DateFrom || DateTime.Now >= it.DateTo)
                {
                    return false;
                }

                //Load Check Location
                PromoOutletCollection po = new PromoOutletCollection();
                po.Where(PromoOutlet.Columns.PromoCampaignHdrID, it.PromoCampaignHdrID);
                po.Where(PromoOutlet.Columns.OutletName.Trim(), PointOfSaleInfo.OutletName.Trim());
                po.Where(PromoOutlet.Columns.Deleted, false);
                po.Load();
                if (po.Count <= 0)
                {
                    return false;
                }

                string query = "select d.* " +
                           "from PromoCampaignHdr h " +
                           "inner join PromoCampaignDet d on h.PromoCampaignHdrID = d.PromoCampaignHdrID " +
                           "where ISNULL(h.Deleted,0) = 0 and ISNULL(d.Deleted,0) = 0 and h.Barcode = '" + barcode + "'";
                DataSet promocol = DataService.GetDataSet(new QueryCommand(query));

                int countItem = 0;

                if (promocol != null && promocol.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in promocol.Tables[0].Rows)
                    {
                        if ((int)row["UnitQty"] > 1)
                        {
                            countItem++;
                        }
                    }
                }
                return promocol.Tables[0].Rows.Count == 1 && countItem == 1;
            }

            return false;
        }

        // The Rule for a Low Quantity Item is when On Hand <= MinQty 
        // MinQty = MIN(TriggerQuantity) FROM ItemQuantityTrigger
        // On Hand = Stock In - Stock Out + Transfer In - Transfer Out + Adjustment In - AdjustmentOut - Undeducted
        // Return 1 means Low, 0 means OK
        public static bool IsLowQuantityItem(string ItemNo, int InventoryLocationID, decimal qtyOrdered)
        {
            QueryCommand cmd = new QueryCommand(
                //"SELECT CASE WHEN MinQty = -1 THEN 0 ELSE CASE WHEN MinQty >= [On Hand] - " + qtyOrdered.ToString("F2") + " THEN 1 ELSE 0 END END LowStatus " +
                "SELECT CASE WHEN MinQty = -1 THEN 0 ELSE CASE WHEN MinQty >= [On Hand] - @qtyOrdered THEN 1 ELSE 0 END END LowStatus " +
                "FROM " +
                "( " +
                "SELECT a.*" +
                    ", a.BalanceQty AS [On Hand] " +
                    ", ISNULL(MinQty,-1) AS MinQty " +
                "FROM ItemSummary a " +
                    "INNER JOIN InventoryLocation e  on a.InventoryLocationID = e.InventoryLocationID " +
                    "LEFT OUTER JOIN (  " +
                        "SELECT x.ItemNo, MIN(TriggerQuantity) AS MinQty  " +
                        "FROM ItemQuantityTrigger x   " +
                            "INNER JOIN InventoryLocation IL ON x.InventoryLocationID = IL.InventoryLocationID  " +
                            "INNER JOIN Item c ON x.ItemNo = c.ItemNo  " +
                            "INNER JOIN Category d ON c.CategoryName = d.CategoryName  " +
                        "WHERE x.Deleted IS NULL OR x.Deleted = 0  " +
                            "AND IL.InventoryLocationID = @InventoryLocationID " +
                        "GROUP BY x.ItemNo   " +
                    ") o ON a.ItemNo = o.ItemNo   " +
                "WHERE a.ItemNo = @ItemNo AND a.InventoryLocationID = @InventoryLocationID " +
                ") p");

            cmd.Parameters.Add("@ItemNo", ItemNo);
            cmd.Parameters.Add("@InventoryLocationID", InventoryLocationID);
            cmd.Parameters.Add("@qtyOrdered", qtyOrdered, DbType.Decimal);

            //ItemQuantityTriggerCollection it = new ItemQuantityTrigger

            object result = DataService.ExecuteScalar(cmd);

            if (result != null)
            {
                if (int.Parse(result.ToString()) == 1)
                {
                    return true;
                }
            }

            return false;
        }

        public DataTable SearchItem_PlusPointInfoForIntegration(bool SyncAll)
        {
            string QueryStr =
                "SELECT  dbo.Category.CategoryName as Category,  " +
                      "dbo.Item.ItemNo, dbo.Item.ItemName, dbo.Item.Barcode, dbo.Item.RetailPrice, dbo.Item.FactoryPrice, dbo.Item.ItemDesc, " +
                      "dbo.Item.IsInInventory, dbo.Item.IsServiceItem, dbo.Item.IsNonDiscountable, dbo.Item.Attributes1, " +
                      "dbo.Item.Attributes2, dbo.Item.Attributes3, dbo.Item.Attributes4, dbo.Item.Attributes5, dbo.Item.Attributes6, dbo.Item.Attributes8, dbo.Item.Attributes7, " +
                      "dbo.Category.ItemDepartmentId, dbo.ItemDepartment.DepartmentName, " +
                      "dbo.Item.GSTRule, dbo.Item.IsCommission, ISNULL(dbo.Item.userfld10,'N') AS PointType, ISNULL(dbo.Item.userfloat1,0) AS PointAmount, dbo.Item.Deleted, " +
                      "CASE WHEN ISNULL(dbo.Item.Userfld9,'N') = 'N' then 'No' Else 'Yes' End as IsPointRedeemable, ISNULL(dbo.Item.Remark,'') as Remark,  " +
                      "dbo.Item.CreatedOn, dbo.Item.CreatedBy, dbo.Item.ModifiedOn, dbo.Item.ModifiedBy, CASE WHEN ISNULL(dbo.item.userflag5, 0) = 0 then 'No' else 'Yes' End IsDelivery, CASE WHEN ISNULL(dbo.item.userflag1, 0) = 0 then 'No' else 'Yes' End IsMatrixItem " +
                "FROM         dbo.Category INNER JOIN " +
                      "dbo.Item ON dbo.Category.CategoryName = dbo.Item.CategoryName INNER JOIN " +
                      "dbo.ItemDepartment ON dbo.Category.ItemDepartmentId = dbo.ItemDepartment.ItemDepartmentID " +
                "WHERE (dbo.Item.Deleted = 0 OR dbo.Item.Deleted IS NULL) AND " +
                      "(dbo.Item.ItemNo + ' ' + dbo.Item.ItemName + ' ' + ISNULL(dbo.Item.Barcode, '') + ' ' + dbo.Item.CategoryName + ' ' + ISNULL(dbo.ItemDepartment.DepartmentName, '') + ' ' + ISNULL(dbo.Item.ItemDesc, '')) LIKE '%' + @Search + '%' ";



            if (!SyncAll)
                QueryStr += "AND dbo.Item.ModifiedOn > '" + DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd") + "' ";

            QueryStr += "ORDER BY ItemNo ";

            QueryCommand cmd = new QueryCommand(QueryStr);

            cmd.Parameters.Add("@Search", "");

            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(cmd));

            //mark true false to be yes no....
            dt = CommonUILib.DataTableConvertBoolColumnToYesOrNo(dt);
            dt = ConvertGSTRuleToString(dt);
            return dt;

        }

        // The Rule for a Low Quantity Item is when On Hand <= MinQty 
        // MinQty = MIN(TriggerQuantity) FROM ItemQuantityTrigger
        // On Hand = Stock In - Stock Out + Transfer In - Transfer Out + Adjustment In - AdjustmentOut - Undeducted
        // Return 1 means Low, 0 means OK
        public static decimal GetStockOnHand(string ItemNo, int InventoryLocationID)
        {
            QueryCommand cmd = new QueryCommand(
                "SELECT [BalanceQty] as OnHand " +
                "FROM ItemSummary where ItemNo = @ItemNo and InventoryLocationID = @InventoryLocationID");

            cmd.Parameters.Add("@ItemNo", ItemNo);
            cmd.Parameters.Add("@InventoryLocationID", InventoryLocationID);

            object result = DataService.ExecuteScalar(cmd);

            if (result != null)
            {
                //return int.Parse(result.ToString());
                decimal res;
                if (!decimal.TryParse(result.ToString(), out res))
                {
                    res = 0;
                }
                return res;
            }

            return 0;
        }

        public static int GetMinQty(string ItemNo, int InventoryLocationID)
        {
            QueryCommand cmd = new QueryCommand("SELECT ISNULL(MIN(TriggerQuantity), 0) AS MinQty  " +
                "FROM ItemQuantityTrigger x   " +
                    "INNER JOIN InventoryLocation IL ON x.InventoryLocationID = IL.InventoryLocationID  " +
                    "INNER JOIN Item c ON x.ItemNo = c.ItemNo  " +
                    "INNER JOIN Category d ON c.CategoryName = d.CategoryName  " +
                "WHERE x.Deleted IS NULL OR x.Deleted = 0  " +
                    "AND IL.InventoryLocationID = @InventoryLocationID " +
                    "AND c.ItemNo = @ItemNo " +
                "GROUP BY x.ItemNo");

            cmd.Parameters.Add("@ItemNo", ItemNo);
            cmd.Parameters.Add("@InventoryLocationID", InventoryLocationID);

            object result = DataService.ExecuteScalar(cmd);

            if (result != null)
            {
                int intResult = int.Parse(result.ToString());
                return intResult;
            }

            return 0;
        }

        public static bool IsSupplierItem(string ItemNo, string SupplierName)
        {
            //check if item no exist
            String qry = "Select count(*) from itemsuppliermap i, supplier s where i.itemno = '" + ItemNo + "' " +
                "and i.SupplierID = s.SupplierId and s.SupplierName = N'" + SupplierName.Replace("'", "''") + "'";

            int count = (int)DataService.ExecuteScalar(new QueryCommand(qry));

            if (count > 0)
            {
                return true;
            }
            return false;
        }

        public static int FetchPreOrderSoldQty(string ItemNo)
        {
            int val = 0;
            string sql = @"SELECT ISNULL(SUM(Quantity), 0)
                            FROM OrderDet
                            WHERE IsVoided = 0 AND IsPreOrder = 1 AND ItemNo = @ItemNo";
            QueryCommand cmd = new QueryCommand(sql, "PowerPOS");
            cmd.Parameters.Add("@ItemNo", ItemNo);

            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(cmd));
            if (dt.Rows.Count > 0 && dt.Columns.Count > 0)
                val = ((decimal)dt.Rows[0][0]).ToString("N0").GetIntValue();
            return val;
        }

        /// <summary>
        /// Price calculated for funding
        /// </summary>
        public static decimal FetchPriceForFunding(string ItemNo, string FundingSettingKey)
        {
            string sql = "SELECT [dbo].[GetFundingPrice](@ItemNo, @FundingSettingKey)";
            QueryCommand cmd = new QueryCommand(sql, "PowerPOS");
            cmd.Parameters.Add("@ItemNo", ItemNo);
            cmd.Parameters.Add("@FundingSettingKey", FundingSettingKey);
            object result = DataService.ExecuteScalar(cmd);

            if (result != null)
            {
                decimal decResult = decimal.Parse(result.ToString());
                return decResult;
            }

            return 0;
        }

        public static DataTable GetSimilarItem(string barcode)
        {
            DataTable dt = null;

            if (!string.IsNullOrEmpty(barcode))
            {
                string query = "select itemno, itemname, barcode from item where ISNULL(deleted,0) = 0 and itemname in (select itemname from item where barcode = '" + barcode + "') ";

                DataSet ds = DataService.GetDataSet(new QueryCommand(query));
                dt = ds.Tables[0];
            }

            return dt;
        }

        public static bool MergeSimilarItem(string barcode, string username, out string status)
        {
            status = "";

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    Item i = new Item(Item.Columns.Barcode, barcode);

                    if (i == null)
                    {
                        throw new Exception("Item with selected barcode doesn't exist");
                    }

                    ItemCollection col = new ItemCollection();
                    col.Where(Item.Columns.Deleted, false);
                    col.Where(Item.Columns.ItemName, i.ItemName);
                    col.Where(Item.Columns.Barcode, Comparison.NotEquals, i.Barcode);
                    col.Load();

                    if (col.Count > 0)
                    {
                        List<string> ListItemNo = new List<string>();

                        for (int k = 0; k < col.Count; k++)
                        {

                            ListItemNo.Add(col[k].ItemNo);

                            //deleted other item and keep main item
                            string alternatebarcode = col[k].Barcode;
                            string itemno = col[k].ItemNo;
                            col[k].Barcode = string.Empty;
                            col[k].Deleted = true;
                            col[k].Save(username);

                            //if it has barcode then save it into alternate barcode

                            if (!string.IsNullOrEmpty(alternatebarcode))
                            {
                                if (!CheckDuplicateAlternateBarcode(alternatebarcode, 0))
                                {
                                    AlternateBarcode ab = new AlternateBarcode();
                                    ab.ItemNo = i.ItemNo;
                                    ab.Barcode = alternatebarcode;

                                    ab.Save(username);
                                }
                            }

                            //delete item base level

                            ItemBaseLevelCollection ib = new ItemBaseLevelCollection();
                            ib.Where(ItemBaseLevel.Columns.Deleted, false);
                            ib.Where(ItemBaseLevel.Columns.ItemNo, itemno);
                            ib.Load();

                            if (ib.Count > 0)
                            {
                                foreach (ItemBaseLevel ibdetail in ib)
                                {
                                    ibdetail.Deleted = true;
                                    ibdetail.Save(username);
                                }
                            }


                            //update item group map
                            ItemGroupMapCollection img = new ItemGroupMapCollection();
                            img.Where(ItemQuantityTrigger.Columns.Deleted, false);
                            img.Where(ItemQuantityTrigger.Columns.ItemNo, itemno);
                            img.Load();

                            if (img.Count > 0)
                            {
                                foreach (ItemGroupMap imgdetail in img)
                                {
                                    imgdetail.ItemNo = i.ItemNo;
                                    imgdetail.Save(username);
                                }
                            }

                            //deleted item quantity trigger
                            ItemQuantityTriggerCollection iq = new ItemQuantityTriggerCollection();
                            iq.Where(ItemQuantityTrigger.Columns.Deleted, false);
                            iq.Where(ItemQuantityTrigger.Columns.ItemNo, itemno);
                            iq.Load();

                            if (iq.Count > 0)
                            {
                                foreach (ItemQuantityTrigger iqdetail in iq)
                                {
                                    iqdetail.Deleted = true;
                                    iqdetail.Save(username);
                                }
                            }

                            //deleted outletitemgroup
                            OutletGroupItemMapCollection og = new OutletGroupItemMapCollection();
                            og.Where(OutletGroupItemMap.Columns.Deleted, false);
                            og.Where(OutletGroupItemMap.Columns.ItemNo, itemno);
                            og.Load();

                            if (og.Count > 0)
                            {
                                foreach (OutletGroupItemMap ogdetail in og)
                                {
                                    ogdetail.IsItemDeleted = true;
                                    ogdetail.Deleted = true;
                                    ogdetail.Save(username);
                                }
                            }



                            //update membership points and point allocation log
                            //MembershipPointCollection mp = new MembershipPointCollection();
                            //mp.Where(MembershipPoint.Columns.Deleted, false);
                            //mp.Where(MembershipPoint.Columns.ItemNo, itemno);
                            //mp.Load();

                            //if (mp.Count > 0)
                            //{
                            //    foreach (ItemQuantityTrigger iqdetail in iq)
                            //    {
                            //        iqdetail.Deleted = true;
                            //        iqdetail.Save(Session["username"] + " ");
                            //    }
                            //}

                            // update inventory detail 
                            InventoryDetCollection id = new InventoryDetCollection();
                            id.Where(InventoryDet.Columns.Deleted, false);
                            id.Where(InventoryDet.Columns.ItemNo, itemno);
                            id.Load();

                            if (id.Count > 0)
                            {
                                foreach (InventoryDet iddetail in id)
                                {
                                    iddetail.ItemNo = i.ItemNo;
                                    iddetail.OldItemNo = itemno;
                                    iddetail.Save(username);
                                }
                            }

                            // update order detail 
                            OrderDetCollection od = new OrderDetCollection();
                            od.Where(OrderDet.Columns.IsVoided, false);
                            od.Where(OrderDet.Columns.ItemNo, itemno);
                            od.Load();

                            if (od.Count > 0)
                            {
                                foreach (OrderDet oddetail in od)
                                {
                                    oddetail.ItemNo = i.ItemNo;
                                    oddetail.OldItemNo = itemno;
                                    oddetail.Save(username);
                                }
                            }

                        }

                        // update item summary
                        InventoryLocationCollection invcol = new InventoryLocationCollection();
                        invcol.Where(InventoryLocation.Columns.Deleted, false);
                        invcol.Load();

                        if (invcol.Count > 0)
                        {
                            for (int k = 0; k < invcol.Count; k++)
                            {
                                ItemSummaryCollection its = new ItemSummaryCollection();
                                its.Where(ItemSummary.Columns.InventoryLocationID, invcol[k].InventoryLocationID);
                                its.Where(ItemSummary.Columns.ItemNo, Comparison.In, ListItemNo);
                                its.Load();

                                if (its.Count > 0)
                                {
                                    ItemSummary mainitemsum;

                                    ItemSummaryCollection mainits = new ItemSummaryCollection();
                                    mainits.Where(ItemSummary.Columns.InventoryLocationID, invcol[k].InventoryLocationID);
                                    mainits.Where(ItemSummary.Columns.ItemNo, i.ItemNo);
                                    mainits.Load();

                                    if (mainits.Count > 0)
                                    {
                                        mainitemsum = mainits[0];
                                    }
                                    else
                                    {
                                        mainitemsum = new ItemSummary();
                                        mainitemsum.ItemSummaryID = i.ItemNo + "-" + invcol[k].InventoryLocationID;
                                        mainitemsum.ItemNo = i.ItemNo;
                                        mainitemsum.InventoryLocationID = invcol[k].InventoryLocationID;
                                        mainitemsum.Deleted = false;
                                        mainitemsum.UniqueID = Guid.NewGuid();
                                        mainitemsum.BalanceQty = 0;
                                        mainitemsum.CostPrice = 0;
                                        mainitemsum.Save(username);
                                    }

                                    for (int z = 0; z < its.Count; z++)
                                    {
                                        decimal oldCostprice = mainitemsum.CostPrice ?? 0;
                                        decimal oldBalanceQty = 0;
                                        if(mainitemsum.BalanceQty != null)
                                            oldBalanceQty = (decimal)mainitemsum.BalanceQty < 0 ? 0 : (decimal)mainitemsum.BalanceQty;

                                        if (mainitemsum.BalanceQty == null)
                                            mainitemsum.BalanceQty = 0;

                                        mainitemsum.BalanceQty += its[z].BalanceQty;
                                        mainitemsum.CostPrice = (oldBalanceQty * oldCostprice) + ((decimal)its[z].BalanceQty * its[z].CostPrice) / (oldBalanceQty + (decimal)its[z].BalanceQty);
                                        mainitemsum.Save(username);


                                        decimal oldItemCostprice = i.AvgCostPrice ?? 0;
                                        decimal oldItemBalanceQty = 0; 
                                        if(i.BalanceQuantity != null)
                                            oldItemBalanceQty = (decimal)i.BalanceQuantity < 0 ? 0 : (decimal)i.BalanceQuantity;

                                        if (i.BalanceQuantity == null)
                                            i.BalanceQuantity = 0;

                                        i.BalanceQuantity += its[z].BalanceQty;
                                        i.AvgCostPrice = (oldItemBalanceQty * oldItemCostprice) + ((decimal)its[z].BalanceQty * its[z].CostPrice) / (oldItemBalanceQty + (decimal)its[z].BalanceQty);
                                        i.Save(username);

                                        its[z].Deleted = true;
                                        its[z].Save(username);
                                    }

                                }

                            }
                        }

                    }

                    scope.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                return false;
            }
        }

        public static bool CheckDuplicateAlternateBarcode(string barcode, int BarcodeID)
        {
            if (BarcodeID == 0)
            {
                /*if new*/
                string query = "select ISNULL(MAX(BarcodeID),0) as BarcodeID " +
                                "from AlternateBarcode";
                QueryCommand cmd = new QueryCommand(query);
                DataTable dt = new DataTable();
                dt.Load(DataService.GetReader(cmd));


                if (dt.Rows[0][0] != null && dt.Rows[0][0].ToString() != "{}")
                    BarcodeID = Int32.Parse(dt.Rows[0][0].ToString()) + 1;
            }

            string sqlstring =
                "DECLARE @Barcode varchar(50) " +
                "Declare @BarcodeID int " +
                "set @Barcode = '" + barcode + "' " +
                "set @BarcodeID = " + BarcodeID + " " +
                "select barcode from AlternateBarcode where Barcode = @Barcode and ISNULL(Deleted,0) = 0 and BarcodeID <> @BarcodeID " +
                "union " +
                "select Barcode from PromoCampaignHdr where Barcode = @Barcode and ISNULL(Deleted,0) = 0 " +
                "union " +
                "Select PromoCode from PromoCampaignHdr where PromoCode = @Barcode and ISNULL(Deleted,0) = 0 " +
                "union " +
                "Select barcode from Item where Barcode = @Barcode and ISNULL(Deleted,0) = 0 " +
                "union " +
                "select barcode from ItemGroup where Barcode = @Barcode and ISNULL(Deleted,0) = 0";

            DataTable d = new DataTable();
            d.Load(DataService.GetReader(new QueryCommand(sqlstring)));

            return d.Rows.Count > 0;
        }

        public static RecipeDetailCollection FetchItemMaterial(string itemNo, out string status)
        {
            RecipeDetailCollection materials = new RecipeDetailCollection();

            try
            {
                status = "";
                RecipeHeaderCollection rh = new RecipeHeaderCollection();
                rh.Where(RecipeHeader.Columns.ItemNo, itemNo);
                rh.Where(RecipeHeader.Columns.Deleted, false);
                rh.Load();
                if (rh.Count > 0)
                {
                    if ((rh[0].Type + "") != "Attribute")
                    {
                        if (rh[0].ItemNo == itemNo && rh[0].Deleted == false)
                        {
                            materials.Where(RecipeDetail.Columns.RecipeHeaderID, rh[0].RecipeHeaderID);
                            materials.Where(RecipeDetail.Columns.Deleted, false);
                            materials.Load();
                        }
                    }
                }

                return materials;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = "Error occurred: " + ex.Message;
                return materials;
            }
        }

        public static decimal GetConversionRate(string itemNo, string fromUOM, string toUOM)
        {
            if (fromUOM == toUOM) return 1;

            UOMConversionDetCollection dets = new UOMConversionHdr("ItemNo", itemNo).UOMConversionDetRecords();
            foreach (UOMConversionDet det in dets)
            {
                if (det.Deleted || det.UOMConversionHdr.Deleted) continue;

                if (det.FromUOM == fromUOM)
                {
                    if (det.ToUOM == toUOM)
                    {
                        return det.ConversionRate;
                    }
                    else
                    {
                        return det.ConversionRate * GetConversionRate(itemNo, det.ToUOM, toUOM);
                    }
                }
            }

            foreach (UOMConversionDet det in dets)
            {
                if (det.Deleted || det.UOMConversionHdr.Deleted) continue;

                if (det.FromUOM == toUOM)
                {
                    if (det.ToUOM == fromUOM)
                    {
                        return 1 / det.ConversionRate;
                    }
                    else
                    {
                        return 1 / (det.ConversionRate * GetConversionRate(itemNo, det.ToUOM, fromUOM));
                    }
                }
            }

            // Conversion setup not found, return default rate = 1
            return 1;
        }

        public static List<string> GetUOMListByItemNo(string itemNo)
        {
            List<string> uomList = new List<string>();
            IDataReader rdr = null;

            try
            {
                #region *) SQL String
                string sql = "SELECT DISTINCT UOM " +
                             "From " +
                             "   ( " +
                             "   SELECT DISTINCT ItemNo,(LTRIM(RTRIM(Userfld1))) UOM FROM Item WHERE ISNULL(Userfld1, '') <> '' and ISNULL(Deleted, 0) = 0 " +
                             "   UNION " +
                             "   SELECT DISTINCT h.ItemNo,(LTRIM(RTRIM(d.FromUOM))) UOM FROM UOMConversionDet d inner join UOMConversionHdr h on d.ConversionHdrID = h.ConversionHdrID and ISNULL(h.Deleted,0) = 0 and ISNULL(d.Deleted,0) = 0 " +
                             "   UNION " +
                             "   SELECT DISTINCT h.ItemNo,(LTRIM(RTRIM(d.ToUOM))) UOM FROM UOMConversionDet d inner join UOMConversionHdr h on d.ConversionHdrID = h.ConversionHdrID and ISNULL(h.Deleted,0) = 0 and ISNULL(d.Deleted,0) = 0 " +
                             "   ) as T " +
                             "where T.ItemNo = '" + itemNo + "' ";
                #endregion
                rdr = new InlineQuery().ExecuteReader(sql);
                while (rdr.Read())
                {
                    uomList.Add(rdr[0].ToString());
                }
                return uomList;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return uomList;
            }
            finally
            {
                if (rdr != null)
                {
                    if (!rdr.IsClosed) rdr.Close();
                    rdr.Dispose();
                }
            }
        }

        public static Decimal GetCustomPricing(string ItemNo, string MembershipNo, int PointOfSaleID)
        {
            decimal objreturn = 0;
            #region obsolete
            //            string query = @"                                                         
            //                                SELECT ItemNo, OutletName, PointOfSaleID
            //                                FROM
            //                                (
            //	                                select ogm.ItemNo, o.OutletName ,p.PointofSaleID, p.PointOfSaleName 
            //	                                from outletgroupitemmap ogm 
            //	                                inner join Item i on ogm.ItemNo = i.ItemNo and ISNULL(i.Deleted,0) = 0 
            //	                                left join outlet o on o.OutletName = ogm.OutletName and ISNULL(o.Deleted,0) = 0
            //	                                left join pointofsale p on p.OutletName = o.OutletName and ISNULL(p.Deleted,0) = 0
            //	                                where ISNULL(ogm.IsItemDeleted, 0) = 0
            //	                                union 
            //	                                select ogm.ItemNo, o.OutletName, p.PointofSaleID, p.PointOfSaleName 
            //	                                from outletgroupitemmap ogm 
            //	                                inner join Item i on ogm.ItemNo = i.ItemNo and ISNULL(i.Deleted,0) = 0 
            //	                                left join outlet o on o.outletgroupID = ogm.OutletGroupID and ISNULL(o.Deleted,0) = 0
            //	                                left join pointofsale p on p.OutletName = o.OutletName and ISNULL(p.Deleted,0) = 0
            //	                                where ISNULL(ogm.IsItemDeleted, 0) = 0
            //                                )ox
            //                                where ItemNo = '{0}' and PointOfSaleID = {1} ";
            //            query = String.Format(query, ItemNo, PointOfSaleID);

            //            DataTable dt = DataService.GetDataSet(new QueryCommand(query)).Tables[0];
            //            if (dt.Rows.Count > 0)
            //            {
            //                string queryOutlet = @"select ISNULL(ISNULL(ocp.RetailPrice, i.RetailPrice),0) as RetailPrice 
            //                                        from item i
            //                                        Left join OutletCustomerPricing ocp on Ocp.ItemNo = i.ItemNo and ISNULL(ocp.Deleted,0) = 0
            //                                        left join PointOfSale op on op.OutletName = ocp.OutletName
            //                                        where ISNULL(i.Deleted,0) = 0 and i.ItemNo = '{0}' and op.PointOfsaleID = {1} and 
            //	                                        ocp.MembershipNo = '{2}'
            //                                        ";
            //                queryOutlet = string.Format(queryOutlet, ItemNo, PointOfSaleID, MembershipNo);
            //                objreturn = (Decimal)DataService.ExecuteScalar(new QueryCommand(queryOutlet));
            //            }
            //            else
            //            {
            //                string queryOutlet = @"select ISNULL(ISNULL(ocp.RetailPrice, i.RetailPrice),0) as RetailPrice 
            //                                        from item i
            //                                        Left join CustomerPricing ocp on Ocp.ItemNo = i.ItemNo and ISNULL(ocp.Deleted,0) = 0
            //                                        where ISNULL(i.Deleted,0) = 0 and i.ItemNo = '{0}' and ocp.MembershipNo = '{2}'
            //                                        ";
            //                queryOutlet = string.Format(queryOutlet, ItemNo, PointOfSaleID, MembershipNo);
            //                objreturn = (Decimal)DataService.ExecuteScalar(new QueryCommand(queryOutlet));
            //            }
            #endregion
            string query = @"select ISNULL(ocp.RetailPrice, ISNULL(cp.RetailPrice, ISNULL(i.RetailPrice,0))) as RetailPrice 
                                from item i
                                Left join OutletCustomerPricing ocp on Ocp.ItemNo = i.ItemNo and ISNULL(ocp.Deleted,0) = 0 and ocp.MembershipNo = '{2}'
                                left join PointOfSale op on op.OutletName = ocp.OutletName  and op.PointOfsaleID = {1} 
                                Left join CustomerPricing cp on cp.ItemNo = i.ItemNo and ISNULL(cp.Deleted,0) = 0 and cp.MembershipNo = '{2}'                                       
                                where ISNULL(i.Deleted,0) = 0 and i.ItemNo = '{0}'";
            query = string.Format(query, ItemNo, PointOfSaleID, MembershipNo);
            objreturn = (Decimal)DataService.ExecuteScalar(new QueryCommand(query));

            return objreturn;
        }

        public static System.Drawing.Image ResizeImage(System.Drawing.Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (System.Drawing.Image)b;
        }

        public static byte[] ResizeImageBiteToArray(System.Drawing.Image imgToResize, Size size) 
        {
            System.Drawing.Image img = ResizeImage(imgToResize, size);
            return ImageToByteArray(img);
        }

        public static byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }
    }
}
