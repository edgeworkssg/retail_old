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
    /// Strongly-typed collection for the ViewInventoryActivity class.
    /// </summary>
    [Serializable]
    public partial class ViewInventoryActivityCollection : ReadOnlyList<ViewInventoryActivity, ViewInventoryActivityCollection>
    {        
        public ViewInventoryActivityCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewInventoryActivity view.
    /// </summary>
    [Serializable]
    public partial class ViewInventoryActivity : ReadOnlyRecord<ViewInventoryActivity>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewInventoryActivity", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarInventoryDetRefNo = new TableSchema.TableColumn(schema);
                colvarInventoryDetRefNo.ColumnName = "InventoryDetRefNo";
                colvarInventoryDetRefNo.DataType = DbType.AnsiString;
                colvarInventoryDetRefNo.MaxLength = 50;
                colvarInventoryDetRefNo.AutoIncrement = false;
                colvarInventoryDetRefNo.IsNullable = false;
                colvarInventoryDetRefNo.IsPrimaryKey = false;
                colvarInventoryDetRefNo.IsForeignKey = false;
                colvarInventoryDetRefNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarInventoryDetRefNo);
                
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
                
                TableSchema.TableColumn colvarQuantity = new TableSchema.TableColumn(schema);
                colvarQuantity.ColumnName = "Quantity";
                colvarQuantity.DataType = DbType.Decimal;
                colvarQuantity.MaxLength = 0;
                colvarQuantity.AutoIncrement = false;
                colvarQuantity.IsNullable = true;
                colvarQuantity.IsPrimaryKey = false;
                colvarQuantity.IsForeignKey = false;
                colvarQuantity.IsReadOnly = false;
                
                schema.Columns.Add(colvarQuantity);
                
                TableSchema.TableColumn colvarRemainingQty = new TableSchema.TableColumn(schema);
                colvarRemainingQty.ColumnName = "RemainingQty";
                colvarRemainingQty.DataType = DbType.Decimal;
                colvarRemainingQty.MaxLength = 0;
                colvarRemainingQty.AutoIncrement = false;
                colvarRemainingQty.IsNullable = true;
                colvarRemainingQty.IsPrimaryKey = false;
                colvarRemainingQty.IsForeignKey = false;
                colvarRemainingQty.IsReadOnly = false;
                
                schema.Columns.Add(colvarRemainingQty);
                
                TableSchema.TableColumn colvarItemRemark = new TableSchema.TableColumn(schema);
                colvarItemRemark.ColumnName = "ItemRemark";
                colvarItemRemark.DataType = DbType.String;
                colvarItemRemark.MaxLength = -1;
                colvarItemRemark.AutoIncrement = false;
                colvarItemRemark.IsNullable = true;
                colvarItemRemark.IsPrimaryKey = false;
                colvarItemRemark.IsForeignKey = false;
                colvarItemRemark.IsReadOnly = false;
                
                schema.Columns.Add(colvarItemRemark);
                
                TableSchema.TableColumn colvarInvoiceNo = new TableSchema.TableColumn(schema);
                colvarInvoiceNo.ColumnName = "InvoiceNo";
                colvarInvoiceNo.DataType = DbType.AnsiString;
                colvarInvoiceNo.MaxLength = 50;
                colvarInvoiceNo.AutoIncrement = false;
                colvarInvoiceNo.IsNullable = true;
                colvarInvoiceNo.IsPrimaryKey = false;
                colvarInvoiceNo.IsForeignKey = false;
                colvarInvoiceNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarInvoiceNo);
                
                TableSchema.TableColumn colvarSupplier = new TableSchema.TableColumn(schema);
                colvarSupplier.ColumnName = "Supplier";
                colvarSupplier.DataType = DbType.AnsiString;
                colvarSupplier.MaxLength = 50;
                colvarSupplier.AutoIncrement = false;
                colvarSupplier.IsNullable = true;
                colvarSupplier.IsPrimaryKey = false;
                colvarSupplier.IsForeignKey = false;
                colvarSupplier.IsReadOnly = false;
                
                schema.Columns.Add(colvarSupplier);
                
                TableSchema.TableColumn colvarRemark = new TableSchema.TableColumn(schema);
                colvarRemark.ColumnName = "Remark";
                colvarRemark.DataType = DbType.String;
                colvarRemark.MaxLength = -1;
                colvarRemark.AutoIncrement = false;
                colvarRemark.IsNullable = false;
                colvarRemark.IsPrimaryKey = false;
                colvarRemark.IsForeignKey = false;
                colvarRemark.IsReadOnly = false;
                
                schema.Columns.Add(colvarRemark);
                
                TableSchema.TableColumn colvarInventoryDate = new TableSchema.TableColumn(schema);
                colvarInventoryDate.ColumnName = "InventoryDate";
                colvarInventoryDate.DataType = DbType.DateTime;
                colvarInventoryDate.MaxLength = 0;
                colvarInventoryDate.AutoIncrement = false;
                colvarInventoryDate.IsNullable = false;
                colvarInventoryDate.IsPrimaryKey = false;
                colvarInventoryDate.IsForeignKey = false;
                colvarInventoryDate.IsReadOnly = false;
                
                schema.Columns.Add(colvarInventoryDate);
                
                TableSchema.TableColumn colvarUserName = new TableSchema.TableColumn(schema);
                colvarUserName.ColumnName = "UserName";
                colvarUserName.DataType = DbType.AnsiString;
                colvarUserName.MaxLength = 50;
                colvarUserName.AutoIncrement = false;
                colvarUserName.IsNullable = true;
                colvarUserName.IsPrimaryKey = false;
                colvarUserName.IsForeignKey = false;
                colvarUserName.IsReadOnly = false;
                
                schema.Columns.Add(colvarUserName);
                
                TableSchema.TableColumn colvarInventoryHdrRefNo = new TableSchema.TableColumn(schema);
                colvarInventoryHdrRefNo.ColumnName = "InventoryHdrRefNo";
                colvarInventoryHdrRefNo.DataType = DbType.AnsiString;
                colvarInventoryHdrRefNo.MaxLength = 50;
                colvarInventoryHdrRefNo.AutoIncrement = false;
                colvarInventoryHdrRefNo.IsNullable = false;
                colvarInventoryHdrRefNo.IsPrimaryKey = false;
                colvarInventoryHdrRefNo.IsForeignKey = false;
                colvarInventoryHdrRefNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarInventoryHdrRefNo);
                
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
                
                TableSchema.TableColumn colvarMovementType = new TableSchema.TableColumn(schema);
                colvarMovementType.ColumnName = "MovementType";
                colvarMovementType.DataType = DbType.AnsiString;
                colvarMovementType.MaxLength = 50;
                colvarMovementType.AutoIncrement = false;
                colvarMovementType.IsNullable = true;
                colvarMovementType.IsPrimaryKey = false;
                colvarMovementType.IsForeignKey = false;
                colvarMovementType.IsReadOnly = false;
                
                schema.Columns.Add(colvarMovementType);
                
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
                
                TableSchema.TableColumn colvarStockInRefNo = new TableSchema.TableColumn(schema);
                colvarStockInRefNo.ColumnName = "StockInRefNo";
                colvarStockInRefNo.DataType = DbType.AnsiString;
                colvarStockInRefNo.MaxLength = 50;
                colvarStockInRefNo.AutoIncrement = false;
                colvarStockInRefNo.IsNullable = true;
                colvarStockInRefNo.IsPrimaryKey = false;
                colvarStockInRefNo.IsForeignKey = false;
                colvarStockInRefNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarStockInRefNo);
                
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
                
                TableSchema.TableColumn colvarIsDiscrepancy = new TableSchema.TableColumn(schema);
                colvarIsDiscrepancy.ColumnName = "IsDiscrepancy";
                colvarIsDiscrepancy.DataType = DbType.Boolean;
                colvarIsDiscrepancy.MaxLength = 0;
                colvarIsDiscrepancy.AutoIncrement = false;
                colvarIsDiscrepancy.IsNullable = false;
                colvarIsDiscrepancy.IsPrimaryKey = false;
                colvarIsDiscrepancy.IsForeignKey = false;
                colvarIsDiscrepancy.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsDiscrepancy);
                
                TableSchema.TableColumn colvarReasonName = new TableSchema.TableColumn(schema);
                colvarReasonName.ColumnName = "ReasonName";
                colvarReasonName.DataType = DbType.AnsiString;
                colvarReasonName.MaxLength = 50;
                colvarReasonName.AutoIncrement = false;
                colvarReasonName.IsNullable = true;
                colvarReasonName.IsPrimaryKey = false;
                colvarReasonName.IsForeignKey = false;
                colvarReasonName.IsReadOnly = false;
                
                schema.Columns.Add(colvarReasonName);
                
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
                
                TableSchema.TableColumn colvarGst = new TableSchema.TableColumn(schema);
                colvarGst.ColumnName = "GST";
                colvarGst.DataType = DbType.Double;
                colvarGst.MaxLength = 0;
                colvarGst.AutoIncrement = false;
                colvarGst.IsNullable = false;
                colvarGst.IsPrimaryKey = false;
                colvarGst.IsForeignKey = false;
                colvarGst.IsReadOnly = false;
                
                schema.Columns.Add(colvarGst);
                
                TableSchema.TableColumn colvarStockOutReasonID = new TableSchema.TableColumn(schema);
                colvarStockOutReasonID.ColumnName = "StockOutReasonID";
                colvarStockOutReasonID.DataType = DbType.Int32;
                colvarStockOutReasonID.MaxLength = 0;
                colvarStockOutReasonID.AutoIncrement = false;
                colvarStockOutReasonID.IsNullable = true;
                colvarStockOutReasonID.IsPrimaryKey = false;
                colvarStockOutReasonID.IsForeignKey = false;
                colvarStockOutReasonID.IsReadOnly = false;
                
                schema.Columns.Add(colvarStockOutReasonID);
                
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
                
                TableSchema.TableColumn colvarFactoryPriceUSD = new TableSchema.TableColumn(schema);
                colvarFactoryPriceUSD.ColumnName = "FactoryPriceUSD";
                colvarFactoryPriceUSD.DataType = DbType.Currency;
                colvarFactoryPriceUSD.MaxLength = 0;
                colvarFactoryPriceUSD.AutoIncrement = false;
                colvarFactoryPriceUSD.IsNullable = false;
                colvarFactoryPriceUSD.IsPrimaryKey = false;
                colvarFactoryPriceUSD.IsForeignKey = false;
                colvarFactoryPriceUSD.IsReadOnly = false;
                
                schema.Columns.Add(colvarFactoryPriceUSD);
                
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
                
                TableSchema.TableColumn colvarDepartmentID = new TableSchema.TableColumn(schema);
                colvarDepartmentID.ColumnName = "DepartmentID";
                colvarDepartmentID.DataType = DbType.Int32;
                colvarDepartmentID.MaxLength = 0;
                colvarDepartmentID.AutoIncrement = false;
                colvarDepartmentID.IsNullable = true;
                colvarDepartmentID.IsPrimaryKey = false;
                colvarDepartmentID.IsForeignKey = false;
                colvarDepartmentID.IsReadOnly = false;
                
                schema.Columns.Add(colvarDepartmentID);
                
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
                
                TableSchema.TableColumn colvarExpr1 = new TableSchema.TableColumn(schema);
                colvarExpr1.ColumnName = "Expr1";
                colvarExpr1.DataType = DbType.String;
                colvarExpr1.MaxLength = -1;
                colvarExpr1.AutoIncrement = false;
                colvarExpr1.IsNullable = true;
                colvarExpr1.IsPrimaryKey = false;
                colvarExpr1.IsForeignKey = false;
                colvarExpr1.IsReadOnly = false;
                
                schema.Columns.Add(colvarExpr1);
                
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
                
                TableSchema.TableColumn colvarBalanceBefore = new TableSchema.TableColumn(schema);
                colvarBalanceBefore.ColumnName = "BalanceBefore";
                colvarBalanceBefore.DataType = DbType.Int32;
                colvarBalanceBefore.MaxLength = 0;
                colvarBalanceBefore.AutoIncrement = false;
                colvarBalanceBefore.IsNullable = false;
                colvarBalanceBefore.IsPrimaryKey = false;
                colvarBalanceBefore.IsForeignKey = false;
                colvarBalanceBefore.IsReadOnly = false;
                
                schema.Columns.Add(colvarBalanceBefore);
                
                TableSchema.TableColumn colvarBalanceAfter = new TableSchema.TableColumn(schema);
                colvarBalanceAfter.ColumnName = "BalanceAfter";
                colvarBalanceAfter.DataType = DbType.Int32;
                colvarBalanceAfter.MaxLength = 0;
                colvarBalanceAfter.AutoIncrement = false;
                colvarBalanceAfter.IsNullable = false;
                colvarBalanceAfter.IsPrimaryKey = false;
                colvarBalanceAfter.IsForeignKey = false;
                colvarBalanceAfter.IsReadOnly = false;
                
                schema.Columns.Add(colvarBalanceAfter);
                
                TableSchema.TableColumn colvarExpiryDate = new TableSchema.TableColumn(schema);
                colvarExpiryDate.ColumnName = "ExpiryDate";
                colvarExpiryDate.DataType = DbType.DateTime;
                colvarExpiryDate.MaxLength = 0;
                colvarExpiryDate.AutoIncrement = false;
                colvarExpiryDate.IsNullable = true;
                colvarExpiryDate.IsPrimaryKey = false;
                colvarExpiryDate.IsForeignKey = false;
                colvarExpiryDate.IsReadOnly = false;
                
                schema.Columns.Add(colvarExpiryDate);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewInventoryActivity",schema);
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
	    public ViewInventoryActivity()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewInventoryActivity(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewInventoryActivity(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewInventoryActivity(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("InventoryDetRefNo")]
        [Bindable(true)]
        public string InventoryDetRefNo 
	    {
		    get
		    {
			    return GetColumnValue<string>("InventoryDetRefNo");
		    }
            set 
		    {
			    SetColumnValue("InventoryDetRefNo", value);
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
	      
        [XmlAttribute("Quantity")]
        [Bindable(true)]
        public decimal? Quantity 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Quantity");
		    }
            set 
		    {
			    SetColumnValue("Quantity", value);
            }
        }
	      
        [XmlAttribute("RemainingQty")]
        [Bindable(true)]
        public decimal? RemainingQty 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("RemainingQty");
		    }
            set 
		    {
			    SetColumnValue("RemainingQty", value);
            }
        }
	      
        [XmlAttribute("ItemRemark")]
        [Bindable(true)]
        public string ItemRemark 
	    {
		    get
		    {
			    return GetColumnValue<string>("ItemRemark");
		    }
            set 
		    {
			    SetColumnValue("ItemRemark", value);
            }
        }
	      
        [XmlAttribute("InvoiceNo")]
        [Bindable(true)]
        public string InvoiceNo 
	    {
		    get
		    {
			    return GetColumnValue<string>("InvoiceNo");
		    }
            set 
		    {
			    SetColumnValue("InvoiceNo", value);
            }
        }
	      
        [XmlAttribute("Supplier")]
        [Bindable(true)]
        public string Supplier 
	    {
		    get
		    {
			    return GetColumnValue<string>("Supplier");
		    }
            set 
		    {
			    SetColumnValue("Supplier", value);
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
	      
        [XmlAttribute("InventoryDate")]
        [Bindable(true)]
        public DateTime InventoryDate 
	    {
		    get
		    {
			    return GetColumnValue<DateTime>("InventoryDate");
		    }
            set 
		    {
			    SetColumnValue("InventoryDate", value);
            }
        }
	      
        [XmlAttribute("UserName")]
        [Bindable(true)]
        public string UserName 
	    {
		    get
		    {
			    return GetColumnValue<string>("UserName");
		    }
            set 
		    {
			    SetColumnValue("UserName", value);
            }
        }
	      
        [XmlAttribute("InventoryHdrRefNo")]
        [Bindable(true)]
        public string InventoryHdrRefNo 
	    {
		    get
		    {
			    return GetColumnValue<string>("InventoryHdrRefNo");
		    }
            set 
		    {
			    SetColumnValue("InventoryHdrRefNo", value);
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
	      
        [XmlAttribute("MovementType")]
        [Bindable(true)]
        public string MovementType 
	    {
		    get
		    {
			    return GetColumnValue<string>("MovementType");
		    }
            set 
		    {
			    SetColumnValue("MovementType", value);
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
	      
        [XmlAttribute("StockInRefNo")]
        [Bindable(true)]
        public string StockInRefNo 
	    {
		    get
		    {
			    return GetColumnValue<string>("StockInRefNo");
		    }
            set 
		    {
			    SetColumnValue("StockInRefNo", value);
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
	      
        [XmlAttribute("IsDiscrepancy")]
        [Bindable(true)]
        public bool IsDiscrepancy 
	    {
		    get
		    {
			    return GetColumnValue<bool>("IsDiscrepancy");
		    }
            set 
		    {
			    SetColumnValue("IsDiscrepancy", value);
            }
        }
	      
        [XmlAttribute("ReasonName")]
        [Bindable(true)]
        public string ReasonName 
	    {
		    get
		    {
			    return GetColumnValue<string>("ReasonName");
		    }
            set 
		    {
			    SetColumnValue("ReasonName", value);
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
	      
        [XmlAttribute("Gst")]
        [Bindable(true)]
        public double Gst 
	    {
		    get
		    {
			    return GetColumnValue<double>("GST");
		    }
            set 
		    {
			    SetColumnValue("GST", value);
            }
        }
	      
        [XmlAttribute("StockOutReasonID")]
        [Bindable(true)]
        public int? StockOutReasonID 
	    {
		    get
		    {
			    return GetColumnValue<int?>("StockOutReasonID");
		    }
            set 
		    {
			    SetColumnValue("StockOutReasonID", value);
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
	      
        [XmlAttribute("FactoryPriceUSD")]
        [Bindable(true)]
        public decimal FactoryPriceUSD 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("FactoryPriceUSD");
		    }
            set 
		    {
			    SetColumnValue("FactoryPriceUSD", value);
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
	      
        [XmlAttribute("DepartmentID")]
        [Bindable(true)]
        public int? DepartmentID 
	    {
		    get
		    {
			    return GetColumnValue<int?>("DepartmentID");
		    }
            set 
		    {
			    SetColumnValue("DepartmentID", value);
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
	      
        [XmlAttribute("BalanceBefore")]
        [Bindable(true)]
        public int BalanceBefore 
	    {
		    get
		    {
			    return GetColumnValue<int>("BalanceBefore");
		    }
            set 
		    {
			    SetColumnValue("BalanceBefore", value);
            }
        }
	      
        [XmlAttribute("BalanceAfter")]
        [Bindable(true)]
        public int BalanceAfter 
	    {
		    get
		    {
			    return GetColumnValue<int>("BalanceAfter");
		    }
            set 
		    {
			    SetColumnValue("BalanceAfter", value);
            }
        }
	      
        [XmlAttribute("ExpiryDate")]
        [Bindable(true)]
        public DateTime? ExpiryDate 
	    {
		    get
		    {
			    return GetColumnValue<DateTime?>("ExpiryDate");
		    }
            set 
		    {
			    SetColumnValue("ExpiryDate", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string InventoryDetRefNo = @"InventoryDetRefNo";
            
            public static string ItemNo = @"ItemNo";
            
            public static string Quantity = @"Quantity";
            
            public static string RemainingQty = @"RemainingQty";
            
            public static string ItemRemark = @"ItemRemark";
            
            public static string InvoiceNo = @"InvoiceNo";
            
            public static string Supplier = @"Supplier";
            
            public static string Remark = @"Remark";
            
            public static string InventoryDate = @"InventoryDate";
            
            public static string UserName = @"UserName";
            
            public static string InventoryHdrRefNo = @"InventoryHdrRefNo";
            
            public static string ItemName = @"ItemName";
            
            public static string CategoryName = @"CategoryName";
            
            public static string InventoryLocationName = @"InventoryLocationName";
            
            public static string MovementType = @"MovementType";
            
            public static string CostOfGoods = @"CostOfGoods";
            
            public static string StockInRefNo = @"StockInRefNo";
            
            public static string InventoryLocationID = @"InventoryLocationID";
            
            public static string IsDiscrepancy = @"IsDiscrepancy";
            
            public static string ReasonName = @"ReasonName";
            
            public static string FactoryPrice = @"FactoryPrice";
            
            public static string Gst = @"GST";
            
            public static string StockOutReasonID = @"StockOutReasonID";
            
            public static string RetailPrice = @"RetailPrice";
            
            public static string FactoryPriceUSD = @"FactoryPriceUSD";
            
            public static string ProductLine = @"ProductLine";
            
            public static string DepartmentID = @"DepartmentID";
            
            public static string Attributes1 = @"Attributes1";
            
            public static string Attributes2 = @"Attributes2";
            
            public static string Attributes3 = @"Attributes3";
            
            public static string Attributes4 = @"Attributes4";
            
            public static string Attributes5 = @"Attributes5";
            
            public static string Attributes6 = @"Attributes6";
            
            public static string Attributes7 = @"Attributes7";
            
            public static string Attributes8 = @"Attributes8";
            
            public static string Expr1 = @"Expr1";
            
            public static string ProductionDate = @"ProductionDate";
            
            public static string HasWarranty = @"hasWarranty";
            
            public static string IsDelivery = @"IsDelivery";
            
            public static string GSTRule = @"GSTRule";
            
            public static string IsVitaMix = @"IsVitaMix";
            
            public static string IsWaterFilter = @"IsWaterFilter";
            
            public static string IsYoung = @"IsYoung";
            
            public static string IsJuicePlus = @"IsJuicePlus";
            
            public static string IsCourse = @"IsCourse";
            
            public static string CourseTypeID = @"CourseTypeID";
            
            public static string IsServiceItem = @"IsServiceItem";
            
            public static string Search = @"search";
            
            public static string DepartmentName = @"DepartmentName";
            
            public static string ItemDepartmentId = @"ItemDepartmentId";
            
            public static string Barcode = @"Barcode";
            
            public static string ItemDesc = @"ItemDesc";
            
            public static string MinimumPrice = @"MinimumPrice";
            
            public static string IsInInventory = @"IsInInventory";
            
            public static string Brand = @"Brand";
            
            public static string IsNonDiscountable = @"IsNonDiscountable";
            
            public static string Deleted = @"Deleted";
            
            public static string BalanceBefore = @"BalanceBefore";
            
            public static string BalanceAfter = @"BalanceAfter";
            
            public static string ExpiryDate = @"ExpiryDate";
            
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
