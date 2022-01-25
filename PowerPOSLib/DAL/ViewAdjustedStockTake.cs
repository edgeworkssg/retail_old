using System; 
using System.Text; 
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration; 
using System.Xml; 
using System.Xml.Serialization;
using SubSonic; 
using SubSonic.Utilities;
namespace PowerPOS{
    /// <summary>
    /// Strongly-typed collection for the ViewAdjustedStockTake class.
    /// </summary>
    [Serializable]
    public partial class ViewAdjustedStockTakeCollection : ReadOnlyList<ViewAdjustedStockTake, ViewAdjustedStockTakeCollection>
    {        
        public ViewAdjustedStockTakeCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewAdjustedStockTake view.
    /// </summary>
    [Serializable]
    public partial class ViewAdjustedStockTake : ReadOnlyRecord<ViewAdjustedStockTake>, IReadOnlyRecord
    {
    
	    #region Default Settings
	    protected static void SetSQLProps() 
	    {
		    GetTableSchema();
	    }
	    #endregion
        #region Schema Accessor
	    public static TableSchema.Table Schema
        {
            get
            {
                if (BaseSchema == null)
                {
                    SetSQLProps();
                }
                return BaseSchema;
            }
        }
    	
        private static void GetTableSchema() 
        {
            if(!IsSchemaInitialized)
            {
                //Schema declaration
                TableSchema.Table schema = new TableSchema.Table("ViewAdjustedStockTake", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarStockTakeDate = new TableSchema.TableColumn(schema);
                colvarStockTakeDate.ColumnName = "StockTakeDate";
                colvarStockTakeDate.DataType = DbType.DateTime;
                colvarStockTakeDate.MaxLength = 0;
                colvarStockTakeDate.AutoIncrement = false;
                colvarStockTakeDate.IsNullable = false;
                colvarStockTakeDate.IsPrimaryKey = false;
                colvarStockTakeDate.IsForeignKey = false;
                colvarStockTakeDate.IsReadOnly = false;
                
                schema.Columns.Add(colvarStockTakeDate);
                
                TableSchema.TableColumn colvarItemNo = new TableSchema.TableColumn(schema);
                colvarItemNo.ColumnName = "ItemNo";
                colvarItemNo.DataType = DbType.AnsiString;
                colvarItemNo.MaxLength = 50;
                colvarItemNo.AutoIncrement = false;
                colvarItemNo.IsNullable = false;
                colvarItemNo.IsPrimaryKey = false;
                colvarItemNo.IsForeignKey = false;
                colvarItemNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarItemNo);
                
                TableSchema.TableColumn colvarStockTakeID = new TableSchema.TableColumn(schema);
                colvarStockTakeID.ColumnName = "StockTakeID";
                colvarStockTakeID.DataType = DbType.Int32;
                colvarStockTakeID.MaxLength = 0;
                colvarStockTakeID.AutoIncrement = false;
                colvarStockTakeID.IsNullable = false;
                colvarStockTakeID.IsPrimaryKey = false;
                colvarStockTakeID.IsForeignKey = false;
                colvarStockTakeID.IsReadOnly = false;
                
                schema.Columns.Add(colvarStockTakeID);
                
                TableSchema.TableColumn colvarInventoryLocationID = new TableSchema.TableColumn(schema);
                colvarInventoryLocationID.ColumnName = "InventoryLocationID";
                colvarInventoryLocationID.DataType = DbType.Int32;
                colvarInventoryLocationID.MaxLength = 0;
                colvarInventoryLocationID.AutoIncrement = false;
                colvarInventoryLocationID.IsNullable = false;
                colvarInventoryLocationID.IsPrimaryKey = false;
                colvarInventoryLocationID.IsForeignKey = false;
                colvarInventoryLocationID.IsReadOnly = false;
                
                schema.Columns.Add(colvarInventoryLocationID);
                
                TableSchema.TableColumn colvarStockTakeQty = new TableSchema.TableColumn(schema);
                colvarStockTakeQty.ColumnName = "StockTakeQty";
                colvarStockTakeQty.DataType = DbType.Decimal;
                colvarStockTakeQty.MaxLength = 0;
                colvarStockTakeQty.AutoIncrement = false;
                colvarStockTakeQty.IsNullable = true;
                colvarStockTakeQty.IsPrimaryKey = false;
                colvarStockTakeQty.IsForeignKey = false;
                colvarStockTakeQty.IsReadOnly = false;
                
                schema.Columns.Add(colvarStockTakeQty);
                
                TableSchema.TableColumn colvarTakenBy = new TableSchema.TableColumn(schema);
                colvarTakenBy.ColumnName = "TakenBy";
                colvarTakenBy.DataType = DbType.AnsiString;
                colvarTakenBy.MaxLength = 50;
                colvarTakenBy.AutoIncrement = false;
                colvarTakenBy.IsNullable = false;
                colvarTakenBy.IsPrimaryKey = false;
                colvarTakenBy.IsForeignKey = false;
                colvarTakenBy.IsReadOnly = false;
                
                schema.Columns.Add(colvarTakenBy);
                
                TableSchema.TableColumn colvarVerifiedBy = new TableSchema.TableColumn(schema);
                colvarVerifiedBy.ColumnName = "VerifiedBy";
                colvarVerifiedBy.DataType = DbType.AnsiString;
                colvarVerifiedBy.MaxLength = 50;
                colvarVerifiedBy.AutoIncrement = false;
                colvarVerifiedBy.IsNullable = false;
                colvarVerifiedBy.IsPrimaryKey = false;
                colvarVerifiedBy.IsForeignKey = false;
                colvarVerifiedBy.IsReadOnly = false;
                
                schema.Columns.Add(colvarVerifiedBy);
                
                TableSchema.TableColumn colvarAuthorizedBy = new TableSchema.TableColumn(schema);
                colvarAuthorizedBy.ColumnName = "AuthorizedBy";
                colvarAuthorizedBy.DataType = DbType.AnsiString;
                colvarAuthorizedBy.MaxLength = 50;
                colvarAuthorizedBy.AutoIncrement = false;
                colvarAuthorizedBy.IsNullable = false;
                colvarAuthorizedBy.IsPrimaryKey = false;
                colvarAuthorizedBy.IsForeignKey = false;
                colvarAuthorizedBy.IsReadOnly = false;
                
                schema.Columns.Add(colvarAuthorizedBy);
                
                TableSchema.TableColumn colvarIsAdjusted = new TableSchema.TableColumn(schema);
                colvarIsAdjusted.ColumnName = "IsAdjusted";
                colvarIsAdjusted.DataType = DbType.Boolean;
                colvarIsAdjusted.MaxLength = 0;
                colvarIsAdjusted.AutoIncrement = false;
                colvarIsAdjusted.IsNullable = false;
                colvarIsAdjusted.IsPrimaryKey = false;
                colvarIsAdjusted.IsForeignKey = false;
                colvarIsAdjusted.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsAdjusted);
                
                TableSchema.TableColumn colvarRemark = new TableSchema.TableColumn(schema);
                colvarRemark.ColumnName = "Remark";
                colvarRemark.DataType = DbType.AnsiString;
                colvarRemark.MaxLength = 150;
                colvarRemark.AutoIncrement = false;
                colvarRemark.IsNullable = true;
                colvarRemark.IsPrimaryKey = false;
                colvarRemark.IsForeignKey = false;
                colvarRemark.IsReadOnly = false;
                
                schema.Columns.Add(colvarRemark);
                
                TableSchema.TableColumn colvarItemName = new TableSchema.TableColumn(schema);
                colvarItemName.ColumnName = "ItemName";
                colvarItemName.DataType = DbType.String;
                colvarItemName.MaxLength = 300;
                colvarItemName.AutoIncrement = false;
                colvarItemName.IsNullable = false;
                colvarItemName.IsPrimaryKey = false;
                colvarItemName.IsForeignKey = false;
                colvarItemName.IsReadOnly = false;
                
                schema.Columns.Add(colvarItemName);
                
                TableSchema.TableColumn colvarCategoryName = new TableSchema.TableColumn(schema);
                colvarCategoryName.ColumnName = "CategoryName";
                colvarCategoryName.DataType = DbType.String;
                colvarCategoryName.MaxLength = 250;
                colvarCategoryName.AutoIncrement = false;
                colvarCategoryName.IsNullable = false;
                colvarCategoryName.IsPrimaryKey = false;
                colvarCategoryName.IsForeignKey = false;
                colvarCategoryName.IsReadOnly = false;
                
                schema.Columns.Add(colvarCategoryName);
                
                TableSchema.TableColumn colvarIsInInventory = new TableSchema.TableColumn(schema);
                colvarIsInInventory.ColumnName = "IsInInventory";
                colvarIsInInventory.DataType = DbType.Boolean;
                colvarIsInInventory.MaxLength = 0;
                colvarIsInInventory.AutoIncrement = false;
                colvarIsInInventory.IsNullable = false;
                colvarIsInInventory.IsPrimaryKey = false;
                colvarIsInInventory.IsForeignKey = false;
                colvarIsInInventory.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsInInventory);
                
