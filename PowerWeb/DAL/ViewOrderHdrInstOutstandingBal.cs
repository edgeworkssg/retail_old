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
    /// Strongly-typed collection for the ViewOrderHdrInstOutstandingBal class.
    /// </summary>
    [Serializable]
    public partial class ViewOrderHdrInstOutstandingBalCollection : ReadOnlyList<ViewOrderHdrInstOutstandingBal, ViewOrderHdrInstOutstandingBalCollection>
    {        
        public ViewOrderHdrInstOutstandingBalCollection() {}
    }
    /// <summary>
    /// This is  Read-only wrapper class for the ViewOrderHdrInstOutstandingBal view.
    /// </summary>
    [Serializable]
    public partial class ViewOrderHdrInstOutstandingBal : ReadOnlyRecord<ViewOrderHdrInstOutstandingBal>, IReadOnlyRecord
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
                TableSchema.Table schema = new TableSchema.Table("ViewOrderHdrInstOutstandingBal", TableType.View, DataService.GetInstance("PowerPOS"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns
                
                TableSchema.TableColumn colvarOrderHdrID = new TableSchema.TableColumn(schema);
                colvarOrderHdrID.ColumnName = "OrderHdrID";
                colvarOrderHdrID.DataType = DbType.AnsiString;
                colvarOrderHdrID.MaxLength = 14;
                colvarOrderHdrID.AutoIncrement = false;
                colvarOrderHdrID.IsNullable = false;
                colvarOrderHdrID.IsPrimaryKey = false;
                colvarOrderHdrID.IsForeignKey = false;
                colvarOrderHdrID.IsReadOnly = false;
                
                schema.Columns.Add(colvarOrderHdrID);
                
                TableSchema.TableColumn colvarOrderRefNo = new TableSchema.TableColumn(schema);
                colvarOrderRefNo.ColumnName = "OrderRefNo";
                colvarOrderRefNo.DataType = DbType.AnsiString;
                colvarOrderRefNo.MaxLength = 50;
                colvarOrderRefNo.AutoIncrement = false;
                colvarOrderRefNo.IsNullable = false;
                colvarOrderRefNo.IsPrimaryKey = false;
                colvarOrderRefNo.IsForeignKey = false;
                colvarOrderRefNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarOrderRefNo);
                
                TableSchema.TableColumn colvarDiscount = new TableSchema.TableColumn(schema);
                colvarDiscount.ColumnName = "Discount";
                colvarDiscount.DataType = DbType.Decimal;
                colvarDiscount.MaxLength = 0;
                colvarDiscount.AutoIncrement = false;
                colvarDiscount.IsNullable = false;
                colvarDiscount.IsPrimaryKey = false;
                colvarDiscount.IsForeignKey = false;
                colvarDiscount.IsReadOnly = false;
                
                schema.Columns.Add(colvarDiscount);
                
                TableSchema.TableColumn colvarInventoryHdrRefNo = new TableSchema.TableColumn(schema);
                colvarInventoryHdrRefNo.ColumnName = "InventoryHdrRefNo";
                colvarInventoryHdrRefNo.DataType = DbType.AnsiString;
                colvarInventoryHdrRefNo.MaxLength = 50;
                colvarInventoryHdrRefNo.AutoIncrement = false;
                colvarInventoryHdrRefNo.IsNullable = true;
                colvarInventoryHdrRefNo.IsPrimaryKey = false;
                colvarInventoryHdrRefNo.IsForeignKey = false;
                colvarInventoryHdrRefNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarInventoryHdrRefNo);
                
                TableSchema.TableColumn colvarCashierID = new TableSchema.TableColumn(schema);
                colvarCashierID.ColumnName = "CashierID";
                colvarCashierID.DataType = DbType.AnsiString;
                colvarCashierID.MaxLength = 50;
                colvarCashierID.AutoIncrement = false;
                colvarCashierID.IsNullable = false;
                colvarCashierID.IsPrimaryKey = false;
                colvarCashierID.IsForeignKey = false;
                colvarCashierID.IsReadOnly = false;
                
                schema.Columns.Add(colvarCashierID);
                
                TableSchema.TableColumn colvarPointOfSaleID = new TableSchema.TableColumn(schema);
                colvarPointOfSaleID.ColumnName = "PointOfSaleID";
                colvarPointOfSaleID.DataType = DbType.Int32;
                colvarPointOfSaleID.MaxLength = 0;
                colvarPointOfSaleID.AutoIncrement = false;
                colvarPointOfSaleID.IsNullable = false;
                colvarPointOfSaleID.IsPrimaryKey = false;
                colvarPointOfSaleID.IsForeignKey = false;
                colvarPointOfSaleID.IsReadOnly = false;
                
                schema.Columns.Add(colvarPointOfSaleID);
                
                TableSchema.TableColumn colvarOrderDate = new TableSchema.TableColumn(schema);
                colvarOrderDate.ColumnName = "OrderDate";
                colvarOrderDate.DataType = DbType.DateTime;
                colvarOrderDate.MaxLength = 0;
                colvarOrderDate.AutoIncrement = false;
                colvarOrderDate.IsNullable = false;
                colvarOrderDate.IsPrimaryKey = false;
                colvarOrderDate.IsForeignKey = false;
                colvarOrderDate.IsReadOnly = false;
                
                schema.Columns.Add(colvarOrderDate);
                
                TableSchema.TableColumn colvarGrossAmount = new TableSchema.TableColumn(schema);
                colvarGrossAmount.ColumnName = "GrossAmount";
                colvarGrossAmount.DataType = DbType.Currency;
                colvarGrossAmount.MaxLength = 0;
                colvarGrossAmount.AutoIncrement = false;
                colvarGrossAmount.IsNullable = false;
                colvarGrossAmount.IsPrimaryKey = false;
                colvarGrossAmount.IsForeignKey = false;
                colvarGrossAmount.IsReadOnly = false;
                
                schema.Columns.Add(colvarGrossAmount);
                
                TableSchema.TableColumn colvarNettAmount = new TableSchema.TableColumn(schema);
                colvarNettAmount.ColumnName = "NettAmount";
                colvarNettAmount.DataType = DbType.Currency;
                colvarNettAmount.MaxLength = 0;
                colvarNettAmount.AutoIncrement = false;
                colvarNettAmount.IsNullable = false;
                colvarNettAmount.IsPrimaryKey = false;
                colvarNettAmount.IsForeignKey = false;
                colvarNettAmount.IsReadOnly = false;
                
                schema.Columns.Add(colvarNettAmount);
                
                TableSchema.TableColumn colvarDiscountAmount = new TableSchema.TableColumn(schema);
                colvarDiscountAmount.ColumnName = "DiscountAmount";
                colvarDiscountAmount.DataType = DbType.Currency;
                colvarDiscountAmount.MaxLength = 0;
                colvarDiscountAmount.AutoIncrement = false;
                colvarDiscountAmount.IsNullable = false;
                colvarDiscountAmount.IsPrimaryKey = false;
                colvarDiscountAmount.IsForeignKey = false;
                colvarDiscountAmount.IsReadOnly = false;
                
                schema.Columns.Add(colvarDiscountAmount);
                
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
                
                TableSchema.TableColumn colvarIsVoided = new TableSchema.TableColumn(schema);
                colvarIsVoided.ColumnName = "IsVoided";
                colvarIsVoided.DataType = DbType.Boolean;
                colvarIsVoided.MaxLength = 0;
                colvarIsVoided.AutoIncrement = false;
                colvarIsVoided.IsNullable = false;
                colvarIsVoided.IsPrimaryKey = false;
                colvarIsVoided.IsForeignKey = false;
                colvarIsVoided.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsVoided);
                
                TableSchema.TableColumn colvarMembershipNo = new TableSchema.TableColumn(schema);
                colvarMembershipNo.ColumnName = "MembershipNo";
                colvarMembershipNo.DataType = DbType.AnsiString;
                colvarMembershipNo.MaxLength = 50;
                colvarMembershipNo.AutoIncrement = false;
                colvarMembershipNo.IsNullable = true;
                colvarMembershipNo.IsPrimaryKey = false;
                colvarMembershipNo.IsForeignKey = false;
                colvarMembershipNo.IsReadOnly = false;
                
                schema.Columns.Add(colvarMembershipNo);
                
                TableSchema.TableColumn colvarRemark = new TableSchema.TableColumn(schema);
                colvarRemark.ColumnName = "Remark";
                colvarRemark.DataType = DbType.String;
                colvarRemark.MaxLength = -1;
                colvarRemark.AutoIncrement = false;
                colvarRemark.IsNullable = true;
                colvarRemark.IsPrimaryKey = false;
                colvarRemark.IsForeignKey = false;
                colvarRemark.IsReadOnly = false;
                
                schema.Columns.Add(colvarRemark);
                
                TableSchema.TableColumn colvarCreatedOn = new TableSchema.TableColumn(schema);
                colvarCreatedOn.ColumnName = "CreatedOn";
                colvarCreatedOn.DataType = DbType.DateTime;
                colvarCreatedOn.MaxLength = 0;
                colvarCreatedOn.AutoIncrement = false;
                colvarCreatedOn.IsNullable = false;
                colvarCreatedOn.IsPrimaryKey = false;
                colvarCreatedOn.IsForeignKey = false;
                colvarCreatedOn.IsReadOnly = false;
                
                schema.Columns.Add(colvarCreatedOn);
                
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
                
                TableSchema.TableColumn colvarModifiedOn = new TableSchema.TableColumn(schema);
                colvarModifiedOn.ColumnName = "ModifiedOn";
                colvarModifiedOn.DataType = DbType.DateTime;
                colvarModifiedOn.MaxLength = 0;
                colvarModifiedOn.AutoIncrement = false;
                colvarModifiedOn.IsNullable = false;
                colvarModifiedOn.IsPrimaryKey = false;
                colvarModifiedOn.IsForeignKey = false;
                colvarModifiedOn.IsReadOnly = false;
                
                schema.Columns.Add(colvarModifiedOn);
                
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
                
                TableSchema.TableColumn colvarPromoCodeID = new TableSchema.TableColumn(schema);
                colvarPromoCodeID.ColumnName = "PromoCodeID";
                colvarPromoCodeID.DataType = DbType.Int32;
                colvarPromoCodeID.MaxLength = 0;
                colvarPromoCodeID.AutoIncrement = false;
                colvarPromoCodeID.IsNullable = true;
                colvarPromoCodeID.IsPrimaryKey = false;
                colvarPromoCodeID.IsForeignKey = false;
                colvarPromoCodeID.IsReadOnly = false;
                
                schema.Columns.Add(colvarPromoCodeID);
                
                TableSchema.TableColumn colvarGSTAmount = new TableSchema.TableColumn(schema);
                colvarGSTAmount.ColumnName = "GSTAmount";
                colvarGSTAmount.DataType = DbType.Currency;
                colvarGSTAmount.MaxLength = 0;
                colvarGSTAmount.AutoIncrement = false;
                colvarGSTAmount.IsNullable = true;
                colvarGSTAmount.IsPrimaryKey = false;
                colvarGSTAmount.IsForeignKey = false;
                colvarGSTAmount.IsReadOnly = false;
                
                schema.Columns.Add(colvarGSTAmount);
                
                TableSchema.TableColumn colvarIsPointAllocated = new TableSchema.TableColumn(schema);
                colvarIsPointAllocated.ColumnName = "IsPointAllocated";
                colvarIsPointAllocated.DataType = DbType.Boolean;
                colvarIsPointAllocated.MaxLength = 0;
                colvarIsPointAllocated.AutoIncrement = false;
                colvarIsPointAllocated.IsNullable = false;
                colvarIsPointAllocated.IsPrimaryKey = false;
                colvarIsPointAllocated.IsForeignKey = false;
                colvarIsPointAllocated.IsReadOnly = false;
                
                schema.Columns.Add(colvarIsPointAllocated);
                
                TableSchema.TableColumn colvarInstOutstandingBal = new TableSchema.TableColumn(schema);
                colvarInstOutstandingBal.ColumnName = "InstOutstandingBal";
                colvarInstOutstandingBal.DataType = DbType.Decimal;
                colvarInstOutstandingBal.MaxLength = 0;
                colvarInstOutstandingBal.AutoIncrement = false;
                colvarInstOutstandingBal.IsNullable = false;
                colvarInstOutstandingBal.IsPrimaryKey = false;
                colvarInstOutstandingBal.IsForeignKey = false;
                colvarInstOutstandingBal.IsReadOnly = false;
                
                schema.Columns.Add(colvarInstOutstandingBal);
                
                
                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["PowerPOS"].AddSchema("ViewOrderHdrInstOutstandingBal",schema);
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
	    public ViewOrderHdrInstOutstandingBal()
	    {
            SetSQLProps();
            SetDefaults();
            MarkNew();
        }
        public ViewOrderHdrInstOutstandingBal(bool useDatabaseDefaults)
	    {
		    SetSQLProps();
		    if(useDatabaseDefaults)
		    {
				ForceDefaults();
			}
			MarkNew();
	    }
	    
	    public ViewOrderHdrInstOutstandingBal(object keyID)
	    {
		    SetSQLProps();
		    LoadByKey(keyID);
	    }
    	 
	    public ViewOrderHdrInstOutstandingBal(string columnName, object columnValue)
        {
            SetSQLProps();
            LoadByParam(columnName,columnValue);
        }
        
	    #endregion
	    
	    #region Props
	    
          
        [XmlAttribute("OrderHdrID")]
        [Bindable(true)]
        public string OrderHdrID 
	    {
		    get
		    {
			    return GetColumnValue<string>("OrderHdrID");
		    }
            set 
		    {
			    SetColumnValue("OrderHdrID", value);
            }
        }
	      
        [XmlAttribute("OrderRefNo")]
        [Bindable(true)]
        public string OrderRefNo 
	    {
		    get
		    {
			    return GetColumnValue<string>("OrderRefNo");
		    }
            set 
		    {
			    SetColumnValue("OrderRefNo", value);
            }
        }
	      
        [XmlAttribute("Discount")]
        [Bindable(true)]
        public decimal Discount 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("Discount");
		    }
            set 
		    {
			    SetColumnValue("Discount", value);
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
	      
        [XmlAttribute("CashierID")]
        [Bindable(true)]
        public string CashierID 
	    {
		    get
		    {
			    return GetColumnValue<string>("CashierID");
		    }
            set 
		    {
			    SetColumnValue("CashierID", value);
            }
        }
	      
        [XmlAttribute("PointOfSaleID")]
        [Bindable(true)]
        public int PointOfSaleID 
	    {
		    get
		    {
			    return GetColumnValue<int>("PointOfSaleID");
		    }
            set 
		    {
			    SetColumnValue("PointOfSaleID", value);
            }
        }
	      
        [XmlAttribute("OrderDate")]
        [Bindable(true)]
        public DateTime OrderDate 
	    {
		    get
		    {
			    return GetColumnValue<DateTime>("OrderDate");
		    }
            set 
		    {
			    SetColumnValue("OrderDate", value);
            }
        }
	      
        [XmlAttribute("GrossAmount")]
        [Bindable(true)]
        public decimal GrossAmount 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("GrossAmount");
		    }
            set 
		    {
			    SetColumnValue("GrossAmount", value);
            }
        }
	      
        [XmlAttribute("NettAmount")]
        [Bindable(true)]
        public decimal NettAmount 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("NettAmount");
		    }
            set 
		    {
			    SetColumnValue("NettAmount", value);
            }
        }
	      
        [XmlAttribute("DiscountAmount")]
        [Bindable(true)]
        public decimal DiscountAmount 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("DiscountAmount");
		    }
            set 
		    {
			    SetColumnValue("DiscountAmount", value);
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
	      
        [XmlAttribute("IsVoided")]
        [Bindable(true)]
        public bool IsVoided 
	    {
		    get
		    {
			    return GetColumnValue<bool>("IsVoided");
		    }
            set 
		    {
			    SetColumnValue("IsVoided", value);
            }
        }
	      
        [XmlAttribute("MembershipNo")]
        [Bindable(true)]
        public string MembershipNo 
	    {
		    get
		    {
			    return GetColumnValue<string>("MembershipNo");
		    }
            set 
		    {
			    SetColumnValue("MembershipNo", value);
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
	      
        [XmlAttribute("CreatedOn")]
        [Bindable(true)]
        public DateTime CreatedOn 
	    {
		    get
		    {
			    return GetColumnValue<DateTime>("CreatedOn");
		    }
            set 
		    {
			    SetColumnValue("CreatedOn", value);
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
	      
        [XmlAttribute("ModifiedOn")]
        [Bindable(true)]
        public DateTime ModifiedOn 
	    {
		    get
		    {
			    return GetColumnValue<DateTime>("ModifiedOn");
		    }
            set 
		    {
			    SetColumnValue("ModifiedOn", value);
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
	      
        [XmlAttribute("PromoCodeID")]
        [Bindable(true)]
        public int? PromoCodeID 
	    {
		    get
		    {
			    return GetColumnValue<int?>("PromoCodeID");
		    }
            set 
		    {
			    SetColumnValue("PromoCodeID", value);
            }
        }
	      
        [XmlAttribute("GSTAmount")]
        [Bindable(true)]
        public decimal? GSTAmount 
	    {
		    get
		    {
			    return GetColumnValue<decimal?>("GSTAmount");
		    }
            set 
		    {
			    SetColumnValue("GSTAmount", value);
            }
        }
	      
        [XmlAttribute("IsPointAllocated")]
        [Bindable(true)]
        public bool IsPointAllocated 
	    {
		    get
		    {
			    return GetColumnValue<bool>("IsPointAllocated");
		    }
            set 
		    {
			    SetColumnValue("IsPointAllocated", value);
            }
        }
	      
        [XmlAttribute("InstOutstandingBal")]
        [Bindable(true)]
        public decimal InstOutstandingBal 
	    {
		    get
		    {
			    return GetColumnValue<decimal>("InstOutstandingBal");
		    }
            set 
		    {
			    SetColumnValue("InstOutstandingBal", value);
            }
        }
	    
	    #endregion
    
	    #region Columns Struct
	    public struct Columns
	    {
		    
		    
            public static string OrderHdrID = @"OrderHdrID";
            
            public static string OrderRefNo = @"OrderRefNo";
            
            public static string Discount = @"Discount";
            
            public static string InventoryHdrRefNo = @"InventoryHdrRefNo";
            
            public static string CashierID = @"CashierID";
            
            public static string PointOfSaleID = @"PointOfSaleID";
            
            public static string OrderDate = @"OrderDate";
            
            public static string GrossAmount = @"GrossAmount";
            
            public static string NettAmount = @"NettAmount";
            
            public static string DiscountAmount = @"DiscountAmount";
            
            public static string Gst = @"GST";
            
            public static string IsVoided = @"IsVoided";
            
            public static string MembershipNo = @"MembershipNo";
            
            public static string Remark = @"Remark";
            
            public static string CreatedOn = @"CreatedOn";
            
            public static string CreatedBy = @"CreatedBy";
            
            public static string ModifiedOn = @"ModifiedOn";
            
            public static string ModifiedBy = @"ModifiedBy";
            
            public static string UniqueID = @"UniqueID";
            
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
            
            public static string PromoCodeID = @"PromoCodeID";
            
            public static string GSTAmount = @"GSTAmount";
            
            public static string IsPointAllocated = @"IsPointAllocated";
            
            public static string InstOutstandingBal = @"InstOutstandingBal";
            
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
