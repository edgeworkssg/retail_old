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
    /// Strongly-typed collection for the ViewInventoryHeader class.
    /// </summary>
    [Serializable]
    public partial class ViewInventoryHeaderCollection : ReadOnlyList<ViewInventoryHeader, ViewInventoryHeaderCollection>
    {        
        public ViewInventoryHeaderCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewInventoryHeader view.
    /// </summary>
    [Serializable]
    public partial class ViewInventoryHeader : ReadOnlyRecord<ViewInventoryHeader>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewInventoryHeader", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
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
                
                TableSchema.TableColumn colvarExchangeRate = new TableSchema.TableColumn(schema);
                colvarExchangeRate.ColumnName = "ExchangeRate";
                colvarExchangeRate.DataType = DbType.Double;
                colvarExchangeRate.MaxLength = 0;
                colvarExchangeRate.AutoIncrement = false;
                colvarExchangeRate.IsNullable = false;
                colvarExchangeRate.IsPrimaryKey = false;
                colvarExchangeRate.IsForeignKey = false;
                colvarExchangeRate.IsReadOnly = false;
                
                schema.Columns.Add(colvarExchangeRate);
                
                TableSchema.TableColumn colvarPurchaseOrderNo = new TableSchema.TableColumn(schema);
                colvarPurchaseOrderNo.ColumnName = "PurchaseOrderNo";
                colvarPurchaseOrderNo.DataType = DbType.AnsiString;
                colvarPurchaseOrderNo.MaxLength = 50;
                colvarPurchaseOrderNo.AutoIncrement = false;
                colvarPurchaseOrderNo.IsNullable = true;
                colvarPurchaseOrderNo.IsPrimaryKey = false;
                colvarPurchaseOrderNo.IsForeignKey = false;
                colvarPurchaseOrderNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarPurchaseOrderNo);
                
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
                
                TableSchema.TableColumn colvarDeliveryOrderNo = new TableSchema.TableColumn(schema);
                colvarDeliveryOrderNo.ColumnName = "DeliveryOrderNo";
                colvarDeliveryOrderNo.DataType = DbType.AnsiString;
                colvarDeliveryOrderNo.MaxLength = 50;
                colvarDeliveryOrderNo.AutoIncrement = false;
                colvarDeliveryOrderNo.IsNullable = true;
                colvarDeliveryOrderNo.IsPrimaryKey = false;
                colvarDeliveryOrderNo.IsForeignKey = false;
                colvarDeliveryOrderNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarDeliveryOrderNo);
                
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
                
                TableSchema.TableColumn colvarFreightCharge = new TableSchema.TableColumn(schema);
                colvarFreightCharge.ColumnName = "FreightCharge";
                colvarFreightCharge.DataType = DbType.Currency;
                colvarFreightCharge.MaxLength = 0;
                colvarFreightCharge.AutoIncrement = false;
                colvarFreightCharge.IsNullable = true;
                colvarFreightCharge.IsPrimaryKey = false;
                colvarFreightCharge.IsForeignKey = false;
                colvarFreightCharge.IsReadOnly = false;
                
                schema.Columns.Add(colvarFreightCharge);
                
                TableSchema.TableColumn colvarDeliveryCharge = new TableSchema.TableColumn(schema);
                colvarDeliveryCharge.ColumnName = "DeliveryCharge";
                colvarDeliveryCharge.DataType = DbType.Currency;
                colvarDeliveryCharge.MaxLength = 0;
                colvarDeliveryCharge.AutoIncrement = false;
                colvarDeliveryCharge.IsNullable = true;
                colvarDeliveryCharge.IsPrimaryKey = false;
                colvarDeliveryCharge.IsForeignKey = false;
                colvarDeliveryCharge.IsReadOnly = false;
                
                schema.Columns.Add(colvarDeliveryCharge);
                
                TableSchema.TableColumn colvarTax = new TableSchema.TableColumn(schema);
                colvarTax.ColumnName = "Tax";
                colvarTax.DataType = DbType.Decimal;
                colvarTax.MaxLength = 0;
                colvarTax.AutoIncrement = false;
                colvarTax.IsNullable = true;
                colvarTax.IsPrimaryKey = false;
                colvarTax.IsForeignKey = false;
                colvarTax.IsReadOnly = false;
                
                schema.Columns.Add(colvarTax);
                
                TableSchema.TableColumn colvarDiscount = new TableSchema.TableColumn(schema);
                colvarDiscount.ColumnName = "Discount";
                colvarDiscount.DataType = DbType.Decimal;
                colvarDiscount.MaxLength = 0;
                colvarDiscount.AutoIncrement = false;
                colvarDiscount.IsNullable = true;
                colvarDiscount.IsPrimaryKey = false;
                colvarDiscount.IsForeignKey = false;
                colvarDiscount.IsReadOnly = false;
                
                schema.Columns.Add(colvarDiscount);
                
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
                
                TableSchema.TableColumn colvarInventoryLocationID = new TableSchema.TableColumn(schema);
                colvarInventoryLocationID.ColumnName = "InventoryLocationID";
                colvarInventoryLocationID.DataType = DbType.Int32;
                colvarInventoryLocationID.MaxLength = 0;
                colvarInventoryLocationID.AutoIncrement = false;
                colvarInventoryLocationID.IsNullable = true;
                colvarInventoryLocationID.IsPrimaryKey = false;
                colvarInventoryLocationID.IsForeignKey = false;
                colvarInventoryLocationID.IsReadOnly = false;
                
                schema.Columns.Add(colvarInventoryLocationID);
                
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
                
                TableSchema.TableColumn colvarTmpSavedData = new TableSchema.TableColumn(schema);
                colvarTmpSavedData.ColumnName = "TmpSavedData";
                colvarTmpSavedData.DataType = DbType.Int32;
                colvarTmpSavedData.MaxLength = 0;
                colvarTmpSavedData.AutoIncrement = false;
                colvarTmpSavedData.IsNullable = true;
                colvarTmpSavedData.IsPrimaryKey = false;
                colvarTmpSavedData.IsForeignKey = false;
                colvarTmpSavedData.IsReadOnly = false;
                
                schema.Columns.Add(colvarTmpSavedData);
                
                TableSchema.TableColumn colvarCreatedOn = new TableSchema.TableColumn(schema);
                colvarCreatedOn.ColumnName = "CreatedOn";
                colvarCreatedOn.DataType = DbType.DateTime;
                colvarCreatedOn.MaxLength = 0;
                colvarCreatedOn.AutoIncrement = false;
                colvarCreatedOn.IsNullable = true;
                colvarCreatedOn.IsPrimaryKey = false;
                colvarCreatedOn.IsForeignKey = false;
                colvarCreatedOn.IsReadOnly = false;
                
                schema.Columns.Add(colvarCreatedOn);
                
                TableSchema.TableColumn colvarModifiedOn = new TableSchema.TableColumn(schema);
                colvarModifiedOn.ColumnName = "ModifiedOn";
                colvarModifiedOn.DataType = DbType.DateTime;
                colvarModifiedOn.MaxLength = 0;
                colvarModifiedOn.AutoIncrement = false;
                colvarModifiedOn.IsNullable = true;
                colvarModifiedOn.IsPrimaryKey = false;
                colvarModifiedOn.IsForeignKey = false;
                colvarModifiedOn.IsReadOnly = false;
                
                schema.Columns.Add(colvarModifiedOn);
                
                TableSchema.TableColumn colvarCreatedBy = new TableSchema.TableColumn(schema);
                colvarCreatedBy.ColumnName = "CreatedBy";
                colvarCreatedBy.DataType = DbType.AnsiString;
                colvarCreatedBy.MaxLength = 50;
                colvarCreatedBy.AutoIncrement = false;
                colvarCreatedBy.IsNullable = true;
                colvarCreatedBy.IsPrimaryKey = false;
                colvarCreatedBy.IsForeignKey = false;
                colvarCreatedBy.IsReadOnly = false;
                
                schema.Columns.Add(colvarCreatedBy);
                
                TableSchema.TableColumn colvarModifiedBy = new TableSchema.TableColumn(schema);
                colvarModifiedBy.ColumnName = "ModifiedBy";
                colvarModifiedBy.DataType = DbType.AnsiString;
                colvarModifiedBy.MaxLength = 50;
                colvarModifiedBy.AutoIncrement = false;
                colvarModifiedBy.IsNullable = true;
                colvarModifiedBy.IsPrimaryKey = false;
                colvarModifiedBy.IsForeignKey = false;
                colvarModifiedBy.IsReadOnly = false;
                
                schema.Columns.Add(colvarModifiedBy);
                
                TableSchema.TableColumn colvarUniqueID = new TableSchema.TableColumn(schema);
                colvarUniqueID.ColumnName = "UniqueID";
                colvarUniqueID.DataType = DbType.Guid;
                colvarUniqueID.MaxLength = 0;
                colvarUniqueID.AutoIncrement = false;
                colvarUniqueID.IsNullable = false;
                colvarUniqueID.IsPrimaryKey = false;
                colvarUniqueID.IsForeignKey = false;
                colvarUniqueID.IsReadOnly = false;
                
                schema.Columns.Add(colvarUniqueID);
                
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
                
                TableSchema.TableColumn colvarUserfld1 = new TableSchema.TableColumn(schema);
                colvarUserfld1.ColumnName = "userfld1";
                colvarUserfld1.DataType = DbType.AnsiString;
                colvarUserfld1.MaxLength = 50;
                colvarUserfld1.AutoIncrement = false;
                colvarUserfld1.IsNullable = true;
                colvarUserfld1.IsPrimaryKey = false;
                colvarUserfld1.IsForeignKey = false;
                colvarUserfld1.IsReadOnly = false;
                
                schema.Columns.Add(colvarUserfld1);
                
                TableSchema.TableColumn colvarUserfld2 = new TableSchema.TableColumn(schema);
                colvarUserfld2.ColumnName = "userfld2";
                colvarUserfld2.DataType = DbType.AnsiString;
                colvarUserfld2.MaxLength = 50;
                colvarUserfld2.AutoIncrement = false;
                colvarUserfld2.IsNullable = true;
                colvarUserfld2.IsPrimaryKey = false;
                colvarUserfld2.IsForeignKey = false;
                colvarUserfld2.IsReadOnly = false;
                
                schema.Columns.Add(colvarUserfld2);
                
                TableSchema.TableColumn colvarUserfld3 = new TableSchema.TableColumn(schema);
                colvarUserfld3.ColumnName = "userfld3";
                colvarUserfld3.DataType = DbType.AnsiString;
                colvarUserfld3.MaxLength = 50;
                colvarUserfld3.AutoIncrement = false;
                colvarUserfld3.IsNullable = true;
                colvarUserfld3.IsPrimaryKey = false;
                colvarUserfld3.IsForeignKey = false;
                colvarUserfld3.IsReadOnly = false;
                
                schema.Columns.Add(colvarUserfld3);
                
                TableSchema.TableColumn colvarUserfld4 = new TableSchema.TableColumn(schema);
                colvarUserfld4.ColumnName = "userfld4";
                colvarUserfld4.DataType = DbType.AnsiString;
                colvarUserfld4.MaxLength = 50;
                colvarUserfld4.AutoIncrement = false;
                colvarUserfld4.IsNullable = true;
                colvarUserfld4.IsPrimaryKey = false;
                colvarUserfld4.IsForeignKey = false;
                colvarUserfld4.IsReadOnly = false;
                
                schema.Columns.Add(colvarUserfld4);
                
                TableSchema.TableColumn colvarUserfld5 = new TableSchema.TableColumn(schema);
                colvarUserfld5.ColumnName = "userfld5";
                colvarUserfld5.DataType = DbType.AnsiString;
                colvarUserfld5.MaxLength = 50;
                colvarUserfld5.AutoIncrement = false;
                colvarUserfld5.IsNullable = true;
                colvarUserfld5.IsPrimaryKey = false;
                colvarUserfld5.IsForeignKey = false;
                colvarUserfld5.IsReadOnly = false;
                
                schema.Columns.Add(colvarUserfld5);
                
                TableSchema.TableColumn colvarUserfld6 = new TableSchema.TableColumn(schema);
                colvarUserfld6.ColumnName = "userfld6";
                colvarUserfld6.DataType = DbType.AnsiString;
                colvarUserfld6.MaxLength = 50;
                colvarUserfld6.AutoIncrement = false;
                colvarUserfld6.IsNullable = true;
                colvarUserfld6.IsPrimaryKey = false;
                colvarUserfld6.IsForeignKey = false;
                colvarUserfld6.IsReadOnly = false;
                
                schema.Columns.Add(colvarUserfld6);
                
                TableSchema.TableColumn colvarUserfld7 = new TableSchema.TableColumn(schema);
                colvarUserfld7.ColumnName = "userfld7";
                colvarUserfld7.DataType = DbType.AnsiString;
                colvarUserfld7.MaxLength = 50;
                colvarUserfld7.AutoIncrement = false;
                colvarUserfld7.IsNullable = true;
                colvarUserfld7.IsPrimaryKey = false;
                colvarUserfld7.IsForeignKey = false;
                colvarUserfld7.IsReadOnly = false;
                
                schema.Columns.Add(colvarUserfld7);
                
                TableSchema.TableColumn colvarUserfld8 = new TableSchema.TableColumn(schema);
                colvarUserfld8.ColumnName = "userfld8";
                colvarUserfld8.DataType = DbType.AnsiString;
                colvarUserfld8.MaxLength = 50;
                colvarUserfld8.AutoIncrement = false;
                colvarUserfld8.IsNullable = true;
                colvarUserfld8.IsPrimaryKey = false;
                colvarUserfld8.IsForeignKey = false;
                colvarUserfld8.IsReadOnly = false;
                
                schema.Columns.Add(colvarUserfld8);
                
                TableSchema.TableColumn colvarUserfld9 = new TableSchema.TableColumn(schema);
                colvarUserfld9.ColumnName = "userfld9";
                colvarUserfld9.DataType = DbType.AnsiString;
                colvarUserfld9.MaxLength = 50;
                colvarUserfld9.AutoIncrement = false;
                colvarUserfld9.IsNullable = true;
                colvarUserfld9.IsPrimaryKey = false;
                colvarUserfld9.IsForeignKey = false;
                colvarUserfld9.IsReadOnly = false;
                
                schema.Columns.Add(colvarUserfld9);
                
                TableSchema.TableColumn colvarUserfld10 = new TableSchema.TableColumn(schema);
                colvarUserfld10.ColumnName = "userfld10";
                colvarUserfld10.DataType = DbType.AnsiString;
                colvarUserfld10.MaxLength = 50;
                colvarUserfld10.AutoIncrement = false;
                colvarUserfld10.IsNullable = true;
                colvarUserfld10.IsPrimaryKey = false;
                colvarUserfld10.IsForeignKey = false;
                colvarUserfld10.IsReadOnly = false;
                
                schema.Columns.Add(colvarUserfld10);
                
                TableSchema.TableColumn colvarUserflag1 = new TableSchema.TableColumn(schema);
                colvarUserflag1.ColumnName = "userflag1";
                colvarUserflag1.DataType = DbType.Boolean;
                colvarUserflag1.MaxLength = 0;
                colvarUserflag1.AutoIncrement = false;
                colvarUserflag1.IsNullable = true;
                colvarUserflag1.IsPrimaryKey = false;
                colvarUserflag1.IsForeignKey = false;
                colvarUserflag1.IsReadOnly = false;
                
                schema.Columns.Add(colvarUserflag1);
                
                TableSchema.TableColumn colvarUserflag2 = new TableSchema.TableColumn(schema);
                colvarUserflag2.ColumnName = "userflag2";
                colvarUserflag2.DataType = DbType.Boolean;
                colvarUserflag2.MaxLength = 0;
                colvarUserflag2.AutoIncrement = false;
                colvarUserflag2.IsNullable = true;
                colvarUserflag2.IsPrimaryKey = false;
                colvarUserflag2.IsForeignKey = false;
                colvarUserflag2.IsReadOnly = false;
                
                schema.Columns.Add(colvarUserflag2);
                
                TableSchema.TableColumn colvarUserflag3 = new TableSchema.TableColumn(schema);
                colvarUserflag3.ColumnName = "userflag3";
                colvarUserflag3.DataType = DbType.Boolean;
                colvarUserflag3.MaxLength = 0;
                colvarUserflag3.AutoIncrement = false;
                colvarUserflag3.IsNullable = true;
                colvarUserflag3.IsPrimaryKey = false;
                colvarUserflag3.IsForeignKey = false;
                colvarUserflag3.IsReadOnly = false;
                
                schema.Columns.Add(colvarUserflag3);
                
                TableSchema.TableColumn colvarUserflag4 = new TableSchema.TableColumn(schema);
                colvarUserflag4.ColumnName = "userflag4";
                colvarUserflag4.DataType = DbType.Boolean;
                colvarUserflag4.MaxLength = 0;
                colvarUserflag4.AutoIncrement = false;
                colvarUserflag4.IsNullable = true;
                colvarUserflag4.IsPrimaryKey = false;
                colvarUserflag4.IsForeignKey = false;
                colvarUserflag4.IsReadOnly = false;
                
                schema.Columns.Add(colvarUserflag4);
                
                TableSchema.TableColumn colvarUserflag5 = new TableSchema.TableColumn(schema);
                colvarUserflag5.ColumnName = "userflag5";
                colvarUserflag5.DataType = DbType.Boolean;
                colvarUserflag5.MaxLength = 0;
                colvarUserflag5.AutoIncrement = false;
                colvarUserflag5.IsNullable = true;
                colvarUserflag5.IsPrimaryKey = false;
                colvarUserflag5.IsForeignKey = false;
                colvarUserflag5.IsReadOnly = false;
                
                schema.Columns.Add(colvarUserflag5);
                
                TableSchema.TableColumn colvarUserfloat1 = new TableSchema.TableColumn(schema);
                colvarUserfloat1.ColumnName = "userfloat1";
                colvarUserfloat1.DataType = DbType.Currency;
                colvarUserfloat1.MaxLength = 0;
                colvarUserfloat1.AutoIncrement = false;
                colvarUserfloat1.IsNullable = true;
                colvarUserfloat1.IsPrimaryKey = false;
                colvarUserfloat1.IsForeignKey = false;
                colvarUserfloat1.IsReadOnly = false;
                
                schema.Columns.Add(colvarUserfloat1);
                
                TableSchema.TableColumn colvarUserfloat2 = new TableSchema.TableColumn(schema);
                colvarUserfloat2.ColumnName = "userfloat2";
                colvarUserfloat2.DataType = DbType.Currency;
                colvarUserfloat2.MaxLength = 0;
                colvarUserfloat2.AutoIncrement = false;
                colvarUserfloat2.IsNullable = true;
                colvarUserfloat2.IsPrimaryKey = false;
                colvarUserfloat2.IsForeignKey = false;
                colvarUserfloat2.IsReadOnly = false;
                
                schema.Columns.Add(colvarUserfloat2);
                
                TableSchema.TableColumn colvarUserfloat3 = new TableSchema.TableColumn(schema);
                colvarUserfloat3.ColumnName = "userfloat3";
                colvarUserfloat3.DataType = DbType.Currency;
                colvarUserfloat3.MaxLength = 0;
                colvarUserfloat3.AutoIncrement = false;
                colvarUserfloat3.IsNullable = true;
                colvarUserfloat3.IsPrimaryKey = false;
                colvarUserfloat3.IsForeignKey = false;
                colvarUserfloat3.IsReadOnly = false;
                
                schema.Columns.Add(colvarUserfloat3);
                
                TableSchema.TableColumn colvarUserfloat4 = new TableSchema.TableColumn(schema);
                colvarUserfloat4.ColumnName = "userfloat4";
                colvarUserfloat4.DataType = DbType.Currency;
                colvarUserfloat4.MaxLength = 0;
                colvarUserfloat4.AutoIncrement = false;
                colvarUserfloat4.IsNullable = true;
                colvarUserfloat4.IsPrimaryKey = false;
                colvarUserfloat4.IsForeignKey = false;
                colvarUserfloat4.IsReadOnly = false;
                
                schema.Columns.Add(colvarUserfloat4);
                
                TableSchema.TableColumn colvarUserfloat5 = new TableSchema.TableColumn(schema);
                colvarUserfloat5.ColumnName = "userfloat5";
                colvarUserfloat5.DataType = DbType.Currency;
                colvarUserfloat5.MaxLength = 0;
                colvarUserfloat5.AutoIncrement = false;
                colvarUserfloat5.IsNullable = true;
                colvarUserfloat5.IsPrimaryKey = false;
                colvarUserfloat5.IsForeignKey = false;
                colvarUserfloat5.IsReadOnly = false;
                
                schema.Columns.Add(colvarUserfloat5);
                
                TableSchema.TableColumn colvarUserint1 = new TableSchema.TableColumn(schema);
                colvarUserint1.ColumnName = "userint1";
                colvarUserint1.DataType = DbType.Int32;
                colvarUserint1.MaxLength = 0;
                colvarUserint1.AutoIncrement = false;
                colvarUserint1.IsNullable = true;
                colvarUserint1.IsPrimaryKey = false;
                colvarUserint1.IsForeignKey = false;
                colvarUserint1.IsReadOnly = false;
                
                schema.Columns.Add(colvarUserint1);
                
                TableSchema.TableColumn colvarUserint2 = new TableSchema.TableColumn(schema);
                colvarUserint2.ColumnName = "userint2";
                colvarUserint2.DataType = DbType.Int32;
                colvarUserint2.MaxLength = 0;
                colvarUserint2.AutoIncrement = false;
                colvarUserint2.IsNullable = true;
                colvarUserint2.IsPrimaryKey = false;
                colvarUserint2.IsForeignKey = false;
                colvarUserint2.IsReadOnly = false;
                
                schema.Columns.Add(colvarUserint2);
                
                TableSchema.TableColumn colvarUserint3 = new TableSchema.TableColumn(schema);
                colvarUserint3.ColumnName = "userint3";
                colvarUserint3.DataType = DbType.Int32;
                colvarUserint3.MaxLength = 0;
                colvarUserint3.AutoIncrement = false;
                colvarUserint3.IsNullable = true;
                colvarUserint3.IsPrimaryKey = false;
                colvarUserint3.IsForeignKey = false;
                colvarUserint3.IsReadOnly = false;
                
                schema.Columns.Add(colvarUserint3);
                
                TableSchema.TableColumn colvarUserint4 = new TableSchema.TableColumn(schema);
                colvarUserint4.ColumnName = "userint4";
                colvarUserint4.DataType = DbType.Int32;
                colvarUserint4.MaxLength = 0;
                colvarUserint4.AutoIncrement = false;
                colvarUserint4.IsNullable = true;
                colvarUserint4.IsPrimaryKey = false;
                colvarUserint4.IsForeignKey = false;
                colvarUserint4.IsReadOnly = false;
                
                schema.Columns.Add(colvarUserint4);
                
                TableSchema.TableColumn colvarUserint5 = new TableSchema.TableColumn(schema);
                colvarUserint5.ColumnName = "userint5";
                colvarUserint5.DataType = DbType.Int32;
                colvarUserint5.MaxLength = 0;
                colvarUserint5.AutoIncrement = false;
                colvarUserint5.IsNullable = true;
                colvarUserint5.IsPrimaryKey = false;
                colvarUserint5.IsForeignKey = false;
                colvarUserint5.IsReadOnly = false;
                
                schema.Columns.Add(colvarUserint5);
                
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
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewInventoryHeader",schema);
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
	    public ViewInventoryHeader()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewInventoryHeader(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewInventoryHeader(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewInventoryHeader(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
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
	      
        [XmlAttribute("ExchangeRate")]
        [Bindable(true)]
        public double ExchangeRate 
	    {
		    get
		    {
			    return GetColumnValue<double>("ExchangeRate");
		    }
            set 
		    {
			    SetColumnValue("ExchangeRate", value);
            }
        }
	      
        [XmlAttribute("PurchaseOrderNo")]
        [Bindable(true)]
        public string PurchaseOrderNo 
	    {
		    get
		    {
			    return GetColumnValue<string>("PurchaseOrderNo");
		    }
            set 
		    {
			    SetColumnValue("PurchaseOrderNo", value);
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
	      
        [XmlAttribute("DeliveryOrderNo")]
        [Bindable(true)]
        public string DeliveryOrderNo 
	    {
		    get
		    {
			    return GetColumnValue<string>("DeliveryOrderNo");
		    }
            set 
		    {
			    SetColumnValue("DeliveryOrderNo", value);
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
	      
        [XmlAttribute("FreightCharge")]
        [Bindable(true)]
        public decimal? FreightCharge 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("FreightCharge");
		    }
            set 
		    {
			    SetColumnValue("FreightCharge", value);
            }
        }
	      
        [XmlAttribute("DeliveryCharge")]
        [Bindable(true)]
        public decimal? DeliveryCharge 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("DeliveryCharge");
		    }
            set 
		    {
			    SetColumnValue("DeliveryCharge", value);
            }
        }
	      
        [XmlAttribute("Tax")]
        [Bindable(true)]
        public decimal? Tax 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Tax");
		    }
            set 
		    {
			    SetColumnValue("Tax", value);
            }
        }
	      
        [XmlAttribute("Discount")]
        [Bindable(true)]
        public decimal? Discount 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("Discount");
		    }
            set 
		    {
			    SetColumnValue("Discount", value);
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
	      
        [XmlAttribute("InventoryLocationID")]
        [Bindable(true)]
        public int? InventoryLocationID 
	    {
		    get
		    {
			    return GetColumnValue<int?>("InventoryLocationID");
		    }
            set 
		    {
			    SetColumnValue("InventoryLocationID", value);
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
	      
        [XmlAttribute("TmpSavedData")]
        [Bindable(true)]
        public int? TmpSavedData 
	    {
		    get
		    {
			    return GetColumnValue<int?>("TmpSavedData");
		    }
            set 
		    {
			    SetColumnValue("TmpSavedData", value);
            }
        }
	      
        [XmlAttribute("CreatedOn")]
        [Bindable(true)]
        public DateTime? CreatedOn 
	    {
		    get
		    {
			    return GetColumnValue<DateTime?>("CreatedOn");
		    }
            set 
		    {
			    SetColumnValue("CreatedOn", value);
            }
        }
	      
        [XmlAttribute("ModifiedOn")]
        [Bindable(true)]
        public DateTime? ModifiedOn 
	    {
		    get
		    {
			    return GetColumnValue<DateTime?>("ModifiedOn");
		    }
            set 
		    {
			    SetColumnValue("ModifiedOn", value);
            }
        }
	      
        [XmlAttribute("CreatedBy")]
        [Bindable(true)]
        public string CreatedBy 
	    {
		    get
		    {
			    return GetColumnValue<string>("CreatedBy");
		    }
            set 
		    {
			    SetColumnValue("CreatedBy", value);
            }
        }
	      
        [XmlAttribute("ModifiedBy")]
        [Bindable(true)]
        public string ModifiedBy 
	    {
		    get
		    {
			    return GetColumnValue<string>("ModifiedBy");
		    }
            set 
		    {
			    SetColumnValue("ModifiedBy", value);
            }
        }
	      
        [XmlAttribute("UniqueID")]
        [Bindable(true)]
        public Guid UniqueID 
	    {
		    get
		    {
			    return GetColumnValue<Guid>("UniqueID");
		    }
            set 
		    {
			    SetColumnValue("UniqueID", value);
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
	      
        [XmlAttribute("Userfld1")]
        [Bindable(true)]
        public string Userfld1 
	    {
		    get
		    {
			    return GetColumnValue<string>("userfld1");
		    }
            set 
		    {
			    SetColumnValue("userfld1", value);
            }
        }
	      
        [XmlAttribute("Userfld2")]
        [Bindable(true)]
        public string Userfld2 
	    {
		    get
		    {
			    return GetColumnValue<string>("userfld2");
		    }
            set 
		    {
			    SetColumnValue("userfld2", value);
            }
        }
	      
        [XmlAttribute("Userfld3")]
        [Bindable(true)]
        public string Userfld3 
	    {
		    get
		    {
			    return GetColumnValue<string>("userfld3");
		    }
            set 
		    {
			    SetColumnValue("userfld3", value);
            }
        }
	      
        [XmlAttribute("Userfld4")]
        [Bindable(true)]
        public string Userfld4 
	    {
		    get
		    {
			    return GetColumnValue<string>("userfld4");
		    }
            set 
		    {
			    SetColumnValue("userfld4", value);
            }
        }
	      
        [XmlAttribute("Userfld5")]
        [Bindable(true)]
        public string Userfld5 
	    {
		    get
		    {
			    return GetColumnValue<string>("userfld5");
		    }
            set 
		    {
			    SetColumnValue("userfld5", value);
            }
        }
	      
        [XmlAttribute("Userfld6")]
        [Bindable(true)]
        public string Userfld6 
	    {
		    get
		    {
			    return GetColumnValue<string>("userfld6");
		    }
            set 
		    {
			    SetColumnValue("userfld6", value);
            }
        }
	      
        [XmlAttribute("Userfld7")]
        [Bindable(true)]
        public string Userfld7 
	    {
		    get
		    {
			    return GetColumnValue<string>("userfld7");
		    }
            set 
		    {
			    SetColumnValue("userfld7", value);
            }
        }
	      
        [XmlAttribute("Userfld8")]
        [Bindable(true)]
        public string Userfld8 
	    {
		    get
		    {
			    return GetColumnValue<string>("userfld8");
		    }
            set 
		    {
			    SetColumnValue("userfld8", value);
            }
        }
	      
        [XmlAttribute("Userfld9")]
        [Bindable(true)]
        public string Userfld9 
	    {
		    get
		    {
			    return GetColumnValue<string>("userfld9");
		    }
            set 
		    {
			    SetColumnValue("userfld9", value);
            }
        }
	      
        [XmlAttribute("Userfld10")]
        [Bindable(true)]
        public string Userfld10 
	    {
		    get
		    {
			    return GetColumnValue<string>("userfld10");
		    }
            set 
		    {
			    SetColumnValue("userfld10", value);
            }
        }
	      
        [XmlAttribute("Userflag1")]
        [Bindable(true)]
        public bool? Userflag1 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("userflag1");
		    }
            set 
		    {
			    SetColumnValue("userflag1", value);
            }
        }
	      
        [XmlAttribute("Userflag2")]
        [Bindable(true)]
        public bool? Userflag2 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("userflag2");
		    }
            set 
		    {
			    SetColumnValue("userflag2", value);
            }
        }
	      
        [XmlAttribute("Userflag3")]
        [Bindable(true)]
        public bool? Userflag3 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("userflag3");
		    }
            set 
		    {
			    SetColumnValue("userflag3", value);
            }
        }
	      
        [XmlAttribute("Userflag4")]
        [Bindable(true)]
        public bool? Userflag4 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("userflag4");
		    }
            set 
		    {
			    SetColumnValue("userflag4", value);
            }
        }
	      
        [XmlAttribute("Userflag5")]
        [Bindable(true)]
        public bool? Userflag5 
	    {
		    get
		    {
			    return GetColumnValue<bool?>("userflag5");
		    }
            set 
		    {
			    SetColumnValue("userflag5", value);
            }
        }
	      
        [XmlAttribute("Userfloat1")]
        [Bindable(true)]
        public decimal? Userfloat1 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("userfloat1");
		    }
            set 
		    {
			    SetColumnValue("userfloat1", value);
            }
        }
	      
        [XmlAttribute("Userfloat2")]
        [Bindable(true)]
        public decimal? Userfloat2 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("userfloat2");
		    }
            set 
		    {
			    SetColumnValue("userfloat2", value);
            }
        }
	      
        [XmlAttribute("Userfloat3")]
        [Bindable(true)]
        public decimal? Userfloat3 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("userfloat3");
		    }
            set 
		    {
			    SetColumnValue("userfloat3", value);
            }
        }
	      
        [XmlAttribute("Userfloat4")]
        [Bindable(true)]
        public decimal? Userfloat4 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("userfloat4");
		    }
            set 
		    {
			    SetColumnValue("userfloat4", value);
            }
        }
	      
        [XmlAttribute("Userfloat5")]
        [Bindable(true)]
        public decimal? Userfloat5 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("userfloat5");
		    }
            set 
		    {
			    SetColumnValue("userfloat5", value);
            }
        }
	      
        [XmlAttribute("Userint1")]
        [Bindable(true)]
        public int? Userint1 
	    {
		    get
		    {
			    return GetColumnValue<int?>("userint1");
		    }
            set 
		    {
			    SetColumnValue("userint1", value);
            }
        }
	      
        [XmlAttribute("Userint2")]
        [Bindable(true)]
        public int? Userint2 
	    {
		    get
		    {
			    return GetColumnValue<int?>("userint2");
		    }
            set 
		    {
			    SetColumnValue("userint2", value);
            }
        }
	      
        [XmlAttribute("Userint3")]
        [Bindable(true)]
        public int? Userint3 
	    {
		    get
		    {
			    return GetColumnValue<int?>("userint3");
		    }
            set 
		    {
			    SetColumnValue("userint3", value);
            }
        }
	      
        [XmlAttribute("Userint4")]
        [Bindable(true)]
        public int? Userint4 
	    {
		    get
		    {
			    return GetColumnValue<int?>("userint4");
		    }
            set 
		    {
			    SetColumnValue("userint4", value);
            }
        }
	      
        [XmlAttribute("Userint5")]
        [Bindable(true)]
        public int? Userint5 
	    {
		    get
		    {
			    return GetColumnValue<int?>("userint5");
		    }
            set 
		    {
			    SetColumnValue("userint5", value);
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
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string InventoryHdrRefNo = @"InventoryHdrRefNo";
            
            public static string InventoryDate = @"InventoryDate";
            
            public static string UserName = @"UserName";
            
            public static string MovementType = @"MovementType";
            
            public static string StockOutReasonID = @"StockOutReasonID";
            
            public static string ExchangeRate = @"ExchangeRate";
            
            public static string PurchaseOrderNo = @"PurchaseOrderNo";
            
            public static string InvoiceNo = @"InvoiceNo";
            
            public static string DeliveryOrderNo = @"DeliveryOrderNo";
            
            public static string Supplier = @"Supplier";
            
            public static string FreightCharge = @"FreightCharge";
            
            public static string DeliveryCharge = @"DeliveryCharge";
            
            public static string Tax = @"Tax";
            
            public static string Discount = @"Discount";
            
            public static string Remark = @"Remark";
            
            public static string InventoryLocationID = @"InventoryLocationID";
            
            public static string DepartmentID = @"DepartmentID";
            
            public static string TmpSavedData = @"TmpSavedData";
            
            public static string CreatedOn = @"CreatedOn";
            
            public static string ModifiedOn = @"ModifiedOn";
            
            public static string CreatedBy = @"CreatedBy";
            
            public static string ModifiedBy = @"ModifiedBy";
            
            public static string UniqueID = @"UniqueID";
            
            public static string Deleted = @"Deleted";
            
            public static string Userfld1 = @"userfld1";
            
            public static string Userfld2 = @"userfld2";
            
            public static string Userfld3 = @"userfld3";
            
            public static string Userfld4 = @"userfld4";
            
            public static string Userfld5 = @"userfld5";
            
            public static string Userfld6 = @"userfld6";
            
            public static string Userfld7 = @"userfld7";
            
            public static string Userfld8 = @"userfld8";
            
            public static string Userfld9 = @"userfld9";
            
            public static string Userfld10 = @"userfld10";
            
            public static string Userflag1 = @"userflag1";
            
            public static string Userflag2 = @"userflag2";
            
            public static string Userflag3 = @"userflag3";
            
            public static string Userflag4 = @"userflag4";
            
            public static string Userflag5 = @"userflag5";
            
            public static string Userfloat1 = @"userfloat1";
            
            public static string Userfloat2 = @"userfloat2";
            
            public static string Userfloat3 = @"userfloat3";
            
            public static string Userfloat4 = @"userfloat4";
            
            public static string Userfloat5 = @"userfloat5";
            
            public static string Userint1 = @"userint1";
            
            public static string Userint2 = @"userint2";
            
            public static string Userint3 = @"userint3";
            
            public static string Userint4 = @"userint4";
            
            public static string Userint5 = @"userint5";
            
            public static string InventoryLocationName = @"InventoryLocationName";
            
            public static string ReasonName = @"ReasonName";
            
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