                TableSchema.TableColumn colvarInventoryLocationName = new TableSchema.TableColumn(schema);
                colvarInventoryLocationName.ColumnName = "InventoryLocationName";
                colvarInventoryLocationName.DataType = DbType.AnsiString;
                colvarInventoryLocationName.MaxLength = 50;
                colvarInventoryLocationName.AutoIncrement = false;
                colvarInventoryLocationName.IsNullable = false;
                colvarInventoryLocationName.IsPrimaryKey = false;
                colvarInventoryLocationName.IsForeignKey = false;
                colvarInventoryLocationName.IsReadOnly = false;
                
                schema.Columns.Add(colvarInventoryLocationName);
                
                TableSchema.TableColumn colvarCostOfGoods = new TableSchema.TableColumn(schema);
                colvarCostOfGoods.ColumnName = "CostOfGoods";
                colvarCostOfGoods.DataType = DbType.Currency;
                colvarCostOfGoods.MaxLength = 0;
                colvarCostOfGoods.AutoIncrement = false;
                colvarCostOfGoods.IsNullable = false;
                colvarCostOfGoods.IsPrimaryKey = false;
                colvarCostOfGoods.IsForeignKey = false;
                colvarCostOfGoods.IsReadOnly = false;
                
                schema.Columns.Add(colvarCostOfGoods);
                
                TableSchema.TableColumn colvarIsForSale = new TableSchema.TableColumn(schema);
                colvarIsForSale.ColumnName = "IsForSale";
                colvarIsForSale.DataType = DbType.Boolean;
                colvarIsForSale.MaxLength = 0;
                colvarIsForSale.AutoIncrement = false;
                colvarIsForSale.IsNullable = false;
                colvarIsForSale.IsPrimaryKey = false;
                colvarIsForSale.IsForeignKey = false;
                colvarIsForSale.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsForSale);
                
                TableSchema.TableColumn colvarIsDiscountable = new TableSchema.TableColumn(schema);
                colvarIsDiscountable.ColumnName = "IsDiscountable";
                colvarIsDiscountable.DataType = DbType.Boolean;
                colvarIsDiscountable.MaxLength = 0;
                colvarIsDiscountable.AutoIncrement = false;
                colvarIsDiscountable.IsNullable = false;
                colvarIsDiscountable.IsPrimaryKey = false;
                colvarIsDiscountable.IsForeignKey = false;
                colvarIsDiscountable.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsDiscountable);
                
                TableSchema.TableColumn colvarExpr1 = new TableSchema.TableColumn(schema);
                colvarExpr1.ColumnName = "Expr1";
                colvarExpr1.DataType = DbType.String;
                colvarExpr1.MaxLength = 250;
                colvarExpr1.AutoIncrement = false;
                colvarExpr1.IsNullable = false;
                colvarExpr1.IsPrimaryKey = false;
                colvarExpr1.IsForeignKey = false;
                colvarExpr1.IsReadOnly = false;
                
                schema.Columns.Add(colvarExpr1);
                
                TableSchema.TableColumn colvarCategoryId = new TableSchema.TableColumn(schema);
                colvarCategoryId.ColumnName = "Category_ID";
                colvarCategoryId.DataType = DbType.AnsiString;
                colvarCategoryId.MaxLength = 50;
                colvarCategoryId.AutoIncrement = false;
                colvarCategoryId.IsNullable = true;
                colvarCategoryId.IsPrimaryKey = false;
                colvarCategoryId.IsForeignKey = false;
                colvarCategoryId.IsReadOnly = false;
                
                schema.Columns.Add(colvarCategoryId);
                
