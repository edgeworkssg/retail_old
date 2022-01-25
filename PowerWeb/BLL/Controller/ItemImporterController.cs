using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using PowerPOS;
using SubSonic;
using System.Collections.Generic;

namespace PowerWeb
{
    public class ItemImporterController
    {
        public static DataTable FetchItem(string itemDeptID, string category)
        {
            DataTable dt = new DataTable();

            try
            {
                string sql = @"
                            DECLARE @ItemDepartmentID NVARCHAR(MAX);
                            DECLARE @CategoryName NVARCHAR(MAX);

                            SET @ItemDepartmentID = '{0}';
                            SET @CategoryName ='{1}';
                            SELECT    ID.ItemDepartmentID [Department Code]
                                        ,ID.DepartmentName [Department Name]
                                        ,C.Category_ID [Category Code]
                                        ,C.CategoryName [Category Name]
                                        ,I.ItemNo  [Item No]
                                        ,CASE WHEN ISNULL(I.userflag1,0) = 0 THEN I.ItemName ELSE I.Attributes2 END   [Item Name]
                                        ,I.Barcode
                                        ,I.RetailPrice [Retail Price]
                                        ,I.FactoryPrice [Cost Price]
                                        ,I.userfld1 UOM
                                        ,CASE WHEN I.GSTRule = 1 THEN 'Exclusive'
                                              WHEN I.GSTRule = 2 THEN 'Inclusive'
                                              WHEN I.GSTRule = 3 THEN 'Non GST' END [GST Rule]			  
                                        ,CASE WHEN ISNULL(I.IsNonDiscountable,0) = 0 THEN 'No' ELSE 'Yes' END  [Is Non Discountable]
                                        ,CASE WHEN ISNULL(I.userfld9,'N') = 'N' THEN 'No' ELSE 'Yes' END  [Point Redeemable]		
                                        ,CASE WHEN ISNULL(I.IsCommission,0) = 0 THEN 'No' ELSE 'Yes' END  [Give Commission]
                                        ,CASE WHEN ISNULL(I.Userflag2,0) = 0 THEN 'No' ELSE 'Yes' END  [Allow Pre Order]        
                                        ,CASE WHEN I.IsServiceItem = 0 AND I.IsInInventory = 1 THEN 'Product'
                                              WHEN I.IsServiceItem = 1 AND I.IsInInventory = 1 THEN 'Open Price Product'
                                              WHEN I.IsServiceItem = 1 AND I.IsInInventory = 0 THEN 'Service'
                                              WHEN I.IsServiceItem = 0 AND I.IsInInventory = 0 AND I.userfld10 = 'D' THEN 'Point Package'
                                              WHEN I.IsServiceItem = 0 AND I.IsInInventory = 0 AND I.userfld10 = 'T' THEN 'Course Package'	
                                              {12}		
                                        END [Item Type]	  
                                        ,ISNULL(I.Userfloat1,0) [Point Get]
                                        ,ISNULL(I.userfloat3,0) [Breakdown Price]
                                        ,I.ItemDesc Description
                                        ,I.Attributes1
                                        ,I.Attributes2
                                        ,I.Attributes3
                                        ,I.Attributes4
                                        ,I.Attributes5
                                        ,I.Attributes6
                                        ,I.Attributes7
                                        ,I.Attributes8
                                        ,I.Remark
                                        ,CASE WHEN ISNULL(I.Deleted,0) = 0 THEN 'No' ELSE 'Yes' END [Deleted]		
                                        ,MAX(SUP.SupplierName) [Supplier Name]		
                                        ,CASE WHEN ISNULL(I.userflag1,0) = 0 THEN 'No' ELSE 'Yes' END  [Is Matrix Item]	
                                        ,CASE WHEN ISNULL(I.Userflag9,0) = 0 THEN 'No' ELSE 'Yes' END [Is Using Fixed Value for COG]
                                        ,I.Userfld7 as [Fixed COG Type]
                                        ,I.Userfloat4 as [Fix COG Value]
                                        {13}
                                        {2} {7}	
                                        {3} {8}
                                        {4} {9}
                                        {5} {10}
                                        {6} {11}
                                        {15}
                                FROM	Item I
                                        LEFT JOIN Category C ON C.CategoryName = I.CategoryName
                                        LEFT JOIN ItemDepartment ID ON ID.ItemDepartmentID = C.ItemDepartmentId
                                        LEFT JOIN ItemSupplierMap ISM ON ISM.ItemNo = I.ItemNo AND ISNULL(ISM.Deleted,0) = 0
                                        LEFT JOIN Supplier SUP ON SUP.SupplierID = ISM.SupplierID		
                                WHERE	ID.DepartmentName <> 'SYSTEM'
                                        AND (ID.ItemDepartmentID = @ItemDepartmentID OR @ItemDepartmentID = 'ALL')
                                        AND (C.CategoryName = @CategoryName OR @CategoryName = 'ALL')
                                GROUP BY ID.ItemDepartmentID 
                                        ,ID.DepartmentName 
                                        ,C.Category_ID
                                        ,C.CategoryName 
                                        ,I.ItemNo
                                        ,CASE WHEN ISNULL(I.userflag1,0) = 0 THEN I.ItemName ELSE I.Attributes2 END
                                        ,I.Barcode
                                        ,I.RetailPrice 
                                        ,I.FactoryPrice 
                                        ,I.userfld1 
                                        ,CASE WHEN I.GSTRule = 1 THEN 'Exclusive'
                                              WHEN I.GSTRule = 2 THEN 'Inclusive'
                                              WHEN I.GSTRule = 3 THEN 'Non GST' END 			  
                                        ,CASE WHEN ISNULL(I.IsNonDiscountable,0) = 0 THEN 'No' ELSE 'Yes' END  
                                        ,CASE WHEN ISNULL(I.userfld9,'N') = 'N' THEN 'No' ELSE 'Yes' END  	
                                        ,CASE WHEN ISNULL(I.IsCommission,0) = 0 THEN 'No' ELSE 'Yes' END  
                                        ,CASE WHEN ISNULL(I.Userflag2,0) = 0 THEN 'No' ELSE 'Yes' END
                                        ,CASE WHEN I.IsServiceItem = 0 AND I.IsInInventory = 1 THEN 'Product'
                                              WHEN I.IsServiceItem = 1 AND I.IsInInventory = 1 THEN 'Open Price Product'
                                              WHEN I.IsServiceItem = 1 AND I.IsInInventory = 0 THEN 'Service'
                                              WHEN I.IsServiceItem = 0 AND I.IsInInventory = 0 AND I.userfld10 = 'D' THEN 'Point Package'
                                              WHEN I.IsServiceItem = 0 AND I.IsInInventory = 0 AND I.userfld10 = 'T' THEN 'Course Package'		
                                              {12}
                                        END 
                                        ,ISNULL(I.Userfloat1,0) 
                                        ,ISNULL(I.userfloat3,0) 
                                        ,I.ItemDesc 
                                        ,I.Attributes1
                                        ,I.Attributes2
                                        ,I.Attributes3
                                        ,I.Attributes4
                                        ,I.Attributes5
                                        ,I.Attributes6
                                        ,I.Attributes7
                                        ,I.Attributes8
                                        ,I.Remark
                                        ,CASE WHEN ISNULL(I.Deleted,0) = 0 THEN 'No' ELSE 'Yes' END
                                        ,CASE WHEN ISNULL(I.userflag1,0) = 0 THEN 'No' ELSE 'Yes' END
                                        ,CASE WHEN ISNULL(I.Userflag9,0) = 0 THEN 'No' ELSE 'Yes' END 
                                        ,I.Userfld7 
                                        ,I.Userfloat4 
                                        {14}
                                        {2}
                                        {3}
                                        {4}
                                        {5}
                                        {6}     
                                        {16}  
                                ORDER BY [Department Code] 
                                        ,[Department Name] 
                                        ,[Category Code]
                                        ,[Category Name]
                                        ,[Item No]";
                string priceLevel1 = "";
                string priceLevel1Name = "";
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice1), true))
                {
                    priceLevel1Name = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P1DiscountName);
                    if (string.IsNullOrEmpty(priceLevel1Name)) priceLevel1Name = "PriceLevel1";
                    priceLevel1Name = string.Format("[{0}]", priceLevel1Name);
                    priceLevel1 = ",ISNULL(I.Userfloat6,0)";
                }

                string priceLevel2 = "";
                string priceLevel2Name = "";
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice2), true))
                {
                    priceLevel2Name = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P2DiscountName);
                    if (string.IsNullOrEmpty(priceLevel2Name)) priceLevel2Name = "PriceLevel2";
                    priceLevel2Name = string.Format("[{0}]", priceLevel2Name);
                    priceLevel2 = ",ISNULL(I.Userfloat7,0)";
                }

                string priceLevel3 = "";
                string priceLevel3Name = "";
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice3), true))
                {
                    priceLevel3Name = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P3DiscountName);
                    if (string.IsNullOrEmpty(priceLevel3Name)) priceLevel3Name = "PriceLevel3";
                    priceLevel3Name = string.Format("[{0}]", priceLevel3Name);
                    priceLevel3 = ",ISNULL(I.Userfloat8,0)";
                }

                string priceLevel4 = "";
                string priceLevel4Name = "";
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice4), true))
                {
                    priceLevel4Name = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P4DiscountName);
                    if (string.IsNullOrEmpty(priceLevel4Name)) priceLevel4Name = "PriceLevel4";
                    priceLevel4Name = string.Format("[{0}]", priceLevel4Name);
                    priceLevel4 = ",ISNULL(I.Userfloat9,0)";
                }

                string priceLevel5 = "";
                string priceLevel5Name = "";
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice5), true))
                {
                    priceLevel5Name = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P5DiscountName);
                    if (string.IsNullOrEmpty(priceLevel5Name)) priceLevel5Name = "PriceLevel5";
                    priceLevel5Name = string.Format("[{0}]", priceLevel5Name);
                    priceLevel5 = ",ISNULL(I.Userfloat10,0)";
                }

                string queryNonInventory = "";
                string groupByDeduct = "";
                string queryByDeduct = "";
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayNonInventoryProduct), false))
                {
                    queryNonInventory = "WHEN I.IsServiceItem = 0 AND I.IsInInventory = 0 AND ISNULL(I.userflag7,0) = 1 THEN 'Non Inventory Product' ";
                    groupByDeduct = @",I.userfld8 
                                        ,I.userfloat5 
                                        ,CASE WHEN userflag8 IS NULL then ''
                                              WHEN userflag8 = 1 Then 'Down'
                                              ELSE 'Up' END ";
                    queryByDeduct = @",I.userfld8 as [Deduction Item No]
                                        ,I.userfloat5 as [Deduction Qty]
                                        ,CASE WHEN userflag8 IS NULL then ''
                                              WHEN userflag8 = 1 Then 'Down'
                                              ELSE 'Up' END as [Deduction Type]";
                }

                string queryMinimumPrice = "";
                string groupByMiminumPrice = "";
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayMinimumSellingPrice), false))
                {
                    queryMinimumPrice = ", I.MinimumPrice as [Minimum Price]";
                    groupByMiminumPrice = ", I.MinimumPrice";

                }

                sql = string.Format(sql, itemDeptID
                                       , category
                                       , priceLevel1
                                       , priceLevel2
                                       , priceLevel3
                                       , priceLevel4
                                       , priceLevel5
                                       , priceLevel1Name
                                       , priceLevel2Name
                                       , priceLevel3Name
                                       , priceLevel4Name
                                       , priceLevel5Name
                                       , queryNonInventory
                                       , queryByDeduct
                                       , groupByDeduct
                                       , queryMinimumPrice
                                       , groupByMiminumPrice);
                dt.Load(DataService.GetReader(new QueryCommand(sql)));

                var attribData = new AttributesLabelController().FetchAll()
                                                                .OrderBy(o => o.AttributesNo)
                                                                .ToList();
                for (int i = 0; i < 8; i++)
                {
                    if (i < attribData.Count && !string.IsNullOrEmpty(attribData[i].Label))
                        dt.Columns[i + 19].ColumnName = attribData[i].Label;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return dt;
        }

        public static DataTable FetchItemWithOutletOption(string itemDeptID, string category, string ApplicableTo, string OutletChosen)
        {
            DataTable dt = new DataTable();

            try
            {
                string sql = "";
                if (ApplicableTo == "Product Master")
                {
                    sql = @"
                            DECLARE @ItemDepartmentID NVARCHAR(MAX);
                            DECLARE @CategoryName NVARCHAR(MAX);
                            DECLARE @OutletName NVARCHAR(MAX);                            

                            SET @ItemDepartmentID = '{0}';
                            SET @CategoryName = '{1}';
                            SET @OutletName = '{12}'
                            SELECT    ID.ItemDepartmentID [Department Code]
                                        ,ID.DepartmentName [Department Name]
                                        ,C.Category_ID [Category Code]
                                        ,C.CategoryName [Category Name]
                                        ,I.ItemNo  [Item No]
                                        ,CASE WHEN ISNULL(I.userflag1,0) = 0 THEN I.ItemName ELSE I.Attributes2 END   [Item Name]
                                        ,I.Barcode
                                        ,I.RetailPrice [Retail Price]
                                        ,I.FactoryPrice [Cost Price]
                                        ,I.userfld1 UOM
                                        ,CASE WHEN I.GSTRule = 1 THEN 'Exclusive'
                                              WHEN I.GSTRule = 2 THEN 'Inclusive'
                                              WHEN I.GSTRule = 3 THEN 'Non GST' END [GST Rule]			  
                                        ,CASE WHEN ISNULL(I.IsNonDiscountable,0) = 0 THEN 'No' ELSE 'Yes' END  [Is Non Discountable]
                                        ,CASE WHEN ISNULL(I.userfld9,'N') = 'N' THEN 'No' ELSE 'Yes' END  [Point Redeemable]		
                                        ,CASE WHEN ISNULL(I.IsCommission,0) = 0 THEN 'No' ELSE 'Yes' END  [Give Commission]
                                        ,CASE WHEN ISNULL(I.Userflag2,0) = 0 THEN 'No' ELSE 'Yes' END  [Allow Pre Order]        
                                        ,CASE WHEN I.IsServiceItem = 0 AND I.IsInInventory = 1 THEN 'Product'
                                              WHEN I.IsServiceItem = 1 AND I.IsInInventory = 1 THEN 'Open Price Product'
                                              WHEN I.IsServiceItem = 1 AND I.IsInInventory = 0 THEN 'Service'
                                              WHEN I.IsServiceItem = 0 AND I.IsInInventory = 0 AND I.userfld10 = 'D' THEN 'Point Package'
                                              WHEN I.IsServiceItem = 0 AND I.IsInInventory = 0 AND I.userfld10 = 'T' THEN 'Course Package'	
                                              {13}		
                                        END [Item Type]	  
                                        ,ISNULL(I.Userfloat1,0) [Point Get]
                                        ,ISNULL(I.userfloat3,0) [Breakdown Price]
                                        ,I.ItemDesc Description
                                        ,I.Attributes1
                                        ,I.Attributes2
                                        ,I.Attributes3
                                        ,I.Attributes4
                                        ,I.Attributes5
                                        ,I.Attributes6
                                        ,I.Attributes7
                                        ,I.Attributes8
                                        ,I.Remark
                                        ,CASE WHEN ISNULL(I.Deleted,0) = 0 THEN 'No' ELSE 'Yes' END [Deleted]		
                                        ,MAX(SUP.SupplierName) [Supplier Name]		
                                        ,CASE WHEN ISNULL(I.userflag1,0) = 0 THEN 'No' ELSE 'Yes' END  [Is Matrix Item]	
                                        ,CASE WHEN ISNULL(I.Userflag9,0) = 0 THEN 'No' ELSE 'Yes' END [Is Using Fixed Value for COG]
                                        ,I.Userfld7 as [Fixed COG Type]
                                        ,I.Userfloat4 as [Fix COG Value]
                                        {14}
                                        {2} {7}	
                                        {3} {8}
                                        {4} {9}
                                        {5} {10}
                                        {6} {11}
                                        {16}
                                FROM	Item I
                                        LEFT JOIN Category C ON C.CategoryName = I.CategoryName
                                        LEFT JOIN ItemDepartment ID ON ID.ItemDepartmentID = C.ItemDepartmentId
                                        LEFT JOIN ItemSupplierMap ISM ON ISM.ItemNo = I.ItemNo AND ISNULL(ISM.Deleted,0) = 0
                                        LEFT JOIN Supplier SUP ON SUP.SupplierID = ISM.SupplierID		
                                WHERE	ID.DepartmentName <> 'SYSTEM'
                                        AND (ID.ItemDepartmentID = @ItemDepartmentID OR @ItemDepartmentID = 'ALL')
                                        AND (C.CategoryName = @CategoryName OR @CategoryName = 'ALL')
                                GROUP BY ID.ItemDepartmentID 
                                        ,ID.DepartmentName 
                                        ,C.Category_ID
                                        ,C.CategoryName 
                                        ,I.ItemNo
                                        ,CASE WHEN ISNULL(I.userflag1,0) = 0 THEN I.ItemName ELSE I.Attributes2 END
                                        ,I.Barcode
                                        ,I.RetailPrice 
                                        ,I.FactoryPrice 
                                        ,I.userfld1 
                                        ,CASE WHEN I.GSTRule = 1 THEN 'Exclusive'
                                              WHEN I.GSTRule = 2 THEN 'Inclusive'
                                              WHEN I.GSTRule = 3 THEN 'Non GST' END 			  
                                        ,CASE WHEN ISNULL(I.IsNonDiscountable,0) = 0 THEN 'No' ELSE 'Yes' END  
                                        ,CASE WHEN ISNULL(I.userfld9,'N') = 'N' THEN 'No' ELSE 'Yes' END  	
                                        ,CASE WHEN ISNULL(I.IsCommission,0) = 0 THEN 'No' ELSE 'Yes' END  
                                        ,CASE WHEN ISNULL(I.Userflag2,0) = 0 THEN 'No' ELSE 'Yes' END
                                        ,CASE WHEN I.IsServiceItem = 0 AND I.IsInInventory = 1 THEN 'Product'
                                              WHEN I.IsServiceItem = 1 AND I.IsInInventory = 1 THEN 'Open Price Product'
                                              WHEN I.IsServiceItem = 1 AND I.IsInInventory = 0 THEN 'Service'
                                              WHEN I.IsServiceItem = 0 AND I.IsInInventory = 0 AND I.userfld10 = 'D' THEN 'Point Package'
                                              WHEN I.IsServiceItem = 0 AND I.IsInInventory = 0 AND I.userfld10 = 'T' THEN 'Course Package'		
                                              {13}	
                                        END 
                                        ,ISNULL(I.Userfloat1,0) 
                                        ,ISNULL(I.userfloat3,0) 
                                        ,I.ItemDesc 
                                        ,I.Attributes1
                                        ,I.Attributes2
                                        ,I.Attributes3
                                        ,I.Attributes4
                                        ,I.Attributes5
                                        ,I.Attributes6
                                        ,I.Attributes7
                                        ,I.Attributes8
                                        ,I.Remark
                                        ,CASE WHEN ISNULL(I.Deleted,0) = 0 THEN 'No' ELSE 'Yes' END
                                        ,CASE WHEN ISNULL(I.userflag1,0) = 0 THEN 'No' ELSE 'Yes' END
                                        ,CASE WHEN ISNULL(I.Userflag9,0) = 0 THEN 'No' ELSE 'Yes' END 
                                        ,I.Userfld7 
                                        ,I.Userfloat4 
                                        {15}
                                        {2}
                                        {3}
                                        {4}
                                        {5}
                                        {6}     
                                        {17}  
                                ORDER BY [Department Code] 
                                        ,[Department Name] 
                                        ,[Category Code]
                                        ,[Category Name]
                                        ,[Item No]";
                }
                else if (ApplicableTo == "Outlet Group")
                {
                    sql = @"
                            DECLARE @ItemDepartmentID NVARCHAR(MAX);
                            DECLARE @CategoryName NVARCHAR(MAX);
                            DECLARE @OutletGroupID INT

                            SET @ItemDepartmentID = '{0}';
                            SET @CategoryName = '{1}';
                            SET @OutletGroupID = {12}
                            SELECT    ID.ItemDepartmentID [Department Code]
                                        ,ID.DepartmentName [Department Name]
                                        ,C.Category_ID [Category Code]
                                        ,C.CategoryName [Category Name]
                                        ,I.ItemNo  [Item No]
                                        ,CASE WHEN ISNULL(I.userflag1,0) = 0 THEN I.ItemName ELSE I.Attributes2 END   [Item Name]
                                        ,I.Barcode
                                        ,CASE WHEN OGIM.OutletGroupItemMapID IS NULL THEN I.RetailPrice ELSE OGIM.RetailPrice END [Retail Price]
                                        ,CASE WHEN OGIM.OutletGroupItemMapID IS NULL THEN I.FactoryPrice ELSE OGIM.CostPrice END [Cost Price]
                                        ,I.userfld1 UOM
                                        ,CASE WHEN I.GSTRule = 1 THEN 'Exclusive'
                                              WHEN I.GSTRule = 2 THEN 'Inclusive'
                                              WHEN I.GSTRule = 3 THEN 'Non GST' END [GST Rule]			  
                                        ,CASE WHEN ISNULL(I.IsNonDiscountable,0) = 0 THEN 'No' ELSE 'Yes' END  [Is Non Discountable]
                                        ,CASE WHEN ISNULL(I.userfld9,'N') = 'N' THEN 'No' ELSE 'Yes' END  [Point Redeemable]		
                                        ,CASE WHEN ISNULL(I.IsCommission,0) = 0 THEN 'No' ELSE 'Yes' END  [Give Commission]
                                        ,CASE WHEN ISNULL(I.Userflag2,0) = 0 THEN 'No' ELSE 'Yes' END  [Allow Pre Order]        
                                        ,CASE WHEN I.IsServiceItem = 0 AND I.IsInInventory = 1 THEN 'Product'
                                              WHEN I.IsServiceItem = 1 AND I.IsInInventory = 1 THEN 'Open Price Product'
                                              WHEN I.IsServiceItem = 1 AND I.IsInInventory = 0 THEN 'Service'
                                              WHEN I.IsServiceItem = 0 AND I.IsInInventory = 0 AND I.userfld10 = 'D' THEN 'Point Package'
                                              WHEN I.IsServiceItem = 0 AND I.IsInInventory = 0 AND I.userfld10 = 'T' THEN 'Course Package'
                                              {13}		
                                        END [Item Type]	  
                                        ,ISNULL(I.Userfloat1,0) [Point Get]
                                        ,ISNULL(I.userfloat3,0) [Breakdown Price]
                                        ,I.ItemDesc Description
                                        ,I.Attributes1
                                        ,I.Attributes2
                                        ,I.Attributes3
                                        ,I.Attributes4
                                        ,I.Attributes5
                                        ,I.Attributes6
                                        ,I.Attributes7
                                        ,I.Attributes8
                                        ,I.Remark
                                        ,CASE WHEN OGIM.OutletGroupItemMapID IS NULL THEN CASE WHEN ISNULL(I.Deleted,0) = 0 THEN 'No' ELSE 'Yes' END ELSE CASE WHEN ISNULL(OGIM.Deleted,0) = 0 THEN 'No' ELSE 'Yes' END END [Deleted]
                                        ,MAX(SUP.SupplierName) [Supplier Name]		
                                        ,CASE WHEN ISNULL(I.userflag1,0) = 0 THEN 'No' ELSE 'Yes' END  [Is Matrix Item]	
                                        ,CASE WHEN ISNULL(I.Userflag9,0) = 0 THEN 'No' ELSE 'Yes' END [Is Using Fixed Value for COG]
                                        ,I.Userfld7 as [Fixed COG Type]
                                        ,I.Userfloat4 as [Fix COG Value]
                                        {14}
                                        {2} {7}	
                                        {3} {8}
                                        {4} {9}
                                        {5} {10}
                                        {6} {11}
                                        {16}
                                FROM	Item I
                                        LEFT JOIN Category C ON C.CategoryName = I.CategoryName
                                        LEFT JOIN ItemDepartment ID ON ID.ItemDepartmentID = C.ItemDepartmentId
                                        LEFT JOIN ItemSupplierMap ISM ON ISM.ItemNo = I.ItemNo AND ISNULL(ISM.Deleted,0) = 0
                                        LEFT JOIN Supplier SUP ON SUP.SupplierID = ISM.SupplierID		
                                        LEFT JOIN OutletgroupItemMap OGIM ON OGIM.ItemNo = I.ItemNo AND OGIM.OutletGroupID = @OutletGroupID
                                WHERE	ID.DepartmentName <> 'SYSTEM'
                                        AND (ID.ItemDepartmentID = @ItemDepartmentID OR @ItemDepartmentID = 'ALL')
                                        AND (C.CategoryName = @CategoryName OR @CategoryName = 'ALL')
                                GROUP BY ID.ItemDepartmentID 
                                        ,ID.DepartmentName 
                                        ,C.Category_ID
                                        ,C.CategoryName 
                                        ,I.ItemNo
                                        ,CASE WHEN ISNULL(I.userflag1,0) = 0 THEN I.ItemName ELSE I.Attributes2 END
                                        ,I.Barcode
                                        ,I.RetailPrice 
                                        ,I.FactoryPrice 
                                        ,I.userfld1 
                                        ,CASE WHEN I.GSTRule = 1 THEN 'Exclusive'
                                              WHEN I.GSTRule = 2 THEN 'Inclusive'
                                              WHEN I.GSTRule = 3 THEN 'Non GST' END 			  
                                        ,CASE WHEN ISNULL(I.IsNonDiscountable,0) = 0 THEN 'No' ELSE 'Yes' END  
                                        ,CASE WHEN ISNULL(I.userfld9,'N') = 'N' THEN 'No' ELSE 'Yes' END  	
                                        ,CASE WHEN ISNULL(I.IsCommission,0) = 0 THEN 'No' ELSE 'Yes' END  
                                        ,CASE WHEN ISNULL(I.Userflag2,0) = 0 THEN 'No' ELSE 'Yes' END
                                        ,CASE WHEN I.IsServiceItem = 0 AND I.IsInInventory = 1 THEN 'Product'
                                              WHEN I.IsServiceItem = 1 AND I.IsInInventory = 1 THEN 'Open Price Product'
                                              WHEN I.IsServiceItem = 1 AND I.IsInInventory = 0 THEN 'Service'
                                              WHEN I.IsServiceItem = 0 AND I.IsInInventory = 0 AND I.userfld10 = 'D' THEN 'Point Package'
                                              WHEN I.IsServiceItem = 0 AND I.IsInInventory = 0 AND I.userfld10 = 'T' THEN 'Course Package'	
                                              {13}		
                                        END 
                                        ,ISNULL(I.Userfloat1,0) 
                                        ,ISNULL(I.userfloat3,0) 
                                        ,I.ItemDesc 
                                        ,I.Attributes1
                                        ,I.Attributes2
                                        ,I.Attributes3
                                        ,I.Attributes4
                                        ,I.Attributes5
                                        ,I.Attributes6
                                        ,I.Attributes7
                                        ,I.Attributes8
                                        ,I.Remark
                                        ,CASE WHEN ISNULL(I.Deleted,0) = 0 THEN 'No' ELSE 'Yes' END
                                        ,CASE WHEN ISNULL(I.userflag1,0) = 0 THEN 'No' ELSE 'Yes' END
                                        ,OGIM.OutletGroupItemMapID
			                            ,OGIM.RetailPrice   
			                            ,OGIM.CostPrice
                                        ,OGIM.Deleted  
                                        ,CASE WHEN ISNULL(I.Userflag9,0) = 0 THEN 'No' ELSE 'Yes' END 
                                        ,I.Userfld7 
                                        ,I.Userfloat4 
                                        {15}
                                        {2}
                                        {3}
                                        {4}
                                        {5}
                                        {6}   
                                        {17}    
                                ORDER BY [Department Code] 
                                        ,[Department Name] 
                                        ,[Category Code]
                                        ,[Category Name]
                                        ,[Item No]";
                }
                else
                {
                    sql = @"
                            DECLARE @ItemDepartmentID NVARCHAR(MAX);
                            DECLARE @CategoryName NVARCHAR(MAX);
                            DECLARE @OutletName NVARCHAR(MAX)

                            SET @ItemDepartmentID = '{0}';
                            SET @CategoryName = '{1}';
                            SET @OutletName = '{12}'
                            SELECT    ID.ItemDepartmentID [Department Code]
                                        ,ID.DepartmentName [Department Name]
                                        ,C.Category_ID [Category Code]
                                        ,C.CategoryName [Category Name]
                                        ,I.ItemNo  [Item No]
                                        ,CASE WHEN ISNULL(I.userflag1,0) = 0 THEN I.ItemName ELSE I.Attributes2 END   [Item Name]
                                        ,I.Barcode
                                        ,CASE WHEN OGIM.OutletGroupItemMapID IS NULL THEN I.RetailPrice ELSE OGIM.RetailPrice END [Retail Price]
                                        ,CASE WHEN OGIM.OutletGroupItemMapID IS NULL THEN I.FactoryPrice ELSE OGIM.CostPrice END [Cost Price]
                                        ,I.userfld1 UOM
                                        ,CASE WHEN I.GSTRule = 1 THEN 'Exclusive'
                                              WHEN I.GSTRule = 2 THEN 'Inclusive'
                                              WHEN I.GSTRule = 3 THEN 'Non GST' END [GST Rule]			  
                                        ,CASE WHEN ISNULL(I.IsNonDiscountable,0) = 0 THEN 'No' ELSE 'Yes' END  [Is Non Discountable]
                                        ,CASE WHEN ISNULL(I.userfld9,'N') = 'N' THEN 'No' ELSE 'Yes' END  [Point Redeemable]		
                                        ,CASE WHEN ISNULL(I.IsCommission,0) = 0 THEN 'No' ELSE 'Yes' END  [Give Commission]
                                        ,CASE WHEN ISNULL(I.Userflag2,0) = 0 THEN 'No' ELSE 'Yes' END  [Allow Pre Order]        
                                        ,CASE WHEN I.IsServiceItem = 0 AND I.IsInInventory = 1 THEN 'Product'
                                              WHEN I.IsServiceItem = 1 AND I.IsInInventory = 1 THEN 'Open Price Product'
                                              WHEN I.IsServiceItem = 1 AND I.IsInInventory = 0 THEN 'Service'
                                              WHEN I.IsServiceItem = 0 AND I.IsInInventory = 0 AND I.userfld10 = 'D' THEN 'Point Package'
                                              WHEN I.IsServiceItem = 0 AND I.IsInInventory = 0 AND I.userfld10 = 'T' THEN 'Course Package'	
                                              {13}	
                                        END [Item Type]	  
                                        ,ISNULL(I.Userfloat1,0) [Point Get]
                                        ,ISNULL(I.userfloat3,0) [Breakdown Price]
                                        ,I.ItemDesc Description
                                        ,I.Attributes1
                                        ,I.Attributes2
                                        ,I.Attributes3
                                        ,I.Attributes4
                                        ,I.Attributes5
                                        ,I.Attributes6
                                        ,I.Attributes7
                                        ,I.Attributes8
                                        ,I.Remark
                                        ,CASE WHEN OGIM.OutletGroupItemMapID IS NULL THEN CASE WHEN ISNULL(I.Deleted,0) = 0 THEN 'No' ELSE 'Yes' END ELSE CASE WHEN ISNULL(OGIM.Deleted,0) = 0 THEN 'No' ELSE 'Yes' END END [Deleted]		
                                        ,MAX(SUP.SupplierName) [Supplier Name]		
                                        ,CASE WHEN ISNULL(I.userflag1,0) = 0 THEN 'No' ELSE 'Yes' END  [Is Matrix Item]	
                                        ,CASE WHEN ISNULL(I.Userflag9,0) = 0 THEN 'No' ELSE 'Yes' END [Is Using Fixed Value for COG]
                                        ,I.Userfld7 as [Fixed COG Type]
                                        ,I.Userfloat4 as [Fix COG Value]
                                        {14}
                                        {2} {7}	
                                        {3} {8}
                                        {4} {9}
                                        {5} {10}
                                        {6} {11}
                                        {16} 
                                FROM	Item I
                                        LEFT JOIN Category C ON C.CategoryName = I.CategoryName
                                        LEFT JOIN ItemDepartment ID ON ID.ItemDepartmentID = C.ItemDepartmentId
                                        LEFT JOIN ItemSupplierMap ISM ON ISM.ItemNo = I.ItemNo AND ISNULL(ISM.Deleted,0) = 0
                                        LEFT JOIN Supplier SUP ON SUP.SupplierID = ISM.SupplierID		
                                        LEFT JOIN OutletgroupItemMap OGIM ON OGIM.ItemNo = I.ItemNo AND OGIM.OutletName = @OutletName
                                WHERE	ID.DepartmentName <> 'SYSTEM'
                                        AND (ID.ItemDepartmentID = @ItemDepartmentID OR @ItemDepartmentID = 'ALL')
                                        AND (C.CategoryName = @CategoryName OR @CategoryName = 'ALL')
                                GROUP BY ID.ItemDepartmentID 
                                        ,ID.DepartmentName 
                                        ,C.Category_ID
                                        ,C.CategoryName 
                                        ,I.ItemNo
                                        ,CASE WHEN ISNULL(I.userflag1,0) = 0 THEN I.ItemName ELSE I.Attributes2 END
                                        ,I.Barcode
                                        ,I.RetailPrice 
                                        ,I.FactoryPrice 
                                        ,I.userfld1 
                                        ,CASE WHEN I.GSTRule = 1 THEN 'Exclusive'
                                              WHEN I.GSTRule = 2 THEN 'Inclusive'
                                              WHEN I.GSTRule = 3 THEN 'Non GST' END 			  
                                        ,CASE WHEN ISNULL(I.IsNonDiscountable,0) = 0 THEN 'No' ELSE 'Yes' END  
                                        ,CASE WHEN ISNULL(I.userfld9,'N') = 'N' THEN 'No' ELSE 'Yes' END  	
                                        ,CASE WHEN ISNULL(I.IsCommission,0) = 0 THEN 'No' ELSE 'Yes' END  
                                        ,CASE WHEN ISNULL(I.Userflag2,0) = 0 THEN 'No' ELSE 'Yes' END
                                        ,CASE WHEN I.IsServiceItem = 0 AND I.IsInInventory = 1 THEN 'Product'
                                              WHEN I.IsServiceItem = 1 AND I.IsInInventory = 1 THEN 'Open Price Product'
                                              WHEN I.IsServiceItem = 1 AND I.IsInInventory = 0 THEN 'Service'
                                              WHEN I.IsServiceItem = 0 AND I.IsInInventory = 0 AND I.userfld10 = 'D' THEN 'Point Package'
                                              WHEN I.IsServiceItem = 0 AND I.IsInInventory = 0 AND I.userfld10 = 'T' THEN 'Course Package'	
                                              {13}		
                                        END 
                                        ,ISNULL(I.Userfloat1,0) 
                                        ,ISNULL(I.userfloat3,0) 
                                        ,I.ItemDesc 
                                        ,I.Attributes1
                                        ,I.Attributes2
                                        ,I.Attributes3
                                        ,I.Attributes4
                                        ,I.Attributes5
                                        ,I.Attributes6
                                        ,I.Attributes7
                                        ,I.Attributes8
                                        ,I.Remark
                                        ,CASE WHEN ISNULL(I.Deleted,0) = 0 THEN 'No' ELSE 'Yes' END
                                        ,CASE WHEN ISNULL(I.userflag1,0) = 0 THEN 'No' ELSE 'Yes' END
                                        ,OGIM.OutletGroupItemMapID
			                            ,OGIM.RetailPrice   
			                            ,OGIM.CostPrice 
                                        ,OGIM.Deleted 
                                        ,CASE WHEN ISNULL(I.Userflag9,0) = 0 THEN 'No' ELSE 'Yes' END 
                                        ,I.Userfld7 
                                        ,I.Userfloat4 
                                        {15}
                                        {2}
                                        {3}
                                        {4}
                                        {5}
                                        {6}      
                                        {17} 
                                ORDER BY [Department Code] 
                                        ,[Department Name] 
                                        ,[Category Code]
                                        ,[Category Name]
                                        ,[Item No]";
                }
                string priceLevel1 = "";
                string priceLevel1Name = "";
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice1), true))
                {
                    priceLevel1Name = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P1DiscountName);
                    if (string.IsNullOrEmpty(priceLevel1Name)) priceLevel1Name = "PriceLevel1";
                    priceLevel1Name = string.Format("[{0}]", priceLevel1Name);
                    if (ApplicableTo == "Product Master")
                        priceLevel1 = ",ISNULL(I.Userfloat6,0)";
                    else
                        priceLevel1 = ",CASE WHEN OGIM.OutletGroupItemMapID IS NULL THEN I.Userfloat6 ELSE OGIM.P1 END ";
                }

                string priceLevel2 = "";
                string priceLevel2Name = "";
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice2), true))
                {
                    priceLevel2Name = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P2DiscountName);
                    if (string.IsNullOrEmpty(priceLevel2Name)) priceLevel2Name = "PriceLevel2";
                    priceLevel2Name = string.Format("[{0}]", priceLevel2Name);
                    if (ApplicableTo == "Product Master")
                        priceLevel2 = ",ISNULL(I.Userfloat7,0)";
                    else
                        priceLevel2 = ",CASE WHEN OGIM.OutletGroupItemMapID IS NULL THEN I.Userfloat7 ELSE OGIM.P2 END ";
                }

                string priceLevel3 = "";
                string priceLevel3Name = "";
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice3), true))
                {
                    priceLevel3Name = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P3DiscountName);
                    if (string.IsNullOrEmpty(priceLevel3Name)) priceLevel3Name = "PriceLevel3";
                    priceLevel3Name = string.Format("[{0}]", priceLevel3Name);
                    if (ApplicableTo == "Product Master")
                        priceLevel3 = ",ISNULL(I.Userfloat8,0)";
                    else
                        priceLevel3 = ",CASE WHEN OGIM.OutletGroupItemMapID IS NULL THEN I.Userfloat8 ELSE OGIM.P3 END ";
                }

                string priceLevel4 = "";
                string priceLevel4Name = "";
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice4), true))
                {
                    priceLevel4Name = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P4DiscountName);
                    if (string.IsNullOrEmpty(priceLevel4Name)) priceLevel4Name = "PriceLevel4";
                    priceLevel4Name = string.Format("[{0}]", priceLevel4Name);
                    if (ApplicableTo == "Product Master")
                        priceLevel4 = ",ISNULL(I.Userfloat9,0)";
                    else
                        priceLevel4 = ",CASE WHEN OGIM.OutletGroupItemMapID IS NULL THEN I.Userfloat9 ELSE OGIM.P4 END ";
                }

                string priceLevel5 = "";
                string priceLevel5Name = "";
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice5), true))
                {
                    priceLevel5Name = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P5DiscountName);
                    if (string.IsNullOrEmpty(priceLevel5Name)) priceLevel5Name = "PriceLevel5";
                    priceLevel5Name = string.Format("[{0}]", priceLevel5Name);
                    if (ApplicableTo == "Product Master")
                        priceLevel5 = ",ISNULL(I.Userfloat10,0)";
                    else
                        priceLevel5 = ",CASE WHEN OGIM.OutletGroupItemMapID IS NULL THEN I.Userfloat10 ELSE OGIM.P5 END ";
                }


                string queryNonInventory = "";
                string groupByDeduct = "";
                string queryByDeduct = "";
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayNonInventoryProduct), false))
                {
                    queryNonInventory = "WHEN I.IsServiceItem = 0 AND I.IsInInventory = 0 AND ISNULL(I.userflag7,0) = 1 THEN 'Non Inventory Product' ";
                    groupByDeduct = @",I.userfld8 
                                        ,I.userfloat5 
                                        ,CASE WHEN userflag8 IS NULL then ''
                                              WHEN userflag8 = 1 Then 'Down'
                                              ELSE 'Up' END ";
                    queryByDeduct = @",I.userfld8 as [Deduction Item No]
                                        ,I.userfloat5 as [Deduction Qty]
                                        ,CASE WHEN userflag8 IS NULL then ''
                                              WHEN userflag8 = 1 Then 'Down'
                                              ELSE 'Up' END as [Deduction Type]";
                }

                string queryMinimumPrice = "";
                string groupByMiminumPrice = "";
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayMinimumSellingPrice), false))
                {
                    queryMinimumPrice = ", I.MinimumPrice as [Minimum Price]";
                    groupByMiminumPrice = ", I.MinimumPrice";

                }

                sql = string.Format(sql, itemDeptID
                                       , category
                                       , priceLevel1
                                       , priceLevel2
                                       , priceLevel3
                                       , priceLevel4
                                       , priceLevel5
                                       , priceLevel1Name
                                       , priceLevel2Name
                                       , priceLevel3Name
                                       , priceLevel4Name
                                       , priceLevel5Name
                                       , OutletChosen
                                       , queryNonInventory
                                       , queryByDeduct
                                       , groupByDeduct
                                       , queryMinimumPrice
                                       , groupByMiminumPrice);
                dt.Load(DataService.GetReader(new QueryCommand(sql)));

                var attribData = new AttributesLabelController().FetchAll()
                                                                .OrderBy(o => o.AttributesNo)
                                                                .ToList();
                for (int i = 0; i < 8; i++)
                {
                    if (i < attribData.Count && !string.IsNullOrEmpty(attribData[i].Label))
                        dt.Columns[i + 19].ColumnName = attribData[i].Label;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return dt;
        }

        public static bool ImportData(string userName, DataTable data, out DataTable result, out string message)
        {
            bool isSuccess = false;
            result = data.Copy();
            message = "";
            bool displaySupplier = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplaySupplier), true);
            try
            {
                result.Columns.Add("Status", typeof(string));
                Dictionary<string, string> itemDeptData = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                Dictionary<string, KeyValuePair<string, string>> categoryData = new Dictionary<string, KeyValuePair<string, string>>(StringComparer.OrdinalIgnoreCase);
                QueryCommandCollection qmc = new QueryCommandCollection();

                DataTable counterMatrix = new DataTable();
                counterMatrix.Columns.Add("Attributes1", typeof(string));
                counterMatrix.Columns.Add("Counter", typeof(Int32));

                #region *) Validation
                List<string> theItemNos = new List<string>();
                List<string> theBarcodes = new List<string>();
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    bool isValid = true;
                    string itemNo = (string)result.Rows[i]["Item No"];
                    string parentItemNo = (string)result.Rows[i]["Attributes1"];
                    string barcode = (string)result.Rows[i]["Barcode"];
                    string supplier = (string)result.Rows[i]["Supplier Name"];
                    string itemDeptID = (string)result.Rows[i]["Department Code"];
                    string itemDeptName = (string)result.Rows[i]["Department Name"];
                    string categoryID = (string)result.Rows[i]["Category Code"];
                    string categoryName = (string)result.Rows[i]["Category Name"];
                    string itemType = (string)result.Rows[i]["Item Type"];
                    bool isMatrixMode = ((string)result.Rows[i]["Is Matrix Item"] + "").GetBoolValue(false);
                    bool isUseFixedValueCOG = ((string)result.Rows[i]["Is Using Fixed Value for COG"] + "").GetBoolValue(false);
                    string FixedCOGType = ((string)result.Rows[i]["Fixed COG Type"]);
                    string FixCOGValue = ((string)result.Rows[i]["Fix COG Value"]);
                    string status = "";

                    if (string.IsNullOrEmpty(categoryName))
                    {
                        status = "- Category Cannot Empty\n";
                        isValid = false;
                    }
                    if (string.IsNullOrEmpty(itemType))
                    {
                        status = "- Item Type Cannot Empty\n";
                        isValid = false;
                    }
                    if (isMatrixMode)
                    {
                        if (string.IsNullOrEmpty(parentItemNo))
                        {
                            status = "- Parent Item No Cannot Empty if use Matrix Mode\n";
                            isValid = false;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(itemNo))
                        {
                            status = "- ItemNo Cannot Empty\n";
                            isValid = false;
                        }

                        if (theItemNos.Contains(itemNo.Trim()))
                        {
                            status = "- Duplicated ItemNo\n";
                            isValid = false;
                        }
                        if (!string.IsNullOrEmpty(barcode.Trim()) && theBarcodes.Contains(barcode.Trim()))
                        {
                            status = "- Duplicated Barcode\n";
                            isValid = false;
                        }
                        if (!string.IsNullOrEmpty(barcode.Trim()))
                        {
                            var existingBarcode = new Item(Item.Columns.Barcode, barcode);
                            if (!existingBarcode.IsNew && existingBarcode.ItemNo != itemNo)
                            {
                                status = "- Another item with same barcode already exist\n";
                                isValid = false;
                            }
                        }
                    }
                    if (displaySupplier)
                    {
                        if (!string.IsNullOrEmpty(supplier))
                        {
                            var supp = new Supplier(Supplier.Columns.SupplierName, supplier.Trim());
                            if (supp.IsNew)
                            {
                                status = "- Supplier not found";
                                isValid = false;
                            }
                        }
                    }

                    if (isUseFixedValueCOG)
                    { 
                        if(string.IsNullOrEmpty(FixedCOGType) || string.IsNullOrEmpty(FixCOGValue))
                        {
                            status = "- Fix COG Type and Fix COG Value can not be empty";
                            isValid = false;
                        }
                    }

                    result.Rows[i]["Status"] = status;
                    theItemNos.Add(itemNo.Trim());
                    if (!string.IsNullOrEmpty(barcode.Trim()))
                        theBarcodes.Add(barcode.Trim());

                    if (isValid)
                    {
                        if (!string.IsNullOrEmpty(itemDeptID) && !itemDeptData.ContainsKey(itemDeptID.Trim()))
                            itemDeptData.Add(itemDeptID.Trim(), itemDeptName);
                        if (!string.IsNullOrEmpty(categoryName) && !categoryData.ContainsKey(categoryName.Trim()))
                            categoryData.Add(categoryName.Trim(), new KeyValuePair<string, string>(itemDeptID, categoryID));
                    }
                }

                #endregion

                #region *) ItemDepartment

                foreach (var itemDept in itemDeptData)
                {
                    ItemDepartment id = new ItemDepartment(ItemDepartment.Columns.ItemDepartmentID, itemDept.Key);
                    if (id.IsNew)
                        id.ItemDepartmentID = itemDept.Key;
                    id.DepartmentName = itemDept.Value;
                    id.Deleted = false;
                    if (id.IsNew)
                        qmc.Add(id.GetInsertCommand(string.Format("{0} via WEB Item Importer", userName)));
                    else
                        qmc.Add(id.GetUpdateCommand(string.Format("{0} via WEB Item Importer", userName)));
                }

                #endregion

                #region *) Category

                foreach (var ctgItem in categoryData)
                {
                    Category ctg = new Category(Category.Columns.CategoryName, ctgItem.Key);
                    if (ctg.IsNew)
                        ctg.CategoryName = ctgItem.Key;
                    ctg.ItemDepartmentId = ctgItem.Value.Key;
                    ctg.CategoryId = ctgItem.Value.Value;
                    ctg.Deleted = false;
                    if (ctg.IsNew)
                        qmc.Add(ctg.GetInsertCommand(string.Format("{0} via WEB Item Importer", userName)));
                    else
                        qmc.Add(ctg.GetUpdateCommand(string.Format("{0} via WEB Item Importer", userName)));
                }

                #endregion

                #region *) Item & Supplier Map
                int matrixCount = 0;
                List<string> createdDiscount = new List<string>();
                List<string> createdAttribute3 = new List<string>();
                List<string> createdAttribute4 = new List<string>();
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    string err = (string)result.Rows[i]["Status"];
                    bool isMatrixMode = ((string)result.Rows[i]["Is Matrix Item"] + "").GetBoolValue(false);
                    if (string.IsNullOrEmpty(err))
                    {
                        #region *) Item

                        var theItem = new Item(Item.Columns.ItemNo, (string)result.Rows[i]["Item No"]);
                        if (theItem.IsNew)
                        {
                            theItem.UniqueID = Guid.NewGuid();
                            theItem.ItemNo = (string)result.Rows[i]["Item No"];
                        }
                        theItem.CategoryName = (string)result.Rows[i]["Category Name"];
                        theItem.ItemName = (string)result.Rows[i]["Item Name"];
                        theItem.Barcode = (string)result.Rows[i]["Barcode"];
                        theItem.RetailPrice = ((string)result.Rows[i]["Retail Price"]).GetDecimalValue();
                        theItem.FactoryPrice = ((string)result.Rows[i]["Cost Price"]).GetDecimalValue();
                        theItem.Userfld1 = (string)result.Rows[i]["UOM"];
                        theItem.IsNonDiscountable = ((string)result.Rows[i]["Is Non Discountable"]).ToUpper().Equals("YES");
                        theItem.Userfld9 = ((string)result.Rows[i]["Point Redeemable"]).ToUpper().Equals("YES") ? "D" : "N";
                        theItem.IsCommission = ((string)result.Rows[i]["Give Commission"]).ToUpper().Equals("YES");
                        theItem.Userflag2 = ((string)result.Rows[i]["Allow Pre Order"]).ToUpper().Equals("YES");

                        string gstRule = ((string)result.Rows[i]["GST Rule"] + "").ToLower();
                        if (gstRule == "exclusive")
                            theItem.GSTRule = 1;
                        else if (gstRule == "inclusive")
                            theItem.GSTRule = 2;
                        else
                            theItem.GSTRule = 3;

                        string itemType = ((string)result.Rows[i]["Item Type"] + "").ToLower();
                        if (itemType == "product")
                        {
                            theItem.IsServiceItem = false;
                            theItem.IsInInventory = true;
                            theItem.Userfld10 = "N";
                        }
                        else if (itemType == "open price product")
                        {
                            theItem.IsServiceItem = true;
                            theItem.IsInInventory = true;
                            theItem.Userfld10 = "N";
                        }
                        else if (itemType == "service")
                        {
                            theItem.IsServiceItem = true;
                            theItem.IsInInventory = false;
                            theItem.Userfld10 = "N";
                        }
                        else if (itemType == "point package")
                        {
                            theItem.IsServiceItem = false;
                            theItem.IsInInventory = false;
                            theItem.Userfld10 = "D";
                            theItem.Userfloat1 = ((string)result.Rows[i]["Point Get"]).GetDecimalValue();
                        }
                        else if (itemType == "course package")
                        {
                            theItem.IsServiceItem = false;
                            theItem.IsInInventory = false;
                            theItem.Userfld10 = "T";
                            theItem.Userfloat1 = ((string)result.Rows[i]["Point Get"]).GetDecimalValue();
                            theItem.Userfloat3 = ((string)result.Rows[i]["Breakdown Price"]).GetDecimalValue();
                        }
                        theItem.ItemDesc = (string)result.Rows[i]["Description"];
                        theItem.Attributes1 = (string)result.Rows[i]["Attributes1"];
                        theItem.Attributes2 = (string)result.Rows[i]["Attributes2"];
                        theItem.Attributes3 = (string)result.Rows[i]["Attributes3"];
                        theItem.Attributes4 = (string)result.Rows[i]["Attributes4"];
                        theItem.Attributes5 = (string)result.Rows[i]["Attributes5"];
                        theItem.Attributes6 = (string)result.Rows[i]["Attributes6"];
                        theItem.Attributes7 = (string)result.Rows[i]["Attributes7"];
                        theItem.Attributes8 = (string)result.Rows[i]["Attributes8"];
                        theItem.Remark = (string)result.Rows[i]["Remark"];
                        theItem.Deleted = ((string)result.Rows[i]["Deleted"]).ToUpper().Equals("YES");

                        theItem.IsUsingFixedCOG = ((string)result.Rows[i]["Is Using Fixed Value for COG"] + "").GetBoolValue(false);
                        if(theItem.IsUsingFixedCOG){
                            theItem.FixedCOGType = (string)result.Rows[i]["Fixed COG Type"];
                            theItem.FixedCOGValue = ((string)result.Rows[i]["Fix COG Value"]  + "").GetDecimalValue();
                        }else{
                            theItem.FixedCOGType = "";
                            theItem.FixedCOGValue  = 0;
                        }

                        if (isMatrixMode)
                        {
                            if (theItem.IsNew)
                            {
                                int runningnumber = ItemController.getNewRunningNumberMatrix(theItem.Attributes1);

                                var matrixcounter = 0;
                                DataRow[] c = counterMatrix.Select("Attributes1 = '" + theItem.Attributes1 + "'");
                                if (c.Count() == 0)
                                {
                                    matrixcounter = 0;
                                    counterMatrix.Rows.Add(theItem.Attributes1, matrixcounter);
                                }
                                else
                                {
                                    matrixcounter = Int32.Parse(c[0]["Counter"].ToString()) + 1;
                                    c[0]["Counter"] = matrixcounter;
                                }

                                runningnumber += matrixcounter;
                                var itemno = theItem.Attributes1 + runningnumber.ToString("00#");
                                theItem.ItemNo = itemno;
                                if (string.IsNullOrEmpty(theItem.Barcode.Trim()))
                                    theItem.Barcode = itemno;
                                matrixCount++;
                            }
                            theItem.Attributes2 = theItem.ItemName;
                            theItem.ItemName = string.Format("{0}-{1}-{2}",
                                theItem.ItemName, theItem.Attributes3, theItem.Attributes4);
                            theItem.IsMatrixItem = true;
                            if (!string.IsNullOrEmpty(theItem.Attributes3)
                                && !createdAttribute3.Contains(theItem.Attributes3))
                            {
                                Query qr = new Query("ItemAttributes");
                                qr.AddWhere(ItemAttribute.Columns.Type, Comparison.Equals, "Attributes3");
                                qr.AddWhere(ItemAttribute.Columns.ValueX, theItem.Attributes3);
                                ItemAttribute iAtt = new ItemAttributeController().FetchByQuery(qr).FirstOrDefault();
                                if (iAtt == null)
                                    iAtt = new ItemAttribute();
                                iAtt.Type = "Attributes3";
                                iAtt.ValueX = theItem.Attributes3;
                                iAtt.Deleted = false;
                                createdAttribute3.Add(iAtt.ValueX);
                                if (iAtt.IsNew)
                                    qmc.Add(iAtt.GetInsertCommand(string.Format("{0} via WEB Item Importer", userName)));
                                else
                                    qmc.Add(iAtt.GetUpdateCommand(string.Format("{0} via WEB Item Importer", userName)));
                            }
                            if (!string.IsNullOrEmpty(theItem.Attributes4)
                                && !createdAttribute4.Contains(theItem.Attributes4))
                            {
                                Query qr = new Query("ItemAttributes");
                                qr.AddWhere(ItemAttribute.Columns.Type, Comparison.Equals, "Attributes4");
                                qr.AddWhere(ItemAttribute.Columns.ValueX, theItem.Attributes4);
                                ItemAttribute iAtt = new ItemAttributeController().FetchByQuery(qr).FirstOrDefault();
                                if (iAtt == null)
                                    iAtt = new ItemAttribute();
                                iAtt.Type = "Attributes4";
                                iAtt.ValueX = theItem.Attributes4;
                                iAtt.Deleted = false;
                                createdAttribute4.Add(iAtt.ValueX);
                                if (iAtt.IsNew)
                                    qmc.Add(iAtt.GetInsertCommand(string.Format("{0} via WEB Item Importer", userName)));
                                else
                                    qmc.Add(iAtt.GetUpdateCommand(string.Format("{0} via WEB Item Importer", userName)));
                            }
                        }

                        theItem.Userfloat6 = ((string)result.Rows[i]["Userfloat6"]).GetDecimalValue();
                        theItem.Userfloat7 = ((string)result.Rows[i]["Userfloat7"]).GetDecimalValue();
                        theItem.Userfloat8 = ((string)result.Rows[i]["Userfloat8"]).GetDecimalValue();
                        theItem.Userfloat9 = ((string)result.Rows[i]["Userfloat9"]).GetDecimalValue();
                        theItem.Userfloat10 = ((string)result.Rows[i]["Userfloat10"]).GetDecimalValue();

                        if (theItem.IsNew)
                            qmc.Add(theItem.GetInsertCommand(string.Format("{0} via WEB Item Importer", userName)));
                        else
                            qmc.Add(theItem.GetUpdateCommand(string.Format("{0} via WEB Item Importer", userName)));

                        string price1 = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P1DiscountName);
                        string price2 = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P2DiscountName);
                        string price3 = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P3DiscountName);
                        string price4 = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P4DiscountName);
                        string price5 = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P5DiscountName);
                        bool usePrice1 = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice1), false);
                        bool usePrice2 = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice2), false);
                        bool usePrice3 = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice3), false);
                        bool usePrice4 = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice4), false);
                        bool usePrice5 = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice5), false);

                        #region *) Price Level 1
                        if (usePrice1)
                        {
                            SpecialDiscount sp = new SpecialDiscount(SpecialDiscount.Columns.DiscountName, "P1");
                            if (sp.IsNew)
                            {
                                sp.DiscountName = "P1";
                                sp.SpecialDiscountID = Guid.NewGuid();
                                sp.DiscountPercentage = 0;
                                sp.ShowLabel = true;
                                sp.PriorityLevel = 1;
                                sp.Deleted = false;
                                sp.Enabled = true;
                                sp.ApplicableToAllItem = false;
                                sp.StartDate = new DateTime(2015, 01, 01);
                                sp.EndDate = sp.StartDate.GetValueOrDefault(DateTime.Now).AddYears(100);
                                sp.MinimumSpending = 0;
                                sp.IsBankPromo = false;
                                sp.DiscountLabel = price1;
                                if (!createdDiscount.Contains("P1"))
                                {
                                    qmc.Add(sp.GetInsertCommand(string.Format("{0} via WEB Item Importer", userName)));
                                    createdDiscount.Add(sp.DiscountName);
                                }
                            }
                            Query qr = new Query("SpecialDiscountDetail");
                            qr.AddWhere(SpecialDiscountDetail.Columns.DiscountName, Comparison.Equals, sp.DiscountName);
                            qr.AddWhere(SpecialDiscountDetail.Columns.ItemNo, Comparison.Equals, theItem.ItemNo);

                            SpecialDiscountDetail spd = new SpecialDiscountDetailController().FetchByQuery(qr).FirstOrDefault();
                            if (spd == null)
                            {
                                spd = new SpecialDiscountDetail();
                                spd.ItemNo = theItem.ItemNo;
                                spd.DiscountName = sp.DiscountName;
                            }
                            spd.Deleted = false;
                            spd.DiscountAmount = theItem.RetailPrice - theItem.Userfloat6;
                            if (spd.IsNew)
                                qmc.Add(spd.GetInsertCommand(string.Format("{0} via WEB Item Importer", userName)));
                            else
                                qmc.Add(spd.GetUpdateCommand(string.Format("{0} via WEB Item Importer", userName)));
                        }
                        #endregion

                        #region *) Price Level 2
                        if (usePrice2)
                        {
                            SpecialDiscount sp = new SpecialDiscount(SpecialDiscount.Columns.DiscountName, "P2");
                            if (sp.IsNew)
                            {
                                sp.DiscountName = "P2";
                                sp.SpecialDiscountID = Guid.NewGuid();
                                sp.DiscountPercentage = 0;
                                sp.ShowLabel = true;
                                sp.PriorityLevel = 2;
                                sp.Deleted = false;
                                sp.Enabled = true;
                                sp.ApplicableToAllItem = false;
                                sp.StartDate = new DateTime(2015, 01, 01);
                                sp.EndDate = sp.StartDate.GetValueOrDefault(DateTime.Now).AddYears(100);
                                sp.MinimumSpending = 0;
                                sp.IsBankPromo = false;
                                sp.DiscountLabel = price1;
                                if (!createdDiscount.Contains("P2"))
                                {
                                    qmc.Add(sp.GetInsertCommand(string.Format("{0} via WEB Item Importer", userName)));
                                    createdDiscount.Add(sp.DiscountName);
                                }
                            }
                            Query qr = new Query("SpecialDiscountDetail");
                            qr.AddWhere(SpecialDiscountDetail.Columns.DiscountName, Comparison.Equals, sp.DiscountName);
                            qr.AddWhere(SpecialDiscountDetail.Columns.ItemNo, Comparison.Equals, theItem.ItemNo);

                            SpecialDiscountDetail spd = new SpecialDiscountDetailController().FetchByQuery(qr).FirstOrDefault();
                            if (spd == null)
                            {
                                spd = new SpecialDiscountDetail();
                                spd.ItemNo = theItem.ItemNo;
                                spd.DiscountName = sp.DiscountName;
                            }
                            spd.Deleted = false;
                            spd.DiscountAmount = theItem.RetailPrice - theItem.Userfloat7;
                            if (spd.IsNew)
                                qmc.Add(spd.GetInsertCommand(string.Format("{0} via WEB Item Importer", userName)));
                            else
                                qmc.Add(spd.GetUpdateCommand(string.Format("{0} via WEB Item Importer", userName)));
                        }
                        #endregion

                        #region *) Price Level 3
                        if (usePrice3)
                        {
                            SpecialDiscount sp = new SpecialDiscount(SpecialDiscount.Columns.DiscountName, "P3");
                            if (sp.IsNew)
                            {
                                sp.DiscountName = "P3";
                                sp.SpecialDiscountID = Guid.NewGuid();
                                sp.DiscountPercentage = 0;
                                sp.ShowLabel = true;
                                sp.PriorityLevel = 3;
                                sp.Deleted = false;
                                sp.Enabled = true;
                                sp.ApplicableToAllItem = false;
                                sp.StartDate = new DateTime(2015, 01, 01);
                                sp.EndDate = sp.StartDate.GetValueOrDefault(DateTime.Now).AddYears(100);
                                sp.MinimumSpending = 0;
                                sp.IsBankPromo = false;
                                sp.DiscountLabel = price3;
                                if (!createdDiscount.Contains("P3"))
                                {
                                    qmc.Add(sp.GetInsertCommand(string.Format("{0} via WEB Item Importer", userName)));
                                    createdDiscount.Add(sp.DiscountName);
                                }
                            }
                            Query qr = new Query("SpecialDiscountDetail");
                            qr.AddWhere(SpecialDiscountDetail.Columns.DiscountName, Comparison.Equals, sp.DiscountName);
                            qr.AddWhere(SpecialDiscountDetail.Columns.ItemNo, Comparison.Equals, theItem.ItemNo);

                            SpecialDiscountDetail spd = new SpecialDiscountDetailController().FetchByQuery(qr).FirstOrDefault();
                            if (spd == null)
                            {
                                spd = new SpecialDiscountDetail();
                                spd.ItemNo = theItem.ItemNo;
                                spd.DiscountName = sp.DiscountName;
                            }
                            spd.Deleted = false;
                            spd.DiscountAmount = theItem.RetailPrice - theItem.Userfloat8;
                            if (spd.IsNew)
                                qmc.Add(spd.GetInsertCommand(string.Format("{0} via WEB Item Importer", userName)));
                            else
                                qmc.Add(spd.GetUpdateCommand(string.Format("{0} via WEB Item Importer", userName)));
                        }
                        #endregion

                        #region *) Price Level 4
                        if (usePrice4)
                        {
                            SpecialDiscount sp = new SpecialDiscount(SpecialDiscount.Columns.DiscountName, "P4");
                            if (sp.IsNew)
                            {
                                sp.DiscountName = "P4";
                                sp.SpecialDiscountID = Guid.NewGuid();
                                sp.DiscountPercentage = 0;
                                sp.ShowLabel = true;
                                sp.PriorityLevel = 4;
                                sp.Deleted = false;
                                sp.Enabled = true;
                                sp.ApplicableToAllItem = false;
                                sp.StartDate = new DateTime(2015, 01, 01);
                                sp.EndDate = sp.StartDate.GetValueOrDefault(DateTime.Now).AddYears(100);
                                sp.MinimumSpending = 0;
                                sp.IsBankPromo = false;
                                sp.DiscountLabel = price4;
                                if (!createdDiscount.Contains("P4"))
                                {
                                    qmc.Add(sp.GetInsertCommand(string.Format("{0} via WEB Item Importer", userName)));
                                    createdDiscount.Add(sp.DiscountName);
                                }
                            }
                            Query qr = new Query("SpecialDiscountDetail");
                            qr.AddWhere(SpecialDiscountDetail.Columns.DiscountName, Comparison.Equals, sp.DiscountName);
                            qr.AddWhere(SpecialDiscountDetail.Columns.ItemNo, Comparison.Equals, theItem.ItemNo);

                            SpecialDiscountDetail spd = new SpecialDiscountDetailController().FetchByQuery(qr).FirstOrDefault();
                            if (spd == null)
                            {
                                spd = new SpecialDiscountDetail();
                                spd.ItemNo = theItem.ItemNo;
                                spd.DiscountName = sp.DiscountName;
                            }
                            spd.Deleted = false;
                            spd.DiscountAmount = theItem.RetailPrice - theItem.Userfloat9;
                            if (spd.IsNew)
                                qmc.Add(spd.GetInsertCommand(string.Format("{0} via WEB Item Importer", userName)));
                            else
                                qmc.Add(spd.GetUpdateCommand(string.Format("{0} via WEB Item Importer", userName)));
                        }
                        #endregion

                        #region *) Price Level 5
                        if (usePrice5)
                        {
                            SpecialDiscount sp = new SpecialDiscount(SpecialDiscount.Columns.DiscountName, "P5");
                            if (sp.IsNew)
                            {
                                sp.DiscountName = "P5";
                                sp.SpecialDiscountID = Guid.NewGuid();
                                sp.DiscountPercentage = 0;
                                sp.ShowLabel = true;
                                sp.PriorityLevel = 10;
                                sp.Deleted = false;
                                sp.Enabled = true;
                                sp.ApplicableToAllItem = false;
                                sp.StartDate = new DateTime(2015, 01, 01);
                                sp.EndDate = sp.StartDate.GetValueOrDefault(DateTime.Now).AddYears(100);
                                sp.MinimumSpending = 0;
                                sp.IsBankPromo = false;
                                sp.DiscountLabel = price5;
                                if (!createdDiscount.Contains("P5"))
                                {
                                    qmc.Add(sp.GetInsertCommand(string.Format("{0} via WEB Item Importer", userName)));
                                    createdDiscount.Add(sp.DiscountName);
                                }
                            }
                            Query qr = new Query("SpecialDiscountDetail");
                            qr.AddWhere(SpecialDiscountDetail.Columns.DiscountName, Comparison.Equals, sp.DiscountName);
                            qr.AddWhere(SpecialDiscountDetail.Columns.ItemNo, Comparison.Equals, theItem.ItemNo);

                            SpecialDiscountDetail spd = new SpecialDiscountDetailController().FetchByQuery(qr).FirstOrDefault();
                            if (spd == null)
                            {
                                spd = new SpecialDiscountDetail();
                                spd.ItemNo = theItem.ItemNo;
                                spd.DiscountName = sp.DiscountName;
                            }
                            spd.Deleted = false;
                            spd.DiscountAmount = theItem.RetailPrice - theItem.Userfloat10;
                            if (spd.IsNew)
                                qmc.Add(spd.GetInsertCommand(string.Format("{0} via WEB Item Importer", userName)));
                            else
                                qmc.Add(spd.GetUpdateCommand(string.Format("{0} via WEB Item Importer", userName)));
                        }
                        #endregion

                        #endregion

                        if (displaySupplier)
                        {
                            if (!theItem.Deleted.GetValueOrDefault(false))
                            {
                                var query = new Query("ItemSupplierMap");
                                #region *) Item Supplier Map

                                #region *) Delete All Supplier that linked to the Item

                                //query = new Query("ItemSupplierMap");
                                //query.AddWhere(ItemSupplierMap.Columns.ItemNo, Comparison.Equals, theItem.ItemNo);
                                //var existingISM = new ItemSupplierMapController().FetchByQuery(query).ToList();
                                //foreach (var exISM in existingISM)
                                //{
                                //    exISM.Deleted = true;
                                //    qmc.Add(exISM.GetUpdateCommand(string.Format("{0} via WEB Item Importer", userName)));
                                //}

                                #endregion

                                if (!string.IsNullOrEmpty((string)result.Rows[i]["Supplier Name"]))
                                {
                                    Supplier sup = new Supplier(Supplier.Columns.SupplierName, (string)result.Rows[i]["Supplier Name"]);

                                    #region *) Delete All Item that linked to the supplier

                                    //query = new Query("ItemSupplierMap");
                                    //query.AddWhere(ItemSupplierMap.Columns.SupplierID, Comparison.Equals, sup.SupplierID);
                                    //var existingISM2 = new ItemSupplierMapController().FetchByQuery(query).ToList();
                                    //foreach (var exISM in existingISM2)
                                    //{
                                    //    exISM.Deleted = true;
                                    //    qmc.Add(exISM.GetUpdateCommand(string.Format("{0} via WEB Item Importer", userName)));
                                    //}

                                    #endregion

                                    query = new Query("ItemSupplierMap");
                                    query.AddWhere(ItemSupplierMap.Columns.SupplierID, Comparison.Equals, sup.SupplierID);
                                    query.AddWhere(ItemSupplierMap.Columns.ItemNo, Comparison.Equals, theItem.ItemNo);
                                    var ism = new ItemSupplierMapController().FetchByQuery(query).FirstOrDefault();
                                    if (ism == null)
                                        ism = new ItemSupplierMap();
                                    ism.SupplierID = sup.SupplierID;
                                    ism.ItemNo = theItem.ItemNo;
                                    ism.CostPrice = theItem.FactoryPrice;
                                    ism.Deleted = false;
                                    if (ism.IsNew)
                                        qmc.Add(ism.GetInsertCommand(string.Format("{0} via WEB Item Importer", userName)));
                                    else
                                        qmc.Add(ism.GetUpdateCommand(string.Format("{0} via WEB Item Importer", userName)));
                                }

                                #endregion
                            }
                        }
                    }
                }

                #endregion

                if (qmc.Count > 0)
                    DataService.ExecuteTransaction(qmc);
                isSuccess = true;
            }
            catch (Exception ex)
            {
                isSuccess = false;
                message = "ERROR : " + ex.Message;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }

        public static bool ImportDataWithOutletOption(string userName, string ApplicableTo, string OutletChosen, DataTable data, out DataTable result, out string message)
        {
            bool isSuccess = false;
            result = data.Copy();
            message = "";
            ItemController itemLogic = new ItemController();
            bool displaySupplier = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplaySupplier), true);
            try
            {
                result.Columns.Add("Status", typeof(string));
                Dictionary<string, string> itemDeptData = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                Dictionary<string, KeyValuePair<string, string>> categoryData = new Dictionary<string, KeyValuePair<string, string>>(StringComparer.OrdinalIgnoreCase);
                QueryCommandCollection qmc = new QueryCommandCollection();

                DataTable counterMatrix = new DataTable();
                counterMatrix.Columns.Add("Attributes1", typeof(string));
                counterMatrix.Columns.Add("Counter", typeof(Int32));

                #region *) Validation
                List<string> theItemNos = new List<string>();
                List<string> theBarcodes = new List<string>();
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    bool isValid = true;
                    string itemNo = (string)result.Rows[i]["Item No"];
                    string parentItemNo = (string)result.Rows[i]["Attributes1"];
                    string attributes3 = (string)result.Rows[i]["Attributes3"];
                    string attributes4 = (string)result.Rows[i]["Attributes4"];
                    string barcode = (string)result.Rows[i]["Barcode"];
                    string supplier = (string)result.Rows[i]["Supplier Name"];
                    string itemDeptID = (string)result.Rows[i]["Department Code"];
                    string itemDeptName = (string)result.Rows[i]["Department Name"];
                    string categoryID = (string)result.Rows[i]["Category Code"];
                    string categoryName = (string)result.Rows[i]["Category Name"];
                    string itemType = (string)result.Rows[i]["Item Type"];

                    decimal RetailPrice = result.Rows[i]["Retail Price"].ToString().GetDecimalValue();

                    string deductedItemType = "";
                    string deductedConvRate = "";
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayNonInventoryProduct), false))
                    {
                        deductedItemType = (string)result.Rows[i]["Deduction Item No"];
                        deductedConvRate = (string)result.Rows[i]["Deduction Qty"];
                    }
                    
                    bool isMatrixMode = ((string)result.Rows[i]["Is Matrix Item"] + "").GetBoolValue(false);
                    bool isUseFixedValueCOG = ((string)result.Rows[i]["Is Using Fixed Value for COG"] + "").GetBoolValue(false);
                    string FixedCOGType = ((string)result.Rows[i]["Fixed COG Type"]);
                    string FixCOGValue = ((string)result.Rows[i]["Fix COG Value"]);
                    string status = "";

                    if (string.IsNullOrEmpty(categoryName))
                    {
                        status = "- Category Cannot Empty\n";
                        isValid = false;
                    }
                    if (string.IsNullOrEmpty(itemType))
                    {
                        status = "- Item Type Cannot Empty\n";
                        isValid = false;
                    }
                    if (isMatrixMode)
                    {
                        if (string.IsNullOrEmpty(parentItemNo))
                        {
                            status = "- Parent Item No Cannot Empty if use Matrix Mode\n";
                            isValid = false;
                        }
                        else
                        { 
                            //remove this and item importer able to update matrix item
                            //if (itemLogic.CheckIfMatrixItemExist(parentItemNo, attributes3, attributes4))
                            //{
                            //    status = "- This Matrix Item already exist\n";
                            //    isValid = false;
                            //}

                            if (string.IsNullOrEmpty(attributes3) || string.IsNullOrEmpty(attributes4))
                            {
                                status = "- This Matrix Item must have Attributes3 and Attributes4\n";
                                isValid = false;
                            }
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(itemNo))
                        {
                            status = "- ItemNo Cannot Empty\n";
                            isValid = false;
                        }

                        if (theItemNos.Contains(itemNo.Trim()))
                        {
                            status = "- Duplicated ItemNo\n";
                            isValid = false;
                        }
                        if (!string.IsNullOrEmpty(barcode.Trim()) && theBarcodes.Contains(barcode.Trim()))
                        {
                            status = "- Duplicated Barcode\n";
                            isValid = false;
                        }

                        if (!string.IsNullOrEmpty(barcode.Trim()))
                        {
                            //var existingBarcode = new Item(Item.Columns.Barcode, barcode);
                            //if (!existingBarcode.IsNew && existingBarcode.ItemNo != itemNo)
                            //{
                            //    status = "- Another item with same barcode already exist\n";
                            //    isValid = false;
                            //}
                            string barcodeItemno = "";
                            if (itemLogic.CheckIfBarcodeExists(barcode, itemNo, out barcodeItemno))
                            {
                                status = "- Barcode already exist and used for item "+ barcodeItemno +" \n";
                                isValid = false;
                            }
                        }
                    }
                    if (displaySupplier)
                    {
                        if (!string.IsNullOrEmpty(supplier))
                        {
                            var supp = new Supplier(Supplier.Columns.SupplierName, supplier.Trim());
                            if (supp.IsNew)
                            {
                                status = "- Supplier not found";
                                isValid = false;
                            }
                        }
                    }
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayNonInventoryProduct), false))
                    {
                        if (itemType == "Non Inventory Product" && (string.IsNullOrEmpty(deductedItemType) || string.IsNullOrEmpty(deductedConvRate)))
                        {
                            status = "- Deducted Item and Deducted Conversion Rate are empty";
                            isValid = false;
                        }
                    }
                    if (isUseFixedValueCOG)
                    {
                        if (string.IsNullOrEmpty(FixedCOGType) || string.IsNullOrEmpty(FixCOGValue))
                        {
                            status = "- Fix COG Type and Fix COG Value can not be empty";
                            isValid = false;
                        }
                        else {
                            decimal fixCOGval = (FixCOGValue + "").GetDecimalValue();
                            if (fixCOGval <= 0)
                            {
                                status = "- Please input Fix COG value with correct number";
                                isValid = false;
                            }
                            else {
                                if (FixedCOGType == ItemController.FIXEDCOG_PERCENTAGE && fixCOGval > 100) 
                                {
                                    status = "- Fix COG Value can not be more than 100";
                                    isValid = false;
                                }
                            }
                        }
                    }

                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayMinimumSellingPrice), false))
                    {
                        decimal MinimumPrice = result.Rows[i]["Minimum Price"].ToString().GetDecimalValue();

                        if (RetailPrice >= 0 && (MinimumPrice < 0 || MinimumPrice > RetailPrice))
                        {
                            status = "- Minimum Price can not be negative or higher than Retail Price";
                            isValid = false;
                        }
                        else if (RetailPrice < 0 && MinimumPrice != 0)
                        {
                            status = "- Minimum Price must be equal 0";
                            isValid = false;
                        }
                        
                    }

                    result.Rows[i]["Status"] = status;
                    theItemNos.Add(itemNo.Trim());
                    if (!string.IsNullOrEmpty(barcode.Trim()))
                        theBarcodes.Add(barcode.Trim());

                    if (isValid)
                    {
                        if (!string.IsNullOrEmpty(itemDeptID) && !itemDeptData.ContainsKey(itemDeptID.Trim()))
                            itemDeptData.Add(itemDeptID.Trim(), itemDeptName);
                        if (!string.IsNullOrEmpty(categoryName) && !categoryData.ContainsKey(categoryName.Trim()))
                            categoryData.Add(categoryName.Trim(), new KeyValuePair<string, string>(itemDeptID, categoryID));
                    }
                }

                #endregion

                #region *) ItemDepartment

                foreach (var itemDept in itemDeptData)
                {
                    ItemDepartment id = new ItemDepartment(ItemDepartment.Columns.ItemDepartmentID, itemDept.Key);
                    if (id.IsNew)
                        id.ItemDepartmentID = itemDept.Key;
                    id.DepartmentName = itemDept.Value;
                    id.Deleted = false;
                    if (id.IsNew)
                        qmc.Add(id.GetInsertCommand(string.Format("{0} via WEB Item Importer", userName)));
                    else
                        qmc.Add(id.GetUpdateCommand(string.Format("{0} via WEB Item Importer", userName)));
                }

                #endregion

                #region *) Category

                foreach (var ctgItem in categoryData)
                {
                    Category ctg = new Category(Category.Columns.CategoryName, ctgItem.Key);
                    if (ctg.IsNew)
                        ctg.CategoryName = ctgItem.Key;
                    ctg.ItemDepartmentId = ctgItem.Value.Key;
                    ctg.CategoryId = ctgItem.Value.Value;
                    ctg.Deleted = false;
                    if (ctg.IsNew)
                        qmc.Add(ctg.GetInsertCommand(string.Format("{0} via WEB Item Importer", userName)));
                    else
                        qmc.Add(ctg.GetUpdateCommand(string.Format("{0} via WEB Item Importer", userName)));
                }

                #endregion

                #region *) Item & Supplier Map
                int matrixCount = 0;
                List<string> createdDiscount = new List<string>();
                List<string> createdAttribute3 = new List<string>();
                List<string> createdAttribute4 = new List<string>();
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    string err = (string)result.Rows[i]["Status"];
                    bool isMatrixMode = ((string)result.Rows[i]["Is Matrix Item"] + "").GetBoolValue(false);
                    string itemNo = ((string)result.Rows[i]["Item No"]).Trim();
                    string attributes1 = ((string)result.Rows[i]["Attributes1"] + "").Trim();
                    string attributes3 = ((string)result.Rows[i]["Attributes3"] + "").Trim();
                    string attributes4 = ((string)result.Rows[i]["Attributes4"] + "").Trim();
                    if (string.IsNullOrEmpty(err))
                    {
                        #region *) Item
                        bool canUpdate = true;
                        Item originalItem = new Item();

                        Item theItem  = new Item();
                        if (isMatrixMode)
                        {
                            string que = "select top 1 * from Item where Attributes1 = '{0}' and Attributes3 = '{1}' and Attributes4 = '{2}'";
                            que = string.Format(que, attributes1, attributes3, attributes4);

                            DataTable dt = DataService.GetDataSet(new QueryCommand(que)).Tables[0];

                            ItemCollection itCol = new ItemCollection();
                            itCol.Load(dt);

                            if (itCol.Count() > 0)
                            {
                                theItem = itCol[0];

                                //update to remove the redundant
                                string update = @"update item set deleted = 1, modifiedon = getdate() where Attributes1 = '{0}' and Attributes3 = '{1}' and Attributes4 = '{2}' and ItemNo != '{3}'";
                                update = string.Format(update, attributes1, attributes3, attributes4, theItem.ItemNo);

                                qmc.Add(new QueryCommand(update));                                
                            }
                            else
                            {
                                theItem = new Item(Item.Columns.ItemNo, (string)result.Rows[i]["Item No"]);
                                if (theItem.IsNew)
                                {
                                    theItem.UniqueID = Guid.NewGuid();
                                    theItem.ItemNo = (string)result.Rows[i]["Item No"];
                                }
                                else
                                {
                                    theItem.CopyTo(originalItem);
                                    originalItem.IsNew = false;
                                    if (ApplicableTo != "Product Master")
                                        canUpdate = false;
                                }
                            }
                        }
                        else
                        {
                            theItem = new Item(Item.Columns.ItemNo, (string)result.Rows[i]["Item No"]);
                            if (theItem.IsNew)
                            {
                                theItem.UniqueID = Guid.NewGuid();
                                theItem.ItemNo = (string)result.Rows[i]["Item No"];
                            }
                            else
                            {
                                theItem.CopyTo(originalItem);
                                originalItem.IsNew = false;
                                if (ApplicableTo != "Product Master")
                                    canUpdate = false;
                            }
                        }
                        theItem.CategoryName = (string)result.Rows[i]["Category Name"];
                        theItem.ItemName = (string)result.Rows[i]["Item Name"];
                        
                        string barcode = (string)result.Rows[i]["Barcode"];
                        if(!string.IsNullOrEmpty(barcode) && !itemLogic.IsAlternateBarcode(barcode, theItem.ItemNo))
                            theItem.Barcode = (string)result.Rows[i]["Barcode"];

                        //if choose outlet / outlet group, update if only item is new
                        if (ApplicableTo == "Outlet" || ApplicableTo == "Outlet Group")
                        {
                            if (theItem.IsNew)
                            {
                                theItem.RetailPrice = ((string)result.Rows[i]["Retail Price"]).GetDecimalValue();
                                theItem.FactoryPrice = ((string)result.Rows[i]["Cost Price"]).GetDecimalValue();
                            }
                        }
                        else
                        {
                            theItem.RetailPrice = ((string)result.Rows[i]["Retail Price"]).GetDecimalValue();
                            theItem.FactoryPrice = ((string)result.Rows[i]["Cost Price"]).GetDecimalValue();
                        }

                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayMinimumSellingPrice), false))
                        {
                            theItem.MinimumPrice = ((string)result.Rows[i]["Minimum Price"]).GetDecimalValue();
                        }

                        theItem.Userfld1 = (string)result.Rows[i]["UOM"];
                        theItem.IsNonDiscountable = ((string)result.Rows[i]["Is Non Discountable"]).ToUpper().Equals("YES");
                        theItem.Userfld9 = ((string)result.Rows[i]["Point Redeemable"]).ToUpper().Equals("YES") ? "D" : "N";
                        theItem.IsCommission = ((string)result.Rows[i]["Give Commission"]).ToUpper().Equals("YES");
                        theItem.Userflag2 = ((string)result.Rows[i]["Allow Pre Order"]).ToUpper().Equals("YES");

                        string gstRule = ((string)result.Rows[i]["GST Rule"] + "").ToLower();
                        if (gstRule == "exclusive")
                            theItem.GSTRule = 1;
                        else if (gstRule == "inclusive")
                            theItem.GSTRule = 2;
                        else
                            theItem.GSTRule = 3;

                        string itemType = ((string)result.Rows[i]["Item Type"] + "").ToLower();
                        if (itemType == "product")
                        {
                            theItem.IsServiceItem = false;
                            theItem.IsInInventory = true;
                            theItem.Userfld10 = "N";
                        }
                        else if (itemType == "open price product")
                        {
                            theItem.IsServiceItem = true;
                            theItem.IsInInventory = true;
                            theItem.Userfld10 = "N";
                        }
                        else if (itemType == "service")
                        {
                            theItem.IsServiceItem = true;
                            theItem.IsInInventory = false;
                            theItem.Userfld10 = "N";
                        }
                        else if (itemType == "point package")
                        {
                            theItem.IsServiceItem = false;
                            theItem.IsInInventory = false;
                            theItem.Userfld10 = "D";
                            theItem.Userfloat1 = ((string)result.Rows[i]["Point Get"]).GetDecimalValue();
                        }
                        else if (itemType == "course package")
                        {
                            theItem.IsServiceItem = false;
                            theItem.IsInInventory = false;
                            theItem.Userfld10 = "T";
                            theItem.Userfloat1 = ((string)result.Rows[i]["Point Get"]).GetDecimalValue();
                            theItem.Userfloat3 = ((string)result.Rows[i]["Breakdown Price"]).GetDecimalValue();
                        }else if(itemType == "non inventory product" && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayNonInventoryProduct), false))
                        {
                            theItem.IsInInventory = false;
                            theItem.IsServiceItem = false;
                            theItem.NonInventoryProduct = true;
                            theItem.PointGetAmount = 0;
                            theItem.PointGetMode = Item.PointMode.None;
                            theItem.PointRedeemAmount = 0;
                            theItem.Userfloat3 = null; /// Course Breakdown Price
                            theItem.DeductedItem = (string)result.Rows[i]["Deduction Item No"];
                            theItem.DeductConvRate = ((string)result.Rows[i]["Deduction Qty"] + "").GetDecimalValue();
                            theItem.DeductConvType = ((string)result.Rows[i]["Deduction Type"] + "").ToLower() == "down";
                        }
                        theItem.ItemDesc = (string)result.Rows[i]["Description"];
                        theItem.Attributes1 = (string)result.Rows[i]["Attributes1"];
                        theItem.Attributes2 = (string)result.Rows[i]["Attributes2"];
                        theItem.Attributes3 = (string)result.Rows[i]["Attributes3"];
                        theItem.Attributes4 = (string)result.Rows[i]["Attributes4"];
                        theItem.Attributes5 = (string)result.Rows[i]["Attributes5"];
                        theItem.Attributes6 = (string)result.Rows[i]["Attributes6"];
                        theItem.Attributes7 = (string)result.Rows[i]["Attributes7"];
                        theItem.Attributes8 = (string)result.Rows[i]["Attributes8"];
                        theItem.Remark = (string)result.Rows[i]["Remark"];

                        theItem.IsUsingFixedCOG = ((string)result.Rows[i]["Is Using Fixed Value for COG"] + "").GetBoolValue(false);
                        if (theItem.IsUsingFixedCOG)
                        {
                            theItem.FixedCOGType = (string)result.Rows[i]["Fixed COG Type"];
                            theItem.FixedCOGValue = ((string)result.Rows[i]["Fix COG Value"] + "").GetDecimalValue();
                        }
                        else
                        {
                            theItem.FixedCOGType = "";
                            theItem.FixedCOGValue = 0;
                        }

                        if (theItem.IsNew)
                            theItem.Deleted = false;

                        if (ApplicableTo == "Product Master")
                        {
                            theItem.Deleted = ((string)result.Rows[i]["Deleted"]).ToUpper().Equals("YES");
                        }
                        else {
                            if (theItem.IsNew)
                                theItem.Deleted = true;
                        }

                        if (isMatrixMode)
                        {
                            if (theItem.IsNew)
                            {
                                int runningnumber = ItemController.getNewRunningNumberMatrix(theItem.Attributes1);

                                var matrixcounter = 0;
                                DataRow[] c = counterMatrix.Select("Attributes1 = '" + theItem.Attributes1 + "'");
                                if (c.Count() == 0)
                                {
                                    matrixcounter = 0;
                                    counterMatrix.Rows.Add(theItem.Attributes1, matrixcounter);
                                }
                                else
                                {
                                    matrixcounter = Int32.Parse(c[0]["Counter"].ToString()) + 1;
                                    c[0]["Counter"] = matrixcounter;
                                }

                                runningnumber += matrixcounter;
                                var itemno = theItem.Attributes1 + runningnumber.ToString("00#");
                                theItem.ItemNo = itemno;
                                if (string.IsNullOrEmpty(theItem.Barcode.Trim()))
                                    theItem.Barcode = itemno;
                                matrixCount++;
                            }
                            theItem.Attributes2 = theItem.ItemName;
                            theItem.ItemName = string.Format("{0}-{1}-{2}",
                                theItem.ItemName, theItem.Attributes3, theItem.Attributes4);
                            theItem.IsMatrixItem = true;
                            if (!string.IsNullOrEmpty(theItem.Attributes3)
                                && !createdAttribute3.Contains(theItem.Attributes3))
                            {
                                Query qr = new Query("ItemAttributes");
                                qr.AddWhere(ItemAttribute.Columns.Type, Comparison.Equals, "Attributes3");
                                qr.AddWhere(ItemAttribute.Columns.ValueX, theItem.Attributes3);
                                ItemAttribute iAtt = new ItemAttributeController().FetchByQuery(qr).FirstOrDefault();
                                if (iAtt == null)
                                    iAtt = new ItemAttribute();
                                iAtt.Type = "Attributes3";
                                iAtt.ValueX = theItem.Attributes3;
                                iAtt.Deleted = false;
                                createdAttribute3.Add(iAtt.ValueX);
                                if (iAtt.IsNew)
                                    qmc.Add(iAtt.GetInsertCommand(string.Format("{0} via WEB Item Importer", userName)));
                                else
                                    qmc.Add(iAtt.GetUpdateCommand(string.Format("{0} via WEB Item Importer", userName)));
                            }
                            if (!string.IsNullOrEmpty(theItem.Attributes4)
                                && !createdAttribute4.Contains(theItem.Attributes4))
                            {
                                Query qr = new Query("ItemAttributes");
                                qr.AddWhere(ItemAttribute.Columns.Type, Comparison.Equals, "Attributes4");
                                qr.AddWhere(ItemAttribute.Columns.ValueX, theItem.Attributes4);
                                ItemAttribute iAtt = new ItemAttributeController().FetchByQuery(qr).FirstOrDefault();
                                if (iAtt == null)
                                    iAtt = new ItemAttribute();
                                iAtt.Type = "Attributes4";
                                iAtt.ValueX = theItem.Attributes4;
                                iAtt.Deleted = false;
                                createdAttribute4.Add(iAtt.ValueX);
                                if (iAtt.IsNew)
                                    qmc.Add(iAtt.GetInsertCommand(string.Format("{0} via WEB Item Importer", userName)));
                                else
                                    qmc.Add(iAtt.GetUpdateCommand(string.Format("{0} via WEB Item Importer", userName)));
                            }
                        }
                        if (ApplicableTo == "Product Master")
                        {
                            if (result.Columns.Contains("Userfloat6"))
                            {
                                theItem.Userfloat6 = ((string)result.Rows[i]["Userfloat6"]).GetDecimalValue();
                            }
                            if (result.Columns.Contains("Userfloat7"))
                            {
                                theItem.Userfloat7 = ((string)result.Rows[i]["Userfloat7"]).GetDecimalValue();
                            }
                            if (result.Columns.Contains("Userfloat8"))
                            {
                                theItem.Userfloat8 = ((string)result.Rows[i]["Userfloat8"]).GetDecimalValue();
                            }
                            if (result.Columns.Contains("Userfloat9"))
                            {
                                theItem.Userfloat9 = ((string)result.Rows[i]["Userfloat9"]).GetDecimalValue();
                            }
                            if (result.Columns.Contains("Userfloat10"))
                            {
                                theItem.Userfloat10 = ((string)result.Rows[i]["Userfloat10"]).GetDecimalValue();
                            }
                        }

                        if (theItem.IsNew)
                            qmc.Add(theItem.GetInsertCommand(string.Format("{0} via WEB Item Importer", userName)));
                        else
                        {
                            if(canUpdate)
                                qmc.Add(theItem.GetUpdateCommand(string.Format("{0} via WEB Item Importer", userName)));
                        }

                        #region *) Audit Log
                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.AuditLog.ProductSetup), false))
                        {
                            string operation = originalItem.IsNew ? "INSERT" : "UPDATE";
                            if (theItem.RetailPrice != originalItem.RetailPrice)
                                AuditLogController.AddLog(operation, "Item", "ItemNo", theItem.ItemNo, "RetailPrice = " + originalItem.RetailPrice.ToString("N2"), "RetailPrice = " + theItem.RetailPrice.ToString("N2"), userName);
                            if (theItem.Deleted.GetValueOrDefault(false) == true && originalItem.Deleted.GetValueOrDefault(false) == false)
                                AuditLogController.AddLog("DELETE", "Item", "ItemNo", theItem.ItemNo, "Delete = false", "Delete = true", userName);
                        }
                        #endregion

                        string price1 = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P1DiscountName);
                        string price2 = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P2DiscountName);
                        string price3 = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P3DiscountName);
                        string price4 = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P4DiscountName);
                        string price5 = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P5DiscountName);
                        bool usePrice1 = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice1), false);
                        bool usePrice2 = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice2), false);
                        bool usePrice3 = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice3), false);
                        bool usePrice4 = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice4), false);
                        bool usePrice5 = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice5), false);

                        #region *) Price Level 1
                        if (usePrice1)
                        {
                            SpecialDiscount sp = new SpecialDiscount(SpecialDiscount.Columns.DiscountName, "P1");
                            if (sp.IsNew)
                            {
                                sp.DiscountName = "P1";
                                sp.SpecialDiscountID = Guid.NewGuid();
                                sp.DiscountPercentage = 0;
                                sp.ShowLabel = true;
                                sp.PriorityLevel = 1;
                                sp.Deleted = false;
                                sp.Enabled = true;
                                sp.ApplicableToAllItem = false;
                                sp.StartDate = new DateTime(2015, 01, 01);
                                sp.EndDate = sp.StartDate.GetValueOrDefault(DateTime.Now).AddYears(100);
                                sp.MinimumSpending = 0;
                                sp.IsBankPromo = false;
                                sp.DiscountLabel = price1;
                                if (!createdDiscount.Contains("P1"))
                                {
                                    qmc.Add(sp.GetInsertCommand(string.Format("{0} via WEB Item Importer", userName)));
                                    createdDiscount.Add(sp.DiscountName);
                                }
                            }
                            /*
                            Query qr = new Query("SpecialDiscountDetail");
                            qr.AddWhere(SpecialDiscountDetail.Columns.DiscountName, Comparison.Equals, sp.DiscountName);
                            qr.AddWhere(SpecialDiscountDetail.Columns.ItemNo, Comparison.Equals, theItem.ItemNo);

                            SpecialDiscountDetail spd = new SpecialDiscountDetailController().FetchByQuery(qr).FirstOrDefault();
                            if (spd == null)
                            {
                                spd = new SpecialDiscountDetail();
                                spd.ItemNo = theItem.ItemNo;
                                spd.DiscountName = sp.DiscountName;
                            }
                            spd.Deleted = false;
                            spd.DiscountAmount = theItem.RetailPrice - theItem.Userfloat6;*/
                            //if (spd.IsNew)
                            //    qmc.Add(spd.GetInsertCommand(string.Format("{0} via WEB Item Importer", userName)));
                            //else
                            //    qmc.Add(spd.GetUpdateCommand(string.Format("{0} via WEB Item Importer", userName)));

                        }
                        #endregion

                        #region *) Price Level 2
                        if (usePrice2)
                        {
                            SpecialDiscount sp = new SpecialDiscount(SpecialDiscount.Columns.DiscountName, "P2");
                            if (sp.IsNew)
                            {
                                sp.DiscountName = "P2";
                                sp.SpecialDiscountID = Guid.NewGuid();
                                sp.DiscountPercentage = 0;
                                sp.ShowLabel = true;
                                sp.PriorityLevel = 2;
                                sp.Deleted = false;
                                sp.Enabled = true;
                                sp.ApplicableToAllItem = false;
                                sp.StartDate = new DateTime(2015, 01, 01);
                                sp.EndDate = sp.StartDate.GetValueOrDefault(DateTime.Now).AddYears(100);
                                sp.MinimumSpending = 0;
                                sp.IsBankPromo = false;
                                sp.DiscountLabel = price1;
                                if (!createdDiscount.Contains("P2"))
                                {
                                    qmc.Add(sp.GetInsertCommand(string.Format("{0} via WEB Item Importer", userName)));
                                    createdDiscount.Add(sp.DiscountName);
                                }
                            }
                            //Query qr = new Query("SpecialDiscountDetail");
                            //qr.AddWhere(SpecialDiscountDetail.Columns.DiscountName, Comparison.Equals, sp.DiscountName);
                            //qr.AddWhere(SpecialDiscountDetail.Columns.ItemNo, Comparison.Equals, theItem.ItemNo);

                            //SpecialDiscountDetail spd = new SpecialDiscountDetailController().FetchByQuery(qr).FirstOrDefault();
                            //if (spd == null)
                            //{
                            //    spd = new SpecialDiscountDetail();
                            //    spd.ItemNo = theItem.ItemNo;
                            //    spd.DiscountName = sp.DiscountName;
                            //}
                            //spd.Deleted = false;
                            //spd.DiscountAmount = theItem.RetailPrice - theItem.Userfloat7;
                            //if (spd.IsNew)
                            //    qmc.Add(spd.GetInsertCommand(string.Format("{0} via WEB Item Importer", userName)));
                            //else
                            //    qmc.Add(spd.GetUpdateCommand(string.Format("{0} via WEB Item Importer", userName)));
                        }
                        #endregion

                        #region *) Price Level 3
                        if (usePrice3)
                        {
                            SpecialDiscount sp = new SpecialDiscount(SpecialDiscount.Columns.DiscountName, "P3");
                            if (sp.IsNew)
                            {
                                sp.DiscountName = "P3";
                                sp.SpecialDiscountID = Guid.NewGuid();
                                sp.DiscountPercentage = 0;
                                sp.ShowLabel = true;
                                sp.PriorityLevel = 3;
                                sp.Deleted = false;
                                sp.Enabled = true;
                                sp.ApplicableToAllItem = false;
                                sp.StartDate = new DateTime(2015, 01, 01);
                                sp.EndDate = sp.StartDate.GetValueOrDefault(DateTime.Now).AddYears(100);
                                sp.MinimumSpending = 0;
                                sp.IsBankPromo = false;
                                sp.DiscountLabel = price3;
                                if (!createdDiscount.Contains("P3"))
                                {
                                    qmc.Add(sp.GetInsertCommand(string.Format("{0} via WEB Item Importer", userName)));
                                    createdDiscount.Add(sp.DiscountName);
                                }
                            }
                            //Query qr = new Query("SpecialDiscountDetail");
                            //qr.AddWhere(SpecialDiscountDetail.Columns.DiscountName, Comparison.Equals, sp.DiscountName);
                            //qr.AddWhere(SpecialDiscountDetail.Columns.ItemNo, Comparison.Equals, theItem.ItemNo);

                            //SpecialDiscountDetail spd = new SpecialDiscountDetailController().FetchByQuery(qr).FirstOrDefault();
                            //if (spd == null)
                            //{
                            //    spd = new SpecialDiscountDetail();
                            //    spd.ItemNo = theItem.ItemNo;
                            //    spd.DiscountName = sp.DiscountName;
                            //}
                            //spd.Deleted = false;
                            //spd.DiscountAmount = theItem.RetailPrice - theItem.Userfloat8;
                            //if (spd.IsNew)
                            //    qmc.Add(spd.GetInsertCommand(string.Format("{0} via WEB Item Importer", userName)));
                            //else
                            //    qmc.Add(spd.GetUpdateCommand(string.Format("{0} via WEB Item Importer", userName)));
                        }
                        #endregion

                        #region *) Price Level 4
                        if (usePrice4)
                        {
                            SpecialDiscount sp = new SpecialDiscount(SpecialDiscount.Columns.DiscountName, "P4");
                            if (sp.IsNew)
                            {
                                sp.DiscountName = "P4";
                                sp.SpecialDiscountID = Guid.NewGuid();
                                sp.DiscountPercentage = 0;
                                sp.ShowLabel = true;
                                sp.PriorityLevel = 4;
                                sp.Deleted = false;
                                sp.Enabled = true;
                                sp.ApplicableToAllItem = false;
                                sp.StartDate = new DateTime(2015, 01, 01);
                                sp.EndDate = sp.StartDate.GetValueOrDefault(DateTime.Now).AddYears(100);
                                sp.MinimumSpending = 0;
                                sp.IsBankPromo = false;
                                sp.DiscountLabel = price4;
                                if (!createdDiscount.Contains("P4"))
                                {
                                    qmc.Add(sp.GetInsertCommand(string.Format("{0} via WEB Item Importer", userName)));
                                    createdDiscount.Add(sp.DiscountName);
                                }
                            }
                            //Query qr = new Query("SpecialDiscountDetail");
                            //qr.AddWhere(SpecialDiscountDetail.Columns.DiscountName, Comparison.Equals, sp.DiscountName);
                            //qr.AddWhere(SpecialDiscountDetail.Columns.ItemNo, Comparison.Equals, theItem.ItemNo);

                            //SpecialDiscountDetail spd = new SpecialDiscountDetailController().FetchByQuery(qr).FirstOrDefault();
                            //if (spd == null)
                            //{
                            //    spd = new SpecialDiscountDetail();
                            //    spd.ItemNo = theItem.ItemNo;
                            //    spd.DiscountName = sp.DiscountName;
                            //}
                            //spd.Deleted = false;
                            //spd.DiscountAmount = theItem.RetailPrice - theItem.Userfloat9;
                            //if (spd.IsNew)
                            //    qmc.Add(spd.GetInsertCommand(string.Format("{0} via WEB Item Importer", userName)));
                            //else
                            //    qmc.Add(spd.GetUpdateCommand(string.Format("{0} via WEB Item Importer", userName)));
                        }
                        #endregion

                        #region *) Price Level 5
                        if (usePrice5)
                        {
                            SpecialDiscount sp = new SpecialDiscount(SpecialDiscount.Columns.DiscountName, "P5");
                            if (sp.IsNew)
                            {
                                sp.DiscountName = "P5";
                                sp.SpecialDiscountID = Guid.NewGuid();
                                sp.DiscountPercentage = 0;
                                sp.ShowLabel = true;
                                sp.PriorityLevel = 10;
                                sp.Deleted = false;
                                sp.Enabled = true;
                                sp.ApplicableToAllItem = false;
                                sp.StartDate = new DateTime(2015, 01, 01);
                                sp.EndDate = sp.StartDate.GetValueOrDefault(DateTime.Now).AddYears(100);
                                sp.MinimumSpending = 0;
                                sp.IsBankPromo = false;
                                sp.DiscountLabel = price5;
                                if (!createdDiscount.Contains("P5"))
                                {
                                    qmc.Add(sp.GetInsertCommand(string.Format("{0} via WEB Item Importer", userName)));
                                    createdDiscount.Add(sp.DiscountName);
                                }
                            }
                            //Query qr = new Query("SpecialDiscountDetail");
                            //qr.AddWhere(SpecialDiscountDetail.Columns.DiscountName, Comparison.Equals, sp.DiscountName);
                            //qr.AddWhere(SpecialDiscountDetail.Columns.ItemNo, Comparison.Equals, theItem.ItemNo);

                            //SpecialDiscountDetail spd = new SpecialDiscountDetailController().FetchByQuery(qr).FirstOrDefault();
                            //if (spd == null)
                            //{
                            //    spd = new SpecialDiscountDetail();
                            //    spd.ItemNo = theItem.ItemNo;
                            //    spd.DiscountName = sp.DiscountName;
                            //}
                            //spd.Deleted = false;
                            //spd.DiscountAmount = theItem.RetailPrice - theItem.Userfloat10;
                            //if (spd.IsNew)
                            //    qmc.Add(spd.GetInsertCommand(string.Format("{0} via WEB Item Importer", userName)));
                            //else
                            //    qmc.Add(spd.GetUpdateCommand(string.Format("{0} via WEB Item Importer", userName)));
                        }
                        #endregion

                        #endregion

                        if (displaySupplier)
                        {
                            if (!theItem.Deleted.GetValueOrDefault(false) && canUpdate)
                            {
                                var query = new Query("ItemSupplierMap");
                                #region *) Item Supplier Map

                                #region *) Delete All Supplier that linked to the Item

                                //query = new Query("ItemSupplierMap");
                                //query.AddWhere(ItemSupplierMap.Columns.ItemNo, Comparison.Equals, theItem.ItemNo);
                                //var existingISM = new ItemSupplierMapController().FetchByQuery(query).ToList();
                                //foreach (var exISM in existingISM)
                                //{
                                //    exISM.Deleted = true;
                                //    qmc.Add(exISM.GetUpdateCommand(string.Format("{0} via WEB Item Importer", userName)));
                                //}

                                #endregion

                                if (!string.IsNullOrEmpty((string)result.Rows[i]["Supplier Name"]))
                                {
                                    Supplier sup = new Supplier(Supplier.Columns.SupplierName, (string)result.Rows[i]["Supplier Name"]);

                                    #region *) Delete All Item that linked to the supplier

                                    //query = new Query("ItemSupplierMap");
                                    //query.AddWhere(ItemSupplierMap.Columns.SupplierID, Comparison.Equals, sup.SupplierID);
                                    //var existingISM2 = new ItemSupplierMapController().FetchByQuery(query).ToList();
                                    //foreach (var exISM in existingISM2)
                                    //{
                                    //    exISM.Deleted = true;
                                    //    qmc.Add(exISM.GetUpdateCommand(string.Format("{0} via WEB Item Importer", userName)));
                                    //}

                                    #endregion

                                    query = new Query("ItemSupplierMap");
                                    query.AddWhere(ItemSupplierMap.Columns.SupplierID, Comparison.Equals, sup.SupplierID);
                                    query.AddWhere(ItemSupplierMap.Columns.ItemNo, Comparison.Equals, theItem.ItemNo);
                                    var ism = new ItemSupplierMapController().FetchByQuery(query).FirstOrDefault();
                                    if (ism == null)
                                        ism = new ItemSupplierMap();
                                    ism.SupplierID = sup.SupplierID;
                                    ism.ItemNo = theItem.ItemNo;
                                    ism.CostPrice = theItem.FactoryPrice;
                                    ism.Deleted = false;
                                    if (ism.IsNew)
                                        qmc.Add(ism.GetInsertCommand(string.Format("{0} via WEB Item Importer", userName)));
                                    else
                                        qmc.Add(ism.GetUpdateCommand(string.Format("{0} via WEB Item Importer", userName)));
                                }

                                #endregion
                            }
                        }

                        if ((ApplicableTo == "Outlet" || ApplicableTo == "Outlet Group") && !string.IsNullOrEmpty(OutletChosen))
                        {
                            if (ApplicableTo == "Outlet")
                            {
                                Query query = new Query("OutletGroupItemMap");
                                query.AddWhere(OutletGroupItemMap.Columns.OutletName, OutletChosen);
                                query.AddWhere(OutletGroupItemMap.Columns.ItemNo, theItem.ItemNo);
                                var ogm = new OutletGroupItemMapController().FetchByQuery(query).FirstOrDefault();

                                if (!theItem.Deleted.GetValueOrDefault(true))
                                {
                                    //Item is not deleted 
                                    if (ogm == null)
                                    {
                                        if (((string)result.Rows[i]["Deleted"]).ToUpper().Equals("YES"))
                                        {

                                            ogm = new OutletGroupItemMap();
                                            ogm.ItemNo = theItem.ItemNo;
                                            ogm.OutletName = OutletChosen;
                                            ogm.RetailPrice = ((string)result.Rows[i]["Retail Price"]).GetDecimalValue();
                                            ogm.CostPrice = ((string)result.Rows[i]["Cost Price"]).GetDecimalValue();
                                            ogm.Deleted = true;
                                            if (result.Columns.Contains("Userfloat6") && usePrice1)
                                            {
                                                ogm.P1 = ((string)result.Rows[i]["Userfloat6"]).GetDecimalValue();
                                            }
                                            if (result.Columns.Contains("Userfloat7") && usePrice2)
                                            {
                                                ogm.P2 = ((string)result.Rows[i]["Userfloat7"]).GetDecimalValue();
                                            }
                                            if (result.Columns.Contains("Userfloat8") && usePrice3)
                                            {
                                                ogm.P3 = ((string)result.Rows[i]["Userfloat8"]).GetDecimalValue();
                                            }
                                            if (result.Columns.Contains("Userfloat9") && usePrice4)
                                            {
                                                ogm.P4 = ((string)result.Rows[i]["Userfloat9"]).GetDecimalValue();
                                            }
                                            if (result.Columns.Contains("Userfloat10") && usePrice5)
                                            {
                                                ogm.P5 = ((string)result.Rows[i]["Userfloat10"]).GetDecimalValue();
                                            }
                                            qmc.Add(ogm.GetInsertCommand(string.Format("{0} via WEB Item Importer", userName)));

                                        }
                                        else
                                        {
                                            // If Item Retail Price not same with Outlet Price then insert else it will do nothing.
                                            if ((theItem.RetailPrice != ((string)result.Rows[i]["Retail Price"]).GetDecimalValue())
                                                || (usePrice1 && theItem.Userfloat6 != ((string)result.Rows[i]["Userfloat6"]).GetDecimalValue())
                                                || (usePrice2 && theItem.Userfloat7 != ((string)result.Rows[i]["Userfloat7"]).GetDecimalValue())
                                                || (usePrice3 && theItem.Userfloat8 != ((string)result.Rows[i]["Userfloat8"]).GetDecimalValue())
                                                || (usePrice4 && theItem.Userfloat9 != ((string)result.Rows[i]["Userfloat9"]).GetDecimalValue())
                                                || (usePrice5 && theItem.Userfloat10 != ((string)result.Rows[i]["Userfloat10"]).GetDecimalValue()))
                                            {
                                                ogm = new OutletGroupItemMap();
                                                ogm.ItemNo = theItem.ItemNo;
                                                ogm.OutletName = OutletChosen;
                                                ogm.RetailPrice = ((string)result.Rows[i]["Retail Price"]).GetDecimalValue();
                                                ogm.CostPrice = ((string)result.Rows[i]["Cost Price"]).GetDecimalValue();
                                                ogm.Deleted = ((string)result.Rows[i]["Deleted"]).ToUpper().Equals("YES");
                                                if (result.Columns.Contains("Userfloat6") && usePrice1)
                                                {
                                                    ogm.P1 = ((string)result.Rows[i]["Userfloat6"]).GetDecimalValue();
                                                }
                                                if (result.Columns.Contains("Userfloat7") && usePrice2)
                                                {
                                                    ogm.P2 = ((string)result.Rows[i]["Userfloat7"]).GetDecimalValue();
                                                }
                                                if (result.Columns.Contains("Userfloat8") && usePrice3)
                                                {
                                                    ogm.P3 = ((string)result.Rows[i]["Userfloat8"]).GetDecimalValue();
                                                }
                                                if (result.Columns.Contains("Userfloat9") && usePrice4)
                                                {
                                                    ogm.P4 = ((string)result.Rows[i]["Userfloat9"]).GetDecimalValue();
                                                }
                                                if (result.Columns.Contains("Userfloat10") && usePrice5)
                                                {
                                                    ogm.P5 = ((string)result.Rows[i]["Userfloat10"]).GetDecimalValue();
                                                }
                                                qmc.Add(ogm.GetInsertCommand(string.Format("{0} via WEB Item Importer", userName)));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //If Outlet Group Item Map is exist
                                        //If the data is deleted then 
                                        if (((string)result.Rows[i]["Deleted"]).ToUpper().Equals("YES"))
                                        {
                                            ogm.RetailPrice = ((string)result.Rows[i]["Retail Price"]).GetDecimalValue();
                                            ogm.Deleted = true;
                                            if (result.Columns.Contains("Userfloat6") && usePrice1)
                                            {
                                                ogm.P1 = ((string)result.Rows[i]["Userfloat6"]).GetDecimalValue();
                                            }
                                            if (result.Columns.Contains("Userfloat7") && usePrice2)
                                            {
                                                ogm.P2 = ((string)result.Rows[i]["Userfloat7"]).GetDecimalValue();
                                            }
                                            if (result.Columns.Contains("Userfloat8") && usePrice3)
                                            {
                                                ogm.P3 = ((string)result.Rows[i]["Userfloat8"]).GetDecimalValue();
                                            }
                                            if (result.Columns.Contains("Userfloat9") && usePrice4)
                                            {
                                                ogm.P4 = ((string)result.Rows[i]["Userfloat9"]).GetDecimalValue();
                                            }
                                            if (result.Columns.Contains("Userfloat10") && usePrice5)
                                            {
                                                ogm.P5 = ((string)result.Rows[i]["Userfloat10"]).GetDecimalValue();
                                            }
                                            qmc.Add(ogm.GetUpdateCommand(string.Format("{0} via WEB Item Importer", userName)));
                                        }
                                        else
                                        {
                                            //When item Retail Price is same with Outlet Pricing then remove the outlet pricing 
                                            if ((theItem.RetailPrice == ((string)result.Rows[i]["Retail Price"]).GetDecimalValue())
                                                && (!usePrice1 || (usePrice1 && theItem.Userfloat6 == ((string)result.Rows[i]["Userfloat6"]).GetDecimalValue()))
                                                && (!usePrice2 || (usePrice2 && theItem.Userfloat7 == ((string)result.Rows[i]["Userfloat7"]).GetDecimalValue()))
                                                && (!usePrice3 || (usePrice3 && theItem.Userfloat8 == ((string)result.Rows[i]["Userfloat8"]).GetDecimalValue()))
                                                && (!usePrice4 || (usePrice4 && theItem.Userfloat9 == ((string)result.Rows[i]["Userfloat9"]).GetDecimalValue()))
                                                && (!usePrice5 || (usePrice5 && theItem.Userfloat10 == ((string)result.Rows[i]["Userfloat10"]).GetDecimalValue())))
                                            {
                                                string sqlQuery = "Delete OutletGroupItemMap Where ItemNo = '" + ogm.ItemNo + "' and OutletName = '" + ogm.OutletName + "'";
                                                qmc.Add(new QueryCommand(sqlQuery));
                                            }
                                            else
                                            {
                                                //When item Retail Price is not same with Outlet Pricing then update the pricing
                                                ogm.RetailPrice = ((string)result.Rows[i]["Retail Price"]).GetDecimalValue();
                                                ogm.Deleted = ((string)result.Rows[i]["Deleted"]).ToUpper().Equals("YES");
                                                if (result.Columns.Contains("Userfloat6") && usePrice1)
                                                {
                                                    ogm.P1 = ((string)result.Rows[i]["Userfloat6"]).GetDecimalValue();
                                                }
                                                if (result.Columns.Contains("Userfloat7") && usePrice2)
                                                {
                                                    ogm.P2 = ((string)result.Rows[i]["Userfloat7"]).GetDecimalValue();
                                                }
                                                if (result.Columns.Contains("Userfloat8") && usePrice3)
                                                {
                                                    ogm.P3 = ((string)result.Rows[i]["Userfloat8"]).GetDecimalValue();
                                                }
                                                if (result.Columns.Contains("Userfloat9") && usePrice4)
                                                {
                                                    ogm.P4 = ((string)result.Rows[i]["Userfloat9"]).GetDecimalValue();
                                                }
                                                if (result.Columns.Contains("Userfloat10") && usePrice5)
                                                {
                                                    ogm.P5 = ((string)result.Rows[i]["Userfloat10"]).GetDecimalValue();
                                                }
                                                qmc.Add(ogm.GetUpdateCommand(string.Format("{0} via WEB Item Importer", userName)));
                                            }
                                        }
                                    }

                                }
                                else
                                {// Condition Item Is Deleted 
                                    if (ogm == null)
                                    {
                                        // If Item Retail Price not same with Outlet Price then insert else it will do nothing.
                                        if (!(((string)result.Rows[i]["Deleted"]).ToUpper().Equals("YES")))
                                        {
                                            ogm = new OutletGroupItemMap();
                                            ogm.ItemNo = theItem.ItemNo;
                                            ogm.OutletName = OutletChosen;
                                            ogm.RetailPrice = ((string)result.Rows[i]["Retail Price"]).GetDecimalValue();
                                            ogm.CostPrice = ((string)result.Rows[i]["Cost Price"]).GetDecimalValue();
                                            ogm.Deleted = ((string)result.Rows[i]["Deleted"]).ToUpper().Equals("YES");
                                            if (result.Columns.Contains("Userfloat6") && usePrice1)
                                            {
                                                ogm.P1 = ((string)result.Rows[i]["Userfloat6"]).GetDecimalValue();
                                            }
                                            if (result.Columns.Contains("Userfloat7") && usePrice2)
                                            {
                                                ogm.P2 = ((string)result.Rows[i]["Userfloat7"]).GetDecimalValue();
                                            }
                                            if (result.Columns.Contains("Userfloat8") && usePrice3)
                                            {
                                                ogm.P3 = ((string)result.Rows[i]["Userfloat8"]).GetDecimalValue();
                                            }
                                            if (result.Columns.Contains("Userfloat9") && usePrice4)
                                            {
                                                ogm.P4 = ((string)result.Rows[i]["Userfloat9"]).GetDecimalValue();
                                            }
                                            if (result.Columns.Contains("Userfloat10") && usePrice5)
                                            {
                                                ogm.P5 = ((string)result.Rows[i]["Userfloat10"]).GetDecimalValue();
                                            }
                                            qmc.Add(ogm.GetInsertCommand(string.Format("{0} via WEB Item Importer", userName)));
                                        }
                                    }
                                    else
                                    {
                                        //If Outlet Group Item Map is exist
                                        //If the data is deleted then remove the record accordingly
                                        if (((string)result.Rows[i]["Deleted"]).ToUpper().Equals("YES"))
                                        {
                                            //Remove outlet group item map when product master = deleted and outlet group = deleted 
                                            string sqlQuery = "Delete OutletGroupItemMap Where ItemNo = '" + ogm.ItemNo + "' and OutletName = '" + ogm.OutletName + "'";
                                            qmc.Add(new QueryCommand(sqlQuery));

                                        }
                                        else
                                        {
                                            //When item Retail Price is not same with Outlet Pricing then update the pricing
                                            ogm.RetailPrice = ((string)result.Rows[i]["Retail Price"]).GetDecimalValue();
                                            ogm.Deleted = ((string)result.Rows[i]["Deleted"]).ToUpper().Equals("YES");
                                            if (result.Columns.Contains("Userfloat6") && usePrice1)
                                            {
                                                ogm.P1 = ((string)result.Rows[i]["Userfloat6"]).GetDecimalValue();
                                            }
                                            if (result.Columns.Contains("Userfloat7") && usePrice2)
                                            {
                                                ogm.P2 = ((string)result.Rows[i]["Userfloat7"]).GetDecimalValue();
                                            }
                                            if (result.Columns.Contains("Userfloat8") && usePrice3)
                                            {
                                                ogm.P3 = ((string)result.Rows[i]["Userfloat8"]).GetDecimalValue();
                                            }
                                            if (result.Columns.Contains("Userfloat9") && usePrice4)
                                            {
                                                ogm.P4 = ((string)result.Rows[i]["Userfloat9"]).GetDecimalValue();
                                            }
                                            if (result.Columns.Contains("Userfloat10") && usePrice5)
                                            {
                                                ogm.P5 = ((string)result.Rows[i]["Userfloat10"]).GetDecimalValue();
                                            }
                                            qmc.Add(ogm.GetUpdateCommand(string.Format("{0} via WEB Item Importer", userName)));

                                        }
                                    }
                                }
                                //ogm.IsItemDeleted = ;


                            }
                            else if (ApplicableTo == "Outlet Group")
                            {
                                Query query = new Query("OutletGroupItemMap");
                                query.AddWhere(OutletGroupItemMap.Columns.OutletGroupID, OutletChosen.GetIntValue());
                                query.AddWhere(OutletGroupItemMap.Columns.ItemNo, theItem.ItemNo);
                                var ogm = new OutletGroupItemMapController().FetchByQuery(query).FirstOrDefault();
                                if (ogm == null)
                                    ogm = new OutletGroupItemMap();

                                if (!theItem.Deleted.GetValueOrDefault(true))
                                {
                                    //Item is not deleted 
                                    if (ogm == null)
                                    {
                                        // If Item Retail Price not same with Outlet Price then insert else it will do nothing.
                                        if ((theItem.RetailPrice != ((string)result.Rows[i]["Retail Price"]).GetDecimalValue())
                                               || (usePrice1 && theItem.Userfloat6 != ((string)result.Rows[i]["Userfloat6"]).GetDecimalValue())
                                               || (usePrice2 && theItem.Userfloat7 != ((string)result.Rows[i]["Userfloat7"]).GetDecimalValue())
                                               || (usePrice3 && theItem.Userfloat8 != ((string)result.Rows[i]["Userfloat8"]).GetDecimalValue())
                                               || (usePrice4 && theItem.Userfloat9 != ((string)result.Rows[i]["Userfloat9"]).GetDecimalValue())
                                               || (usePrice5 && theItem.Userfloat10 != ((string)result.Rows[i]["Userfloat10"]).GetDecimalValue()))
                                        {
                                            ogm = new OutletGroupItemMap();
                                            ogm.ItemNo = theItem.ItemNo;
                                            ogm.OutletGroupID = OutletChosen.GetIntValue();
                                            ogm.RetailPrice = ((string)result.Rows[i]["Retail Price"]).GetDecimalValue();
                                            ogm.CostPrice = ((string)result.Rows[i]["Cost Price"]).GetDecimalValue();
                                            ogm.Deleted = ((string)result.Rows[i]["Deleted"]).ToUpper().Equals("YES");
                                            if (result.Columns.Contains("Userfloat6") && usePrice1)
                                            {
                                                ogm.P1 = ((string)result.Rows[i]["Userfloat6"]).GetDecimalValue();
                                            }
                                            if (result.Columns.Contains("Userfloat7") && usePrice2)
                                            {
                                                ogm.P2 = ((string)result.Rows[i]["Userfloat7"]).GetDecimalValue();
                                            }
                                            if (result.Columns.Contains("Userfloat8") && usePrice3)
                                            {
                                                ogm.P3 = ((string)result.Rows[i]["Userfloat8"]).GetDecimalValue();
                                            }
                                            if (result.Columns.Contains("Userfloat9") && usePrice4)
                                            {
                                                ogm.P4 = ((string)result.Rows[i]["Userfloat9"]).GetDecimalValue();
                                            }
                                            if (result.Columns.Contains("Userfloat10") && usePrice5)
                                            {
                                                ogm.P5 = ((string)result.Rows[i]["Userfloat10"]).GetDecimalValue();
                                            }
                                            qmc.Add(ogm.GetInsertCommand(string.Format("{0} via WEB Item Importer", userName)));
                                        }
                                    }
                                    else
                                    {
                                        //If Outlet Group Item Map is exist
                                        //If the data is deleted then 
                                        if (((string)result.Rows[i]["Deleted"]).ToUpper().Equals("YES"))
                                        {
                                            ogm.RetailPrice = ((string)result.Rows[i]["Retail Price"]).GetDecimalValue();
                                            ogm.Deleted = true;
                                            if (result.Columns.Contains("Userfloat6") && usePrice1)
                                            {
                                                ogm.P1 = ((string)result.Rows[i]["Userfloat6"]).GetDecimalValue();
                                            }
                                            if (result.Columns.Contains("Userfloat7") && usePrice2)
                                            {
                                                ogm.P2 = ((string)result.Rows[i]["Userfloat7"]).GetDecimalValue();
                                            }
                                            if (result.Columns.Contains("Userfloat8") && usePrice3)
                                            {
                                                ogm.P3 = ((string)result.Rows[i]["Userfloat8"]).GetDecimalValue();
                                            }
                                            if (result.Columns.Contains("Userfloat9") && usePrice4)
                                            {
                                                ogm.P4 = ((string)result.Rows[i]["Userfloat9"]).GetDecimalValue();
                                            }
                                            if (result.Columns.Contains("Userfloat10") && usePrice5)
                                            {
                                                ogm.P5 = ((string)result.Rows[i]["Userfloat10"]).GetDecimalValue();
                                            }
                                            qmc.Add(ogm.GetUpdateCommand(string.Format("{0} via WEB Item Importer", userName)));
                                        }
                                        else
                                        {
                                            //When item Retail Price is same with Outlet Pricing then remove the outlet pricing 
                                            if ((theItem.RetailPrice == ((string)result.Rows[i]["Retail Price"]).GetDecimalValue())
                                               && (!usePrice1 || (usePrice1 && theItem.Userfloat6 == ((string)result.Rows[i]["Userfloat6"]).GetDecimalValue()))
                                               && (!usePrice2 || (usePrice2 && theItem.Userfloat7 == ((string)result.Rows[i]["Userfloat7"]).GetDecimalValue()))
                                               && (!usePrice3 || (usePrice3 && theItem.Userfloat8 == ((string)result.Rows[i]["Userfloat8"]).GetDecimalValue()))
                                               && (!usePrice4 || (usePrice4 && theItem.Userfloat9 == ((string)result.Rows[i]["Userfloat9"]).GetDecimalValue()))
                                               && (!usePrice5 || (usePrice5 && theItem.Userfloat10 == ((string)result.Rows[i]["Userfloat10"]).GetDecimalValue())))
                                            {
                                                string sqlQuery = "Delete OutletGroupItemMap Where ItemNo = '" + ogm.ItemNo + "' and OutletGroupID = " + ogm.OutletGroupID.ToString() + "";
                                                qmc.Add(new QueryCommand(sqlQuery));
                                            }
                                            else
                                            {
                                                //When item Retail Price is not same with Outlet Pricing then update the pricing
                                                ogm.RetailPrice = ((string)result.Rows[i]["Retail Price"]).GetDecimalValue();
                                                ogm.Deleted = ((string)result.Rows[i]["Deleted"]).ToUpper().Equals("YES");
                                                if (result.Columns.Contains("Userfloat6") && usePrice1)
                                                {
                                                    ogm.P1 = ((string)result.Rows[i]["Userfloat6"]).GetDecimalValue();
                                                }
                                                if (result.Columns.Contains("Userfloat7") && usePrice2)
                                                {
                                                    ogm.P2 = ((string)result.Rows[i]["Userfloat7"]).GetDecimalValue();
                                                }
                                                if (result.Columns.Contains("Userfloat8") && usePrice3)
                                                {
                                                    ogm.P3 = ((string)result.Rows[i]["Userfloat8"]).GetDecimalValue();
                                                }
                                                if (result.Columns.Contains("Userfloat9") && usePrice4)
                                                {
                                                    ogm.P4 = ((string)result.Rows[i]["Userfloat9"]).GetDecimalValue();
                                                }
                                                if (result.Columns.Contains("Userfloat10") && usePrice5)
                                                {
                                                    ogm.P5 = ((string)result.Rows[i]["Userfloat10"]).GetDecimalValue();
                                                }
                                                qmc.Add(ogm.GetUpdateCommand(string.Format("{0} via WEB Item Importer", userName)));
                                            }
                                        }
                                    }

                                }
                                else
                                {// Condition Item Is Deleted 
                                    if (ogm == null)
                                    {
                                        // If Item Retail Price not same with Outlet Price then insert else it will do nothing.
                                        if (!(((string)result.Rows[i]["Deleted"]).ToUpper().Equals("YES")))
                                        {
                                            ogm = new OutletGroupItemMap();
                                            ogm.ItemNo = theItem.ItemNo;
                                            ogm.OutletGroupID = OutletChosen.GetIntValue();
                                            ogm.RetailPrice = ((string)result.Rows[i]["Retail Price"]).GetDecimalValue();
                                            ogm.CostPrice = ((string)result.Rows[i]["Cost Price"]).GetDecimalValue();
                                            ogm.Deleted = ((string)result.Rows[i]["Deleted"]).ToUpper().Equals("YES");
                                            if (result.Columns.Contains("Userfloat6") && usePrice1)
                                            {
                                                ogm.P1 = ((string)result.Rows[i]["Userfloat6"]).GetDecimalValue();
                                            }
                                            if (result.Columns.Contains("Userfloat7") && usePrice2)
                                            {
                                                ogm.P2 = ((string)result.Rows[i]["Userfloat7"]).GetDecimalValue();
                                            }
                                            if (result.Columns.Contains("Userfloat8") && usePrice3)
                                            {
                                                ogm.P3 = ((string)result.Rows[i]["Userfloat8"]).GetDecimalValue();
                                            }
                                            if (result.Columns.Contains("Userfloat9") && usePrice4)
                                            {
                                                ogm.P4 = ((string)result.Rows[i]["Userfloat9"]).GetDecimalValue();
                                            }
                                            if (result.Columns.Contains("Userfloat10") && usePrice5)
                                            {
                                                ogm.P5 = ((string)result.Rows[i]["Userfloat10"]).GetDecimalValue();
                                            }
                                            qmc.Add(ogm.GetInsertCommand(string.Format("{0} via WEB Item Importer", userName)));
                                        }
                                    }
                                    else
                                    {
                                        //If Outlet Group Item Map is exist
                                        //If the data is deleted then remove the record accordingly
                                        if (((string)result.Rows[i]["Deleted"]).ToUpper().Equals("YES"))
                                        {
                                            //Remove outlet group item map when product master = deleted and outlet group = deleted 
                                            string sqlQuery = "Delete OutletGroupItemMap Where ItemNo = '" + ogm.ItemNo + "' and OutletGroupID = " + ogm.OutletGroupID.ToString() + "";
                                            qmc.Add(new QueryCommand(sqlQuery));

                                        }
                                        else
                                        {
                                            //When item Retail Price is not same with Outlet Pricing then update the pricing
                                            ogm.RetailPrice = ((string)result.Rows[i]["Retail Price"]).GetDecimalValue();
                                            ogm.Deleted = ((string)result.Rows[i]["Deleted"]).ToUpper().Equals("YES");
                                            if (result.Columns.Contains("Userfloat6") && usePrice1)
                                            {
                                                ogm.P1 = ((string)result.Rows[i]["Userfloat6"]).GetDecimalValue();
                                            }
                                            if (result.Columns.Contains("Userfloat7") && usePrice2)
                                            {
                                                ogm.P2 = ((string)result.Rows[i]["Userfloat7"]).GetDecimalValue();
                                            }
                                            if (result.Columns.Contains("Userfloat8") && usePrice3)
                                            {
                                                ogm.P3 = ((string)result.Rows[i]["Userfloat8"]).GetDecimalValue();
                                            }
                                            if (result.Columns.Contains("Userfloat9") && usePrice4)
                                            {
                                                ogm.P4 = ((string)result.Rows[i]["Userfloat9"]).GetDecimalValue();
                                            }
                                            if (result.Columns.Contains("Userfloat10") && usePrice5)
                                            {
                                                ogm.P5 = ((string)result.Rows[i]["Userfloat10"]).GetDecimalValue();
                                            }
                                            qmc.Add(ogm.GetUpdateCommand(string.Format("{0} via WEB Item Importer", userName)));

                                        }
                                    }
                                }

                                /*ogm.ItemNo = theItem.ItemNo;
                                ogm.OutletGroupID = OutletChosen.GetIntValue();
                                ogm.RetailPrice = ((string)result.Rows[i]["Retail Price"]).GetDecimalValue();
                                ogm.CostPrice = ((string)result.Rows[i]["Cost Price"]).GetDecimalValue();
                                ogm.Deleted = false;
                                ogm.IsItemDeleted = ((string)result.Rows[i]["Deleted"]).ToUpper().Equals("YES");

                                if (ogm.IsNew)
                                    qmc.Add(ogm.GetInsertCommand(string.Format("{0} via WEB Item Importer", userName)));
                                else
                                    qmc.Add(ogm.GetUpdateCommand(string.Format("{0} via WEB Item Importer", userName)));*/
                            }
                        }
                    }
                }

                #endregion

                if (qmc.Count > 0)
                    DataService.ExecuteTransaction(qmc);
                isSuccess = true;
            }
            catch (Exception ex)
            {
                isSuccess = false;
                message = "ERROR : " + ex.Message;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }
    }
}