                TableSchema.TableColumn colvarIsGST = new TableSchema.TableColumn(schema);
                colvarIsGST.ColumnName = "IsGST";
                colvarIsGST.DataType = DbType.Boolean;
                colvarIsGST.MaxLength = 0;
                colvarIsGST.AutoIncrement = false;
                colvarIsGST.IsNullable = false;
                colvarIsGST.IsPrimaryKey = false;
                colvarIsGST.IsForeignKey = false;
                colvarIsGST.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsGST);
                
                TableSchema.TableColumn colvarAccountCategory = new TableSchema.TableColumn(schema);
                colvarAccountCategory.ColumnName = "AccountCategory";
                colvarAccountCategory.DataType = DbType.AnsiString;
                colvarAccountCategory.MaxLength = 250;
                colvarAccountCategory.AutoIncrement = false;
                colvarAccountCategory.IsNullable = true;
                colvarAccountCategory.IsPrimaryKey = false;
                colvarAccountCategory.IsForeignKey = false;
                colvarAccountCategory.IsReadOnly = false;
                
                schema.Columns.Add(colvarAccountCategory);
                
                TableSchema.TableColumn colvarExpr2 = new TableSchema.TableColumn(schema);
                colvarExpr2.ColumnName = "Expr2";
                colvarExpr2.DataType = DbType.AnsiString;
                colvarExpr2.MaxLength = 50;
                colvarExpr2.AutoIncrement = false;
                colvarExpr2.IsNullable = false;
                colvarExpr2.IsPrimaryKey = false;
                colvarExpr2.IsForeignKey = false;
                colvarExpr2.IsReadOnly = false;
                
                schema.Columns.Add(colvarExpr2);
                
                TableSchema.TableColumn colvarExpr3 = new TableSchema.TableColumn(schema);
                colvarExpr3.ColumnName = "Expr3";
                colvarExpr3.DataType = DbType.String;
                colvarExpr3.MaxLength = 300;
                colvarExpr3.AutoIncrement = false;
                colvarExpr3.IsNullable = false;
                colvarExpr3.IsPrimaryKey = false;
                colvarExpr3.IsForeignKey = false;
                colvarExpr3.IsReadOnly = false;
                
                schema.Columns.Add(colvarExpr3);
                
                TableSchema.TableColumn colvarBarcode = new TableSchema.TableColumn(schema);
                colvarBarcode.ColumnName = "Barcode";
                colvarBarcode.DataType = DbType.AnsiString;
                colvarBarcode.MaxLength = 100;
                colvarBarcode.AutoIncrement = false;
                colvarBarcode.IsNullable = true;
                colvarBarcode.IsPrimaryKey = false;
                colvarBarcode.IsForeignKey = false;
                colvarBarcode.IsReadOnly = false;
                
                schema.Columns.Add(colvarBarcode);
                
                TableSchema.TableColumn colvarRetailPrice = new TableSchema.TableColumn(schema);
                colvarRetailPrice.ColumnName = "RetailPrice";
                colvarRetailPrice.DataType = DbType.Currency;
                colvarRetailPrice.MaxLength = 0;
                colvarRetailPrice.AutoIncrement = false;
                colvarRetailPrice.IsNullable = false;
                colvarRetailPrice.IsPrimaryKey = false;
                colvarRetailPrice.IsForeignKey = false;
                colvarRetailPrice.IsReadOnly = false;
                
                schema.Columns.Add(colvarRetailPrice);
                
                TableSchema.TableColumn colvarFactoryPrice = new TableSchema.TableColumn(schema);
                colvarFactoryPrice.ColumnName = "FactoryPrice";
                colvarFactoryPrice.DataType = DbType.Currency;
                colvarFactoryPrice.MaxLength = 0;
                colvarFactoryPrice.AutoIncrement = false;
                colvarFactoryPrice.IsNullable = false;
                colvarFactoryPrice.IsPrimaryKey = false;
                colvarFactoryPrice.IsForeignKey = false;
                colvarFactoryPrice.IsReadOnly = false;
                
                schema.Columns.Add(colvarFactoryPrice);
                
                TableSchema.TableColumn colvarMinimumPrice = new TableSchema.TableColumn(schema);
                colvarMinimumPrice.ColumnName = "MinimumPrice";
                colvarMinimumPrice.DataType = DbType.Currency;
                colvarMinimumPrice.MaxLength = 0;
                colvarMinimumPrice.AutoIncrement = false;
                colvarMinimumPrice.IsNullable = false;
                colvarMinimumPrice.IsPrimaryKey = false;
                colvarMinimumPrice.IsForeignKey = false;
                colvarMinimumPrice.IsReadOnly = false;
                
                schema.Columns.Add(colvarMinimumPrice);
                
                TableSchema.TableColumn colvarItemDesc = new TableSchema.TableColumn(schema);
                colvarItemDesc.ColumnName = "ItemDesc";
                colvarItemDesc.DataType = DbType.String;
                colvarItemDesc.MaxLength = 250;
                colvarItemDesc.AutoIncrement = false;
                colvarItemDesc.IsNullable = true;
                colvarItemDesc.IsPrimaryKey = false;
                colvarItemDesc.IsForeignKey = false;
                colvarItemDesc.IsReadOnly = false;
                
                schema.Columns.Add(colvarItemDesc);
                
                TableSchema.TableColumn colvarExpr4 = new TableSchema.TableColumn(schema);
                colvarExpr4.ColumnName = "Expr4";
                colvarExpr4.DataType = DbType.Boolean;
                colvarExpr4.MaxLength = 0;
                colvarExpr4.AutoIncrement = false;
                colvarExpr4.IsNullable = false;
                colvarExpr4.IsPrimaryKey = false;
                colvarExpr4.IsForeignKey = false;
                colvarExpr4.IsReadOnly = false;
                
                schema.Columns.Add(colvarExpr4);
                
                TableSchema.TableColumn colvarIsNonDiscountable = new TableSchema.TableColumn(schema);
                colvarIsNonDiscountable.ColumnName = "IsNonDiscountable";
                colvarIsNonDiscountable.DataType = DbType.Boolean;
                colvarIsNonDiscountable.MaxLength = 0;
                colvarIsNonDiscountable.AutoIncrement = false;
                colvarIsNonDiscountable.IsNullable = false;
                colvarIsNonDiscountable.IsPrimaryKey = false;
                colvarIsNonDiscountable.IsForeignKey = false;
                colvarIsNonDiscountable.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsNonDiscountable);
                
                TableSchema.TableColumn colvarBrand = new TableSchema.TableColumn(schema);
                colvarBrand.ColumnName = "Brand";
                colvarBrand.DataType = DbType.String;
                colvarBrand.MaxLength = 50;
                colvarBrand.AutoIncrement = false;
                colvarBrand.IsNullable = true;
                colvarBrand.IsPrimaryKey = false;
                colvarBrand.IsForeignKey = false;
                colvarBrand.IsReadOnly = false;
                
                schema.Columns.Add(colvarBrand);
                
                TableSchema.TableColumn colvarProductLine = new TableSchema.TableColumn(schema);
                colvarProductLine.ColumnName = "ProductLine";
                colvarProductLine.DataType = DbType.AnsiString;
                colvarProductLine.MaxLength = 50;
                colvarProductLine.AutoIncrement = false;
                colvarProductLine.IsNullable = true;
                colvarProductLine.IsPrimaryKey = false;
                colvarProductLine.IsForeignKey = false;
                colvarProductLine.IsReadOnly = false;
                
                schema.Columns.Add(colvarProductLine);
                
                TableSchema.TableColumn colvarExpr5 = new TableSchema.TableColumn(schema);
                colvarExpr5.ColumnName = "Expr5";
                colvarExpr5.DataType = DbType.String;
                colvarExpr5.MaxLength = -1;
                colvarExpr5.AutoIncrement = false;
                colvarExpr5.IsNullable = true;
                colvarExpr5.IsPrimaryKey = false;
                colvarExpr5.IsForeignKey = false;
                colvarExpr5.IsReadOnly = false;
                
                schema.Columns.Add(colvarExpr5);
                
                TableSchema.TableColumn colvarDeleted = new TableSchema.TableColumn(schema);
                colvarDeleted.ColumnName = "Deleted";
                colvarDeleted.DataType = DbType.Boolean;
                colvarDeleted.MaxLength = 0;
                colvarDeleted.AutoIncrement = false;
                colvarDeleted.IsNullable = true;
                colvarDeleted.IsPrimaryKey = false;
                colvarDeleted.IsForeignKey = false;
                colvarDeleted.IsReadOnly = false;
                
                schema.Columns.Add(colvarDeleted);
                
                TableSchema.TableColumn colvarAttributes1 = new TableSchema.TableColumn(schema);
                colvarAttributes1.ColumnName = "Attributes1";
                colvarAttributes1.DataType = DbType.String;
                colvarAttributes1.MaxLength = -1;
                colvarAttributes1.AutoIncrement = false;
                colvarAttributes1.IsNullable = true;
                colvarAttributes1.IsPrimaryKey = false;
                colvarAttributes1.IsForeignKey = false;
                colvarAttributes1.IsReadOnly = false;
                
                schema.Columns.Add(colvarAttributes1);
                
                TableSchema.TableColumn colvarAttributes2 = new TableSchema.TableColumn(schema);
                colvarAttributes2.ColumnName = "Attributes2";
                colvarAttributes2.DataType = DbType.String;
                colvarAttributes2.MaxLength = -1;
                colvarAttributes2.AutoIncrement = false;
                colvarAttributes2.IsNullable = true;
                colvarAttributes2.IsPrimaryKey = false;
                colvarAttributes2.IsForeignKey = false;
                colvarAttributes2.IsReadOnly = false;
                
                schema.Columns.Add(colvarAttributes2);
                
                TableSchema.TableColumn colvarAttributes3 = new TableSchema.TableColumn(schema);
                colvarAttributes3.ColumnName = "Attributes3";
                colvarAttributes3.DataType = DbType.String;
                colvarAttributes3.MaxLength = -1;
                colvarAttributes3.AutoIncrement = false;
                colvarAttributes3.IsNullable = true;
                colvarAttributes3.IsPrimaryKey = false;
                colvarAttributes3.IsForeignKey = false;
                colvarAttributes3.IsReadOnly = false;
                
                schema.Columns.Add(colvarAttributes3);
                
                TableSchema.TableColumn colvarAttributes4 = new TableSchema.TableColumn(schema);
                colvarAttributes4.ColumnName = "Attributes4";
                colvarAttributes4.DataType = DbType.String;
                colvarAttributes4.MaxLength = -1;
                colvarAttributes4.AutoIncrement = false;
                colvarAttributes4.IsNullable = true;
                colvarAttributes4.IsPrimaryKey = false;
                colvarAttributes4.IsForeignKey = false;
                colvarAttributes4.IsReadOnly = false;
                
                schema.Columns.Add(colvarAttributes4);
                
                TableSchema.TableColumn colvarAttributes5 = new TableSchema.TableColumn(schema);
                colvarAttributes5.ColumnName = "Attributes5";
                colvarAttributes5.DataType = DbType.String;
                colvarAttributes5.MaxLength = -1;
                colvarAttributes5.AutoIncrement = false;
                colvarAttributes5.IsNullable = true;
                colvarAttributes5.IsPrimaryKey = false;
                colvarAttributes5.IsForeignKey = false;
                colvarAttributes5.IsReadOnly = false;
                
                schema.Columns.Add(colvarAttributes5);
                
                TableSchema.TableColumn colvarAttributes6 = new TableSchema.TableColumn(schema);
                colvarAttributes6.ColumnName = "Attributes6";
                colvarAttributes6.DataType = DbType.String;
                colvarAttributes6.MaxLength = -1;
                colvarAttributes6.AutoIncrement = false;
                colvarAttributes6.IsNullable = true;
                colvarAttributes6.IsPrimaryKey = false;
                colvarAttributes6.IsForeignKey = false;
                colvarAttributes6.IsReadOnly = false;
                
                schema.Columns.Add(colvarAttributes6);
                
                TableSchema.TableColumn colvarAttributes8 = new TableSchema.TableColumn(schema);
                colvarAttributes8.ColumnName = "Attributes8";
                colvarAttributes8.DataType = DbType.String;
                colvarAttributes8.MaxLength = -1;
                colvarAttributes8.AutoIncrement = false;
                colvarAttributes8.IsNullable = true;
                colvarAttributes8.IsPrimaryKey = false;
                colvarAttributes8.IsForeignKey = false;
                colvarAttributes8.IsReadOnly = false;
                
                schema.Columns.Add(colvarAttributes8);
                
                TableSchema.TableColumn colvarAttributes7 = new TableSchema.TableColumn(schema);
                colvarAttributes7.ColumnName = "Attributes7";
                colvarAttributes7.DataType = DbType.String;
                colvarAttributes7.MaxLength = -1;
                colvarAttributes7.AutoIncrement = false;
                colvarAttributes7.IsNullable = true;
                colvarAttributes7.IsPrimaryKey = false;
                colvarAttributes7.IsForeignKey = false;
                colvarAttributes7.IsReadOnly = false;
                
                schema.Columns.Add(colvarAttributes7);
                
                TableSchema.TableColumn colvarItemDepartmentId = new TableSchema.TableColumn(schema);
                colvarItemDepartmentId.ColumnName = "ItemDepartmentId";
                colvarItemDepartmentId.DataType = DbType.AnsiString;
                colvarItemDepartmentId.MaxLength = 50;
                colvarItemDepartmentId.AutoIncrement = false;
                colvarItemDepartmentId.IsNullable = true;
                colvarItemDepartmentId.IsPrimaryKey = false;
                colvarItemDepartmentId.IsForeignKey = false;
                colvarItemDepartmentId.IsReadOnly = false;
                
                schema.Columns.Add(colvarItemDepartmentId);
                
                TableSchema.TableColumn colvarDepartmentName = new TableSchema.TableColumn(schema);
                colvarDepartmentName.ColumnName = "DepartmentName";
                colvarDepartmentName.DataType = DbType.String;
                colvarDepartmentName.MaxLength = 50;
                colvarDepartmentName.AutoIncrement = false;
                colvarDepartmentName.IsNullable = false;
                colvarDepartmentName.IsPrimaryKey = false;
                colvarDepartmentName.IsForeignKey = false;
                colvarDepartmentName.IsReadOnly = false;
                
                schema.Columns.Add(colvarDepartmentName);
                
                TableSchema.TableColumn colvarSearch = new TableSchema.TableColumn(schema);
                colvarSearch.ColumnName = "search";
                colvarSearch.DataType = DbType.String;
                colvarSearch.MaxLength = 1005;
                colvarSearch.AutoIncrement = false;
                colvarSearch.IsNullable = true;
                colvarSearch.IsPrimaryKey = false;
                colvarSearch.IsForeignKey = false;
                colvarSearch.IsReadOnly = false;
                
                schema.Columns.Add(colvarSearch);
                
                TableSchema.TableColumn colvarIsServiceItem = new TableSchema.TableColumn(schema);
                colvarIsServiceItem.ColumnName = "IsServiceItem";
                colvarIsServiceItem.DataType = DbType.Boolean;
                colvarIsServiceItem.MaxLength = 0;
                colvarIsServiceItem.AutoIncrement = false;
                colvarIsServiceItem.IsNullable = true;
                colvarIsServiceItem.IsPrimaryKey = false;
                colvarIsServiceItem.IsForeignKey = false;
                colvarIsServiceItem.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsServiceItem);
                
                TableSchema.TableColumn colvarIsCourse = new TableSchema.TableColumn(schema);
                colvarIsCourse.ColumnName = "IsCourse";
                colvarIsCourse.DataType = DbType.Boolean;
                colvarIsCourse.MaxLength = 0;
                colvarIsCourse.AutoIncrement = false;
                colvarIsCourse.IsNullable = true;
                colvarIsCourse.IsPrimaryKey = false;
                colvarIsCourse.IsForeignKey = false;
                colvarIsCourse.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsCourse);
                
                TableSchema.TableColumn colvarCourseTypeID = new TableSchema.TableColumn(schema);
                colvarCourseTypeID.ColumnName = "CourseTypeID";
                colvarCourseTypeID.DataType = DbType.AnsiString;
                colvarCourseTypeID.MaxLength = 50;
                colvarCourseTypeID.AutoIncrement = false;
                colvarCourseTypeID.IsNullable = true;
                colvarCourseTypeID.IsPrimaryKey = false;
                colvarCourseTypeID.IsForeignKey = false;
                colvarCourseTypeID.IsReadOnly = false;
                
                schema.Columns.Add(colvarCourseTypeID);
                
                TableSchema.TableColumn colvarProductionDate = new TableSchema.TableColumn(schema);
                colvarProductionDate.ColumnName = "ProductionDate";
                colvarProductionDate.DataType = DbType.DateTime;
                colvarProductionDate.MaxLength = 0;
                colvarProductionDate.AutoIncrement = false;
                colvarProductionDate.IsNullable = true;
                colvarProductionDate.IsPrimaryKey = false;
                colvarProductionDate.IsForeignKey = false;
                colvarProductionDate.IsReadOnly = false;
                
                schema.Columns.Add(colvarProductionDate);
                
                TableSchema.TableColumn colvarExpr6 = new TableSchema.TableColumn(schema);
                colvarExpr6.ColumnName = "Expr6";
                colvarExpr6.DataType = DbType.Boolean;
                colvarExpr6.MaxLength = 0;
                colvarExpr6.AutoIncrement = false;
                colvarExpr6.IsNullable = true;
                colvarExpr6.IsPrimaryKey = false;
                colvarExpr6.IsForeignKey = false;
                colvarExpr6.IsReadOnly = false;
                
                schema.Columns.Add(colvarExpr6);
                
                TableSchema.TableColumn colvarHasWarranty = new TableSchema.TableColumn(schema);
                colvarHasWarranty.ColumnName = "hasWarranty";
                colvarHasWarranty.DataType = DbType.Boolean;
                colvarHasWarranty.MaxLength = 0;
                colvarHasWarranty.AutoIncrement = false;
                colvarHasWarranty.IsNullable = true;
                colvarHasWarranty.IsPrimaryKey = false;
                colvarHasWarranty.IsForeignKey = false;
                colvarHasWarranty.IsReadOnly = false;
                
                schema.Columns.Add(colvarHasWarranty);
                
                TableSchema.TableColumn colvarIsDelivery = new TableSchema.TableColumn(schema);
                colvarIsDelivery.ColumnName = "IsDelivery";
                colvarIsDelivery.DataType = DbType.Boolean;
                colvarIsDelivery.MaxLength = 0;
                colvarIsDelivery.AutoIncrement = false;
                colvarIsDelivery.IsNullable = true;
                colvarIsDelivery.IsPrimaryKey = false;
                colvarIsDelivery.IsForeignKey = false;
                colvarIsDelivery.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsDelivery);
                
                TableSchema.TableColumn colvarGSTRule = new TableSchema.TableColumn(schema);
                colvarGSTRule.ColumnName = "GSTRule";
                colvarGSTRule.DataType = DbType.Int32;
                colvarGSTRule.MaxLength = 0;
                colvarGSTRule.AutoIncrement = false;
                colvarGSTRule.IsNullable = true;
                colvarGSTRule.IsPrimaryKey = false;
                colvarGSTRule.IsForeignKey = false;
                colvarGSTRule.IsReadOnly = false;
                
                schema.Columns.Add(colvarGSTRule);
                
                TableSchema.TableColumn colvarIsVitaMix = new TableSchema.TableColumn(schema);
                colvarIsVitaMix.ColumnName = "IsVitaMix";
                colvarIsVitaMix.DataType = DbType.Boolean;
                colvarIsVitaMix.MaxLength = 0;
                colvarIsVitaMix.AutoIncrement = false;
                colvarIsVitaMix.IsNullable = true;
                colvarIsVitaMix.IsPrimaryKey = false;
                colvarIsVitaMix.IsForeignKey = false;
                colvarIsVitaMix.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsVitaMix);
                
                TableSchema.TableColumn colvarIsWaterFilter = new TableSchema.TableColumn(schema);
                colvarIsWaterFilter.ColumnName = "IsWaterFilter";
                colvarIsWaterFilter.DataType = DbType.Boolean;
                colvarIsWaterFilter.MaxLength = 0;
                colvarIsWaterFilter.AutoIncrement = false;
                colvarIsWaterFilter.IsNullable = true;
                colvarIsWaterFilter.IsPrimaryKey = false;
                colvarIsWaterFilter.IsForeignKey = false;
                colvarIsWaterFilter.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsWaterFilter);
                
                TableSchema.TableColumn colvarIsYoung = new TableSchema.TableColumn(schema);
                colvarIsYoung.ColumnName = "IsYoung";
                colvarIsYoung.DataType = DbType.Boolean;
                colvarIsYoung.MaxLength = 0;
                colvarIsYoung.AutoIncrement = false;
                colvarIsYoung.IsNullable = true;
                colvarIsYoung.IsPrimaryKey = false;
                colvarIsYoung.IsForeignKey = false;
                colvarIsYoung.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsYoung);
                
                TableSchema.TableColumn colvarIsJuicePlus = new TableSchema.TableColumn(schema);
                colvarIsJuicePlus.ColumnName = "IsJuicePlus";
                colvarIsJuicePlus.DataType = DbType.Boolean;
                colvarIsJuicePlus.MaxLength = 0;
                colvarIsJuicePlus.AutoIncrement = false;
                colvarIsJuicePlus.IsNullable = true;
                colvarIsJuicePlus.IsPrimaryKey = false;
                colvarIsJuicePlus.IsForeignKey = false;
                colvarIsJuicePlus.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsJuicePlus);
                
                TableSchema.TableColumn colvarAdjustmentQty = new TableSchema.TableColumn(schema);
                colvarAdjustmentQty.ColumnName = "AdjustmentQty";
                colvarAdjustmentQty.DataType = DbType.Decimal;
                colvarAdjustmentQty.MaxLength = 0;
                colvarAdjustmentQty.AutoIncrement = false;
                colvarAdjustmentQty.IsNullable = true;
                colvarAdjustmentQty.IsPrimaryKey = false;
                colvarAdjustmentQty.IsForeignKey = false;
                colvarAdjustmentQty.IsReadOnly = false;
                
                schema.Columns.Add(colvarAdjustmentQty);
                
                TableSchema.TableColumn colvarBalQtyAtEntry = new TableSchema.TableColumn(schema);
                colvarBalQtyAtEntry.ColumnName = "BalQtyAtEntry";
                colvarBalQtyAtEntry.DataType = DbType.Decimal;
                colvarBalQtyAtEntry.MaxLength = 0;
                colvarBalQtyAtEntry.AutoIncrement = false;
                colvarBalQtyAtEntry.IsNullable = true;
                colvarBalQtyAtEntry.IsPrimaryKey = false;
                colvarBalQtyAtEntry.IsForeignKey = false;
                colvarBalQtyAtEntry.IsReadOnly = false;
                
                schema.Columns.Add(colvarBalQtyAtEntry);
                
                TableSchema.TableColumn colvarTotalDiscrepancyValue = new TableSchema.TableColumn(schema);
                colvarTotalDiscrepancyValue.ColumnName = "TotalDiscrepancyValue";
                colvarTotalDiscrepancyValue.DataType = DbType.Decimal;
                colvarTotalDiscrepancyValue.MaxLength = 0;
                colvarTotalDiscrepancyValue.AutoIncrement = false;
                colvarTotalDiscrepancyValue.IsNullable = true;
                colvarTotalDiscrepancyValue.IsPrimaryKey = false;
                colvarTotalDiscrepancyValue.IsForeignKey = false;
                colvarTotalDiscrepancyValue.IsReadOnly = false;
                
                schema.Columns.Add(colvarTotalDiscrepancyValue);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewAdjustedStockTake",schema);
            }
        }
        #endregion
        
        #region Query Accessor
	    public static Query CreateQuery()
	    {
		    return new Query(Schema);
	    }
	    #endregion
	    
	    #region .ctors
	    public ViewAdjustedStockTake()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewAdjustedStockTake(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewAdjustedStockTake(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewAdjustedStockTake(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("StockTakeDate")]
        [Bindable(true)]
        public DateTime StockTakeDate 
	    {
		    get
		    {
			    return GetColumnValue<DateTime>("StockTakeDate");
		    }
            set 
		    {
			    SetColumnValue("StockTakeDate", value);
            }
        }
	      
        [XmlAttribute("ItemNo")]
        [Bindable(true)]
        public string ItemNo 
	    {
		    get
		    {
			    return GetColumnValue<string>("ItemNo");
		    }
            set 
		    {
			    SetColumnValue("ItemNo", value);
            }
        }
	      
        [XmlAttribute("StockTakeID")]
        [Bindable(true)]
        public int StockTakeID 
	    {
		    get
		    {
			    return GetColumnValue<int>("StockTakeID");
		    }
            set 
		    {
			    SetColumnValue("StockTakeID", value);
            }
        }
	      
        [XmlAttribute("InventoryLocationID")]
        [Bindable(true)]
        public int InventoryLocationID 
	    {
		    get
		    {
			    return GetColumnValue<int>("InventoryLocationID");
		    }
            set 
		    {
			    SetColumnValue("InventoryLocationID", value);
            }
        }
	      
        [XmlAttribute("StockTakeQty")]
        [Bindable(true)]
        public decimal? StockTakeQty 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("StockTakeQty");
		    }
            set 
		    {
			    SetColumnValue("StockTakeQty", value);
            }
        }
	      
        [XmlAttribute("TakenBy")]
        [Bindable(true)]
        public string TakenBy 
	    {
		    get
		    {
			    return GetColumnValue<string>("TakenBy");
		    }
            set 
		    {
			    SetColumnValue("TakenBy", value);
            }
        }
	      
        [XmlAttribute("VerifiedBy")]
        [Bindable(true)]
        public string VerifiedBy 
	    {
		    get
		    {
			    return GetColumnValue<string>("VerifiedBy");
		    }
            set 
		    {
			    SetColumnValue("VerifiedBy", value);
            }
        }
	      
        [XmlAttribute("AuthorizedBy")]
        [Bindable(true)]
        public string AuthorizedBy 
	    {
		    get
		    {
			    return GetColumnValue<string>("AuthorizedBy");
		    }
            set 
		    {
			    SetColumnValue("AuthorizedBy", value);
            }
        }
	      
        [XmlAttribute("IsAdjusted")]
        [Bindable(true)]
        public bool IsAdjusted 
	    {
		    get
		    {
			    return GetColumnValue<bool>("IsAdjusted");
		    }
            set 
		    {
			    SetColumnValue("IsAdjusted", value);
            }
        }
	      
        [XmlAttribute("Remark")]
        [Bindable(true)]
        public string Remark 
	    {
		    get
		    {
			    return GetColumnValue<string>("Remark");
		    }
            set 
		    {
			    SetColumnValue("Remark", value);
            }
        }
	      
        [XmlAttribute("ItemName")]
        [Bindable(true)]
        public string ItemName 
	    {
		    get
		    {
			    return GetColumnValue<string>("ItemName");
		    }
            set 
		    {
			    SetColumnValue("ItemName", value);
            }
        }
	      
        [XmlAttribute("CategoryName")]
        [Bindable(true)]
        public string CategoryName 
	    {
		    get
		    {
			    return GetColumnValue<string>("CategoryName");
		    }
            set 
		    {
			    SetColumnValue("CategoryName", value);
            }
        }
	      
        [XmlAttribute("IsInInventory")]
        [Bindable(true)]
        public bool IsInInventory 
	    {
		    get
		    {
			    return GetColumnValue<bool>("IsInInventory");
		    }
            set 
		    {
			    SetColumnValue("IsInInventory", value);
            }
        }
	      
        [XmlAttribute("InventoryLocationName")]
        [Bindable(true)]
        public string InventoryLocationName 
	    {
		    get
		    {
			    return GetColumnValue<string>("InventoryLocationName");
		    }
            set 
		    {
			    SetColumnValue("InventoryLocationName", value);
            }
        }
	      
        [XmlAttribute("CostOfGoods")]
        [Bindable(true)]
        public decimal CostOfGoods 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("CostOfGoods");
		    }
            set 
		    {
			    SetColumnValue("CostOfGoods", value);
            }
        }
	      
        [XmlAttribute("IsForSale")]
        [Bindable(true)]
        public bool IsForSale 
	    {
		    get
		    {
			    return GetColumnValue<bool>("IsForSale");
		    }
            set 
		    {
			    SetColumnValue("IsForSale", value);
            }
        }
	      
        [XmlAttribute("IsDiscountable")]
        [Bindable(true)]
        public bool IsDiscountable 
	    {
		    get
		    {
			    return GetColumnValue<bool>("IsDiscountable");
		    }
            set 
		    {
			    SetColumnValue("IsDiscountable", value);
            }
        }
	      
        [XmlAttribute("Expr1")]
        [Bindable(true)]
        public string Expr1 
	    {
		    get
		    {
			    return GetColumnValue<string>("Expr1");
		    }
            set 
		    {
			    SetColumnValue("Expr1", value);
            }
        }
	      
        [XmlAttribute("CategoryId")]
        [Bindable(true)]
        public string CategoryId 
	    {
		    get
		    {
			    return GetColumnValue<string>("Category_ID");
		    }
            set 
		    {
			    SetColumnValue("Category_ID", value);
            }
        }
	      
        [XmlAttribute("IsGST")]
        [Bindable(true)]
        public bool IsGST 
	    {
		    get
		    {
			    return GetColumnValue<bool>("IsGST");
		    }
            set 
		    {
			    SetColumnValue("IsGST", value);
            }
        }
	      
        [XmlAttribute("AccountCategory")]
        [Bindable(true)]
        public string AccountCategory 
	    {
		    get
		    {
			    return GetColumnValue<string>("AccountCategory");
		    }
            set 
		    {
			    SetColumnValue("AccountCategory", value);
            }
        }
	      
        [XmlAttribute("Expr2")]
        [Bindable(true)]
        public string Expr2 
	    {
		    get
		    {
			    return GetColumnValue<string>("Expr2");
		    }
            set 
		    {
			    SetColumnValue("Expr2", value);
            }
        }
	      
        [XmlAttribute("Expr3")]
        [Bindable(true)]
        public string Expr3 
	    {
		    get
		    {
			    return GetColumnValue<string>("Expr3");
		    }
            set 
		    {
			    SetColumnValue("Expr3", value);
            }
        }
	      
        [XmlAttribute("Barcode")]
        [Bindable(true)]
        public string Barcode 
	    {
		    get
		    {
			    return GetColumnValue<string>("Barcode");
		    }
            set 
		    {
			    SetColumnValue("Barcode", value);
            }
        }
	      
        [XmlAttribute("RetailPrice")]
        [Bindable(true)]
        public decimal RetailPrice 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("RetailPrice");
		    }
            set 
		    {
			    SetColumnValue("RetailPrice", value);
            }
        }
	      
        [XmlAttribute("FactoryPrice")]
        [Bindable(true)]
        public decimal FactoryPrice 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("FactoryPrice");
		    }
            set 
		    {
			    SetColumnValue("FactoryPrice", value);
            }
        }
	      
        [XmlAttribute("MinimumPrice")]
        [Bindable(true)]
        public decimal MinimumPrice 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("MinimumPrice");
		    }
            set 
		    {
			    SetColumnValue("MinimumPrice", value);
            }
        }
	      
        [XmlAttribute("ItemDesc")]
        [Bindable(true)]
        public string ItemDesc 
	    {
		    get
		    {
			    return GetColumnValue<string>("ItemDesc");
		    }
            set 
		    {
			    SetColumnValue("ItemDesc", value);
            }
        }
	      
        [XmlAttribute("Expr4")]
        [Bindable(true)]
        public bool Expr4 
	    {
		    get
		    {
			    return GetColumnValue<bool>("Expr4");
		    }
            set 
		    {
			    SetColumnValue("Expr4", value);
            }
        }
	      
        [XmlAttribute("IsNonDiscountable")]
        [Bindable(true)]
        public bool IsNonDiscountable 
	    {
		    get
		    {
			    return GetColumnValue<bool>("IsNonDiscountable");
		    }
            set 
		    {
			    SetColumnValue("IsNonDiscountable", value);
            }
        }
	      
        [XmlAttribute("Brand")]
        [Bindable(true)]
        public string Brand 
	    {
		    get
		    {
			    return GetColumnValue<string>("Brand");
		    }
            set 
		    {
			    SetColumnValue("Brand", value);
            }
        }
	      
        [XmlAttribute("ProductLine")]
        [Bindable(true)]
        public string ProductLine 
	    {
		    get
		    {
			    return GetColumnValue<string>("ProductLine");
		    }
            set 
		    {
			    SetColumnValue("ProductLine", value);
            }
        }
	      
        [XmlAttribute("Expr5")]
        [Bindable(true)]
        public string Expr5 
	    {
		    get
		    {
			    return GetColumnValue<string>("Expr5");
		    }
            set 
		    {
			    SetColumnValue("Expr5", value);
            }
        }
	      
        [XmlAttribute("Deleted")]
        [Bindable(true)]
        public bool? Deleted 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("Deleted");
		    }
            set 
		    {
			    SetColumnValue("Deleted", value);
            }
        }
	      
        [XmlAttribute("Attributes1")]
        [Bindable(true)]
        public string Attributes1 
	    {
		    get
		    {
			    return GetColumnValue<string>("Attributes1");
		    }
            set 
		    {
			    SetColumnValue("Attributes1", value);
            }
        }
	      
        [XmlAttribute("Attributes2")]
        [Bindable(true)]
        public string Attributes2 
	    {
		    get
		    {
			    return GetColumnValue<string>("Attributes2");
		    }
            set 
		    {
			    SetColumnValue("Attributes2", value);
            }
        }
	      
        [XmlAttribute("Attributes3")]
        [Bindable(true)]
        public string Attributes3 
	    {
		    get
		    {
			    return GetColumnValue<string>("Attributes3");
		    }
            set 
		    {
			    SetColumnValue("Attributes3", value);
            }
        }
	      
        [XmlAttribute("Attributes4")]
        [Bindable(true)]
        public string Attributes4 
	    {
		    get
		    {
			    return GetColumnValue<string>("Attributes4");
		    }
            set 
		    {
			    SetColumnValue("Attributes4", value);
            }
        }
	      
        [XmlAttribute("Attributes5")]
        [Bindable(true)]
        public string Attributes5 
	    {
		    get
		    {
			    return GetColumnValue<string>("Attributes5");
		    }
            set 
		    {
			    SetColumnValue("Attributes5", value);
            }
        }
	      
        [XmlAttribute("Attributes6")]
        [Bindable(true)]
        public string Attributes6 
	    {
		    get
		    {
			    return GetColumnValue<string>("Attributes6");
		    }
            set 
		    {
			    SetColumnValue("Attributes6", value);
            }
        }
	      
        [XmlAttribute("Attributes8")]
        [Bindable(true)]
        public string Attributes8 
	    {
		    get
		    {
			    return GetColumnValue<string>("Attributes8");
		    }
            set 
		    {
			    SetColumnValue("Attributes8", value);
            }
        }
	      
        [XmlAttribute("Attributes7")]
        [Bindable(true)]
        public string Attributes7 
	    {
		    get
		    {
			    return GetColumnValue<string>("Attributes7");
		    }
            set 
		    {
			    SetColumnValue("Attributes7", value);
            }
        }
	      
        [XmlAttribute("ItemDepartmentId")]
        [Bindable(true)]
        public string ItemDepartmentId 
	    {
		    get
		    {
			    return GetColumnValue<string>("ItemDepartmentId");
		    }
            set 
		    {
			    SetColumnValue("ItemDepartmentId", value);
            }
        }
	      
        [XmlAttribute("DepartmentName")]
        [Bindable(true)]
        public string DepartmentName 
	    {
		    get
		    {
			    return GetColumnValue<string>("DepartmentName");
		    }
            set 
		    {
			    SetColumnValue("DepartmentName", value);
            }
        }
	      
        [XmlAttribute("Search")]
        [Bindable(true)]
        public string Search 
	    {
		    get
		    {
			    return GetColumnValue<string>("search");
		    }
            set 
		    {
			    SetColumnValue("search", value);
            }
        }
	      
        [XmlAttribute("IsServiceItem")]
        [Bindable(true)]
        public bool? IsServiceItem 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("IsServiceItem");
		    }
            set 
		    {
			    SetColumnValue("IsServiceItem", value);
            }
        }
	      
        [XmlAttribute("IsCourse")]
        [Bindable(true)]
        public bool? IsCourse 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("IsCourse");
		    }
            set 
		    {
			    SetColumnValue("IsCourse", value);
            }
        }
	      
        [XmlAttribute("CourseTypeID")]
        [Bindable(true)]
        public string CourseTypeID 
	    {
		    get
		    {
			    return GetColumnValue<string>("CourseTypeID");
		    }
            set 
		    {
			    SetColumnValue("CourseTypeID", value);
            }
        }
	      
        [XmlAttribute("ProductionDate")]
        [Bindable(true)]
        public DateTime? ProductionDate 
	    {
		    get
		    {
			    return GetColumnValue<DateTime?>("ProductionDate");
		    }
            set 
		    {
			    SetColumnValue("ProductionDate", value);
            }
        }
	      
        [XmlAttribute("Expr6")]
        [Bindable(true)]
        public bool? Expr6 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("Expr6");
		    }
            set 
		    {
			    SetColumnValue("Expr6", value);
            }
        }
	      
        [XmlAttribute("HasWarranty")]
        [Bindable(true)]
        public bool? HasWarranty 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("hasWarranty");
		    }
            set 
		    {
			    SetColumnValue("hasWarranty", value);
            }
        }
	      
        [XmlAttribute("IsDelivery")]
        [Bindable(true)]
        public bool? IsDelivery 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("IsDelivery");
		    }
            set 
		    {
			    SetColumnValue("IsDelivery", value);
            }
        }
	      
        [XmlAttribute("GSTRule")]
        [Bindable(true)]
        public int? GSTRule 
	    {
		    get
		    {
			    return GetColumnValue<int?>("GSTRule");
		    }
            set 
		    {
			    SetColumnValue("GSTRule", value);
            }
        }
	      
        [XmlAttribute("IsVitaMix")]
        [Bindable(true)]
        public bool? IsVitaMix 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("IsVitaMix");
		    }
            set 
		    {
			    SetColumnValue("IsVitaMix", value);
            }
        }
	      
        [XmlAttribute("IsWaterFilter")]
        [Bindable(true)]
        public bool? IsWaterFilter 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("IsWaterFilter");
		    }
            set 
		    {
			    SetColumnValue("IsWaterFilter", value);
            }
        }
	      
        [XmlAttribute("IsYoung")]
        [Bindable(true)]
        public bool? IsYoung 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("IsYoung");
		    }
            set 
		    {
			    SetColumnValue("IsYoung", value);
            }
        }
	      
        [XmlAttribute("IsJuicePlus")]
        [Bindable(true)]
        public bool? IsJuicePlus 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("IsJuicePlus");
		    }
            set 
		    {
			    SetColumnValue("IsJuicePlus", value);
            }
        }
	      
        [XmlAttribute("AdjustmentQty")]
        [Bindable(true)]
        public decimal? AdjustmentQty 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("AdjustmentQty");
		    }
            set 
		    {
			    SetColumnValue("AdjustmentQty", value);
            }
        }
	      
        [XmlAttribute("BalQtyAtEntry")]
        [Bindable(true)]
        public decimal? BalQtyAtEntry 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("BalQtyAtEntry");
		    }
            set 
		    {
			    SetColumnValue("BalQtyAtEntry", value);
            }
        }
	      
        [XmlAttribute("TotalDiscrepancyValue")]
        [Bindable(true)]
        public decimal? TotalDiscrepancyValue 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("TotalDiscrepancyValue");
		    }
            set 
		    {
			    SetColumnValue("TotalDiscrepancyValue", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string StockTakeDate = @"StockTakeDate";
            
            public static string ItemNo = @"ItemNo";
            
            public static string StockTakeID = @"StockTakeID";
            
            public static string InventoryLocationID = @"InventoryLocationID";
            
            public static string StockTakeQty = @"StockTakeQty";
            
            public static string TakenBy = @"TakenBy";
            
            public static string VerifiedBy = @"VerifiedBy";
            
            public static string AuthorizedBy = @"AuthorizedBy";
            
            public static string IsAdjusted = @"IsAdjusted";
            
            public static string Remark = @"Remark";
            
            public static string ItemName = @"ItemName";
            
            public static string CategoryName = @"CategoryName";
            
            public static string IsInInventory = @"IsInInventory";
            
            public static string InventoryLocationName = @"InventoryLocationName";
            
            public static string CostOfGoods = @"CostOfGoods";
            
            public static string IsForSale = @"IsForSale";
            
            public static string IsDiscountable = @"IsDiscountable";
            
            public static string Expr1 = @"Expr1";
            
            public static string CategoryId = @"Category_ID";
            
            public static string IsGST = @"IsGST";
            
            public static string AccountCategory = @"AccountCategory";
            
            public static string Expr2 = @"Expr2";
            
            public static string Expr3 = @"Expr3";
            
            public static string Barcode = @"Barcode";
            
            public static string RetailPrice = @"RetailPrice";
            
            public static string FactoryPrice = @"FactoryPrice";
            
            public static string MinimumPrice = @"MinimumPrice";
            
            public static string ItemDesc = @"ItemDesc";
            
            public static string Expr4 = @"Expr4";
            
            public static string IsNonDiscountable = @"IsNonDiscountable";
            
            public static string Brand = @"Brand";
            
            public static string ProductLine = @"ProductLine";
            
            public static string Expr5 = @"Expr5";
            
            public static string Deleted = @"Deleted";
            
            public static string Attributes1 = @"Attributes1";
            
            public static string Attributes2 = @"Attributes2";
            
            public static string Attributes3 = @"Attributes3";
            
            public static string Attributes4 = @"Attributes4";
            
            public static string Attributes5 = @"Attributes5";
            
            public static string Attributes6 = @"Attributes6";
            
            public static string Attributes8 = @"Attributes8";
            
            public static string Attributes7 = @"Attributes7";
            
            public static string ItemDepartmentId = @"ItemDepartmentId";
            
            public static string DepartmentName = @"DepartmentName";
            
            public static string Search = @"search";
            
            public static string IsServiceItem = @"IsServiceItem";
            
            public static string IsCourse = @"IsCourse";
            
            public static string CourseTypeID = @"CourseTypeID";
            
            public static string ProductionDate = @"ProductionDate";
            
            public static string Expr6 = @"Expr6";
            
            public static string HasWarranty = @"hasWarranty";
            
            public static string IsDelivery = @"IsDelivery";
            
            public static string GSTRule = @"GSTRule";
            
            public static string IsVitaMix = @"IsVitaMix";
            
            public static string IsWaterFilter = @"IsWaterFilter";
            
            public static string IsYoung = @"IsYoung";
            
            public static string IsJuicePlus = @"IsJuicePlus";
            
            public static string AdjustmentQty = @"AdjustmentQty";
            
            public static string BalQtyAtEntry = @"BalQtyAtEntry";
            
            public static string TotalDiscrepancyValue = @"TotalDiscrepancyValue";
            
	    }
	    #endregion
	    
	    
	    #region IAbstractRecord Members
        public new CT GetColumnValue<CT>(string columnName) {
            return base.GetColumnValue<CT>(columnName);
        }
        public object GetColumnValue(string columnName) {
            return base.GetColumnValue<object>(columnName);
        }
        #endregion
	    
    }
}
