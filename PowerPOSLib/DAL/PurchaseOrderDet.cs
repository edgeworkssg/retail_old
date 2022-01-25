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
namespace PowerPOS
{
	/// <summary>
	/// Strongly-typed collection for the PurchaseOrderDet class.
	/// </summary>
    [Serializable]
	public partial class PurchaseOrderDetCollection : ActiveList<PurchaseOrderDet, PurchaseOrderDetCollection>
	{	   
		public PurchaseOrderDetCollection() {}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>PurchaseOrderDetCollection</returns>
		public PurchaseOrderDetCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                PurchaseOrderDet o = this[i];
                foreach (SubSonic.Where w in this.wheres)
                {
                    bool remove = false;
                    System.Reflection.PropertyInfo pi = o.GetType().GetProperty(w.ColumnName);
                    if (pi.CanRead)
                    {
                        object val = pi.GetValue(o, null);
                        switch (w.Comparison)
                        {
                            case SubSonic.Comparison.Equals:
                                if (!val.Equals(w.ParameterValue))
                                {
                                    remove = true;
                                }
                                break;
                        }
                    }
                    if (remove)
                    {
                        this.Remove(o);
                        break;
                    }
                }
            }
            return this;
        }
		
		
	}
	/// <summary>
	/// This is an ActiveRecord class which wraps the PurchaseOrderDet table.
	/// </summary>
	[Serializable]
	public partial class PurchaseOrderDet : ActiveRecord<PurchaseOrderDet>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public PurchaseOrderDet()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public PurchaseOrderDet(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public PurchaseOrderDet(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public PurchaseOrderDet(string columnName, object columnValue)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByParam(columnName,columnValue);
		}
		
		protected static void SetSQLProps() { GetTableSchema(); }
		
		#endregion
		
		#region Schema and Query Accessor	
		public static Query CreateQuery() { return new Query(Schema); }
		public static TableSchema.Table Schema
		{
			get
			{
				if (BaseSchema == null)
					SetSQLProps();
				return BaseSchema;
			}
		}
		
		private static void GetTableSchema() 
		{
			if(!IsSchemaInitialized)
			{
				//Schema declaration
				TableSchema.Table schema = new TableSchema.Table("PurchaseOrderDet", TableType.Table, DataService.GetInstance("PowerPOS"));
				schema.Columns = new TableSchema.TableColumnCollection();
				schema.SchemaName = @"dbo";
				//columns
				
				TableSchema.TableColumn colvarPurchaseOrderDetRefNo = new TableSchema.TableColumn(schema);
				colvarPurchaseOrderDetRefNo.ColumnName = "PurchaseOrderDetRefNo";
				colvarPurchaseOrderDetRefNo.DataType = DbType.AnsiString;
				colvarPurchaseOrderDetRefNo.MaxLength = 50;
				colvarPurchaseOrderDetRefNo.AutoIncrement = false;
				colvarPurchaseOrderDetRefNo.IsNullable = false;
				colvarPurchaseOrderDetRefNo.IsPrimaryKey = true;
				colvarPurchaseOrderDetRefNo.IsForeignKey = false;
				colvarPurchaseOrderDetRefNo.IsReadOnly = false;
				colvarPurchaseOrderDetRefNo.DefaultSetting = @"";
				colvarPurchaseOrderDetRefNo.ForeignKeyTableName = "";
				schema.Columns.Add(colvarPurchaseOrderDetRefNo);
				
				TableSchema.TableColumn colvarItemNo = new TableSchema.TableColumn(schema);
				colvarItemNo.ColumnName = "ItemNo";
				colvarItemNo.DataType = DbType.AnsiString;
				colvarItemNo.MaxLength = 50;
				colvarItemNo.AutoIncrement = false;
				colvarItemNo.IsNullable = false;
				colvarItemNo.IsPrimaryKey = false;
				colvarItemNo.IsForeignKey = true;
				colvarItemNo.IsReadOnly = false;
				colvarItemNo.DefaultSetting = @"";
				
					colvarItemNo.ForeignKeyTableName = "Item";
				schema.Columns.Add(colvarItemNo);
				
				TableSchema.TableColumn colvarPurchaseOrderHdrRefNo = new TableSchema.TableColumn(schema);
				colvarPurchaseOrderHdrRefNo.ColumnName = "PurchaseOrderHdrRefNo";
				colvarPurchaseOrderHdrRefNo.DataType = DbType.AnsiString;
				colvarPurchaseOrderHdrRefNo.MaxLength = 50;
				colvarPurchaseOrderHdrRefNo.AutoIncrement = false;
				colvarPurchaseOrderHdrRefNo.IsNullable = false;
				colvarPurchaseOrderHdrRefNo.IsPrimaryKey = false;
				colvarPurchaseOrderHdrRefNo.IsForeignKey = true;
				colvarPurchaseOrderHdrRefNo.IsReadOnly = false;
				colvarPurchaseOrderHdrRefNo.DefaultSetting = @"";
				
					colvarPurchaseOrderHdrRefNo.ForeignKeyTableName = "PurchaseOrderHdr";
				schema.Columns.Add(colvarPurchaseOrderHdrRefNo);
				
				TableSchema.TableColumn colvarExpiryDate = new TableSchema.TableColumn(schema);
				colvarExpiryDate.ColumnName = "ExpiryDate";
				colvarExpiryDate.DataType = DbType.DateTime;
				colvarExpiryDate.MaxLength = 0;
				colvarExpiryDate.AutoIncrement = false;
				colvarExpiryDate.IsNullable = true;
				colvarExpiryDate.IsPrimaryKey = false;
				colvarExpiryDate.IsForeignKey = false;
				colvarExpiryDate.IsReadOnly = false;
				colvarExpiryDate.DefaultSetting = @"";
				colvarExpiryDate.ForeignKeyTableName = "";
				schema.Columns.Add(colvarExpiryDate);
				
				TableSchema.TableColumn colvarQuantity = new TableSchema.TableColumn(schema);
				colvarQuantity.ColumnName = "Quantity";
				colvarQuantity.DataType = DbType.Decimal;
				colvarQuantity.MaxLength = 0;
				colvarQuantity.AutoIncrement = false;
				colvarQuantity.IsNullable = true;
				colvarQuantity.IsPrimaryKey = false;
				colvarQuantity.IsForeignKey = false;
				colvarQuantity.IsReadOnly = false;
				colvarQuantity.DefaultSetting = @"";
				colvarQuantity.ForeignKeyTableName = "";
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
				colvarRemainingQty.DefaultSetting = @"";
				colvarRemainingQty.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRemainingQty);
				
				TableSchema.TableColumn colvarFactoryPrice = new TableSchema.TableColumn(schema);
				colvarFactoryPrice.ColumnName = "FactoryPrice";
				colvarFactoryPrice.DataType = DbType.Currency;
				colvarFactoryPrice.MaxLength = 0;
				colvarFactoryPrice.AutoIncrement = false;
				colvarFactoryPrice.IsNullable = false;
				colvarFactoryPrice.IsPrimaryKey = false;
				colvarFactoryPrice.IsForeignKey = false;
				colvarFactoryPrice.IsReadOnly = false;
				colvarFactoryPrice.DefaultSetting = @"";
				colvarFactoryPrice.ForeignKeyTableName = "";
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
				colvarGst.DefaultSetting = @"";
				colvarGst.ForeignKeyTableName = "";
				schema.Columns.Add(colvarGst);
				
				TableSchema.TableColumn colvarCostOfGoods = new TableSchema.TableColumn(schema);
				colvarCostOfGoods.ColumnName = "CostOfGoods";
				colvarCostOfGoods.DataType = DbType.Currency;
				colvarCostOfGoods.MaxLength = 0;
				colvarCostOfGoods.AutoIncrement = false;
				colvarCostOfGoods.IsNullable = false;
				colvarCostOfGoods.IsPrimaryKey = false;
				colvarCostOfGoods.IsForeignKey = false;
				colvarCostOfGoods.IsReadOnly = false;
				colvarCostOfGoods.DefaultSetting = @"";
				colvarCostOfGoods.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCostOfGoods);
				
				TableSchema.TableColumn colvarRemark = new TableSchema.TableColumn(schema);
				colvarRemark.ColumnName = "Remark";
				colvarRemark.DataType = DbType.String;
				colvarRemark.MaxLength = -1;
				colvarRemark.AutoIncrement = false;
				colvarRemark.IsNullable = true;
				colvarRemark.IsPrimaryKey = false;
				colvarRemark.IsForeignKey = false;
				colvarRemark.IsReadOnly = false;
				colvarRemark.DefaultSetting = @"";
				colvarRemark.ForeignKeyTableName = "";
				schema.Columns.Add(colvarRemark);
				
				TableSchema.TableColumn colvarDiscount = new TableSchema.TableColumn(schema);
				colvarDiscount.ColumnName = "Discount";
				colvarDiscount.DataType = DbType.Decimal;
				colvarDiscount.MaxLength = 0;
				colvarDiscount.AutoIncrement = false;
				colvarDiscount.IsNullable = true;
				colvarDiscount.IsPrimaryKey = false;
				colvarDiscount.IsForeignKey = false;
				colvarDiscount.IsReadOnly = false;
				colvarDiscount.DefaultSetting = @"";
				colvarDiscount.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDiscount);
				
				TableSchema.TableColumn colvarCreatedBy = new TableSchema.TableColumn(schema);
				colvarCreatedBy.ColumnName = "CreatedBy";
				colvarCreatedBy.DataType = DbType.AnsiString;
				colvarCreatedBy.MaxLength = 50;
				colvarCreatedBy.AutoIncrement = false;
				colvarCreatedBy.IsNullable = true;
				colvarCreatedBy.IsPrimaryKey = false;
				colvarCreatedBy.IsForeignKey = false;
				colvarCreatedBy.IsReadOnly = false;
				colvarCreatedBy.DefaultSetting = @"";
				colvarCreatedBy.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCreatedBy);
				
				TableSchema.TableColumn colvarCreatedOn = new TableSchema.TableColumn(schema);
				colvarCreatedOn.ColumnName = "CreatedOn";
				colvarCreatedOn.DataType = DbType.DateTime;
				colvarCreatedOn.MaxLength = 0;
				colvarCreatedOn.AutoIncrement = false;
				colvarCreatedOn.IsNullable = true;
				colvarCreatedOn.IsPrimaryKey = false;
				colvarCreatedOn.IsForeignKey = false;
				colvarCreatedOn.IsReadOnly = false;
				colvarCreatedOn.DefaultSetting = @"";
				colvarCreatedOn.ForeignKeyTableName = "";
				schema.Columns.Add(colvarCreatedOn);
				
				TableSchema.TableColumn colvarModifiedBy = new TableSchema.TableColumn(schema);
				colvarModifiedBy.ColumnName = "ModifiedBy";
				colvarModifiedBy.DataType = DbType.AnsiString;
				colvarModifiedBy.MaxLength = 50;
				colvarModifiedBy.AutoIncrement = false;
				colvarModifiedBy.IsNullable = true;
				colvarModifiedBy.IsPrimaryKey = false;
				colvarModifiedBy.IsForeignKey = false;
				colvarModifiedBy.IsReadOnly = false;
				colvarModifiedBy.DefaultSetting = @"";
				colvarModifiedBy.ForeignKeyTableName = "";
				schema.Columns.Add(colvarModifiedBy);
				
				TableSchema.TableColumn colvarModifiedOn = new TableSchema.TableColumn(schema);
				colvarModifiedOn.ColumnName = "ModifiedOn";
				colvarModifiedOn.DataType = DbType.DateTime;
				colvarModifiedOn.MaxLength = 0;
				colvarModifiedOn.AutoIncrement = false;
				colvarModifiedOn.IsNullable = true;
				colvarModifiedOn.IsPrimaryKey = false;
				colvarModifiedOn.IsForeignKey = false;
				colvarModifiedOn.IsReadOnly = false;
				colvarModifiedOn.DefaultSetting = @"";
				colvarModifiedOn.ForeignKeyTableName = "";
				schema.Columns.Add(colvarModifiedOn);
				
				TableSchema.TableColumn colvarUniqueID = new TableSchema.TableColumn(schema);
				colvarUniqueID.ColumnName = "UniqueID";
				colvarUniqueID.DataType = DbType.Guid;
				colvarUniqueID.MaxLength = 0;
				colvarUniqueID.AutoIncrement = false;
				colvarUniqueID.IsNullable = false;
				colvarUniqueID.IsPrimaryKey = false;
				colvarUniqueID.IsForeignKey = false;
				colvarUniqueID.IsReadOnly = false;
				
						colvarUniqueID.DefaultSetting = @"(newid())";
				colvarUniqueID.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUniqueID);
				
				TableSchema.TableColumn colvarUserfld1 = new TableSchema.TableColumn(schema);
				colvarUserfld1.ColumnName = "userfld1";
				colvarUserfld1.DataType = DbType.String;
				colvarUserfld1.MaxLength = -1;
				colvarUserfld1.AutoIncrement = false;
				colvarUserfld1.IsNullable = true;
				colvarUserfld1.IsPrimaryKey = false;
				colvarUserfld1.IsForeignKey = false;
				colvarUserfld1.IsReadOnly = false;
				colvarUserfld1.DefaultSetting = @"";
				colvarUserfld1.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld1);
				
				TableSchema.TableColumn colvarUserfld2 = new TableSchema.TableColumn(schema);
				colvarUserfld2.ColumnName = "userfld2";
				colvarUserfld2.DataType = DbType.String;
				colvarUserfld2.MaxLength = -1;
				colvarUserfld2.AutoIncrement = false;
				colvarUserfld2.IsNullable = true;
				colvarUserfld2.IsPrimaryKey = false;
				colvarUserfld2.IsForeignKey = false;
				colvarUserfld2.IsReadOnly = false;
				colvarUserfld2.DefaultSetting = @"";
				colvarUserfld2.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld2);
				
				TableSchema.TableColumn colvarUserfld3 = new TableSchema.TableColumn(schema);
				colvarUserfld3.ColumnName = "userfld3";
				colvarUserfld3.DataType = DbType.String;
				colvarUserfld3.MaxLength = -1;
				colvarUserfld3.AutoIncrement = false;
				colvarUserfld3.IsNullable = true;
				colvarUserfld3.IsPrimaryKey = false;
				colvarUserfld3.IsForeignKey = false;
				colvarUserfld3.IsReadOnly = false;
				colvarUserfld3.DefaultSetting = @"";
				colvarUserfld3.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld3);
				
				TableSchema.TableColumn colvarUserfld4 = new TableSchema.TableColumn(schema);
				colvarUserfld4.ColumnName = "userfld4";
				colvarUserfld4.DataType = DbType.String;
				colvarUserfld4.MaxLength = -1;
				colvarUserfld4.AutoIncrement = false;
				colvarUserfld4.IsNullable = true;
				colvarUserfld4.IsPrimaryKey = false;
				colvarUserfld4.IsForeignKey = false;
				colvarUserfld4.IsReadOnly = false;
				colvarUserfld4.DefaultSetting = @"";
				colvarUserfld4.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld4);
				
				TableSchema.TableColumn colvarUserfld5 = new TableSchema.TableColumn(schema);
				colvarUserfld5.ColumnName = "userfld5";
				colvarUserfld5.DataType = DbType.String;
				colvarUserfld5.MaxLength = -1;
				colvarUserfld5.AutoIncrement = false;
				colvarUserfld5.IsNullable = true;
				colvarUserfld5.IsPrimaryKey = false;
				colvarUserfld5.IsForeignKey = false;
				colvarUserfld5.IsReadOnly = false;
				colvarUserfld5.DefaultSetting = @"";
				colvarUserfld5.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld5);
				
				TableSchema.TableColumn colvarUserfld6 = new TableSchema.TableColumn(schema);
				colvarUserfld6.ColumnName = "userfld6";
				colvarUserfld6.DataType = DbType.String;
				colvarUserfld6.MaxLength = -1;
				colvarUserfld6.AutoIncrement = false;
				colvarUserfld6.IsNullable = true;
				colvarUserfld6.IsPrimaryKey = false;
				colvarUserfld6.IsForeignKey = false;
				colvarUserfld6.IsReadOnly = false;
				colvarUserfld6.DefaultSetting = @"";
				colvarUserfld6.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld6);
				
				TableSchema.TableColumn colvarUserfld7 = new TableSchema.TableColumn(schema);
				colvarUserfld7.ColumnName = "userfld7";
				colvarUserfld7.DataType = DbType.String;
				colvarUserfld7.MaxLength = -1;
				colvarUserfld7.AutoIncrement = false;
				colvarUserfld7.IsNullable = true;
				colvarUserfld7.IsPrimaryKey = false;
				colvarUserfld7.IsForeignKey = false;
				colvarUserfld7.IsReadOnly = false;
				colvarUserfld7.DefaultSetting = @"";
				colvarUserfld7.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld7);
				
				TableSchema.TableColumn colvarUserfld8 = new TableSchema.TableColumn(schema);
				colvarUserfld8.ColumnName = "userfld8";
				colvarUserfld8.DataType = DbType.String;
				colvarUserfld8.MaxLength = -1;
				colvarUserfld8.AutoIncrement = false;
				colvarUserfld8.IsNullable = true;
				colvarUserfld8.IsPrimaryKey = false;
				colvarUserfld8.IsForeignKey = false;
				colvarUserfld8.IsReadOnly = false;
				colvarUserfld8.DefaultSetting = @"";
				colvarUserfld8.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld8);
				
				TableSchema.TableColumn colvarUserfld9 = new TableSchema.TableColumn(schema);
				colvarUserfld9.ColumnName = "userfld9";
				colvarUserfld9.DataType = DbType.String;
				colvarUserfld9.MaxLength = -1;
				colvarUserfld9.AutoIncrement = false;
				colvarUserfld9.IsNullable = true;
				colvarUserfld9.IsPrimaryKey = false;
				colvarUserfld9.IsForeignKey = false;
				colvarUserfld9.IsReadOnly = false;
				colvarUserfld9.DefaultSetting = @"";
				colvarUserfld9.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfld9);
				
				TableSchema.TableColumn colvarUserfld10 = new TableSchema.TableColumn(schema);
				colvarUserfld10.ColumnName = "userfld10";
				colvarUserfld10.DataType = DbType.String;
				colvarUserfld10.MaxLength = -1;
				colvarUserfld10.AutoIncrement = false;
				colvarUserfld10.IsNullable = true;
				colvarUserfld10.IsPrimaryKey = false;
				colvarUserfld10.IsForeignKey = false;
				colvarUserfld10.IsReadOnly = false;
				colvarUserfld10.DefaultSetting = @"";
				colvarUserfld10.ForeignKeyTableName = "";
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
				colvarUserflag1.DefaultSetting = @"";
				colvarUserflag1.ForeignKeyTableName = "";
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
				colvarUserflag2.DefaultSetting = @"";
				colvarUserflag2.ForeignKeyTableName = "";
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
				colvarUserflag3.DefaultSetting = @"";
				colvarUserflag3.ForeignKeyTableName = "";
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
				colvarUserflag4.DefaultSetting = @"";
				colvarUserflag4.ForeignKeyTableName = "";
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
				colvarUserflag5.DefaultSetting = @"";
				colvarUserflag5.ForeignKeyTableName = "";
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
				colvarUserfloat1.DefaultSetting = @"";
				colvarUserfloat1.ForeignKeyTableName = "";
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
				colvarUserfloat2.DefaultSetting = @"";
				colvarUserfloat2.ForeignKeyTableName = "";
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
				colvarUserfloat3.DefaultSetting = @"";
				colvarUserfloat3.ForeignKeyTableName = "";
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
				colvarUserfloat4.DefaultSetting = @"";
				colvarUserfloat4.ForeignKeyTableName = "";
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
				colvarUserfloat5.DefaultSetting = @"";
				colvarUserfloat5.ForeignKeyTableName = "";
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
				colvarUserint1.DefaultSetting = @"";
				colvarUserint1.ForeignKeyTableName = "";
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
				colvarUserint2.DefaultSetting = @"";
				colvarUserint2.ForeignKeyTableName = "";
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
				colvarUserint3.DefaultSetting = @"";
				colvarUserint3.ForeignKeyTableName = "";
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
				colvarUserint4.DefaultSetting = @"";
				colvarUserint4.ForeignKeyTableName = "";
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
				colvarUserint5.DefaultSetting = @"";
				colvarUserint5.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserint5);
				
				TableSchema.TableColumn colvarBalanceBefore = new TableSchema.TableColumn(schema);
				colvarBalanceBefore.ColumnName = "BalanceBefore";
				colvarBalanceBefore.DataType = DbType.Int32;
				colvarBalanceBefore.MaxLength = 0;
				colvarBalanceBefore.AutoIncrement = false;
				colvarBalanceBefore.IsNullable = false;
				colvarBalanceBefore.IsPrimaryKey = false;
				colvarBalanceBefore.IsForeignKey = false;
				colvarBalanceBefore.IsReadOnly = false;
				colvarBalanceBefore.DefaultSetting = @"";
				colvarBalanceBefore.ForeignKeyTableName = "";
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
				colvarBalanceAfter.DefaultSetting = @"";
				colvarBalanceAfter.ForeignKeyTableName = "";
				schema.Columns.Add(colvarBalanceAfter);
				
				TableSchema.TableColumn colvarDeleted = new TableSchema.TableColumn(schema);
				colvarDeleted.ColumnName = "Deleted";
				colvarDeleted.DataType = DbType.Boolean;
				colvarDeleted.MaxLength = 0;
				colvarDeleted.AutoIncrement = false;
				colvarDeleted.IsNullable = true;
				colvarDeleted.IsPrimaryKey = false;
				colvarDeleted.IsForeignKey = false;
				colvarDeleted.IsReadOnly = false;
				colvarDeleted.DefaultSetting = @"";
				colvarDeleted.ForeignKeyTableName = "";
				schema.Columns.Add(colvarDeleted);
				
				TableSchema.TableColumn colvarStockinrefno = new TableSchema.TableColumn(schema);
				colvarStockinrefno.ColumnName = "stockinrefno";
				colvarStockinrefno.DataType = DbType.AnsiString;
				colvarStockinrefno.MaxLength = 20;
				colvarStockinrefno.AutoIncrement = false;
				colvarStockinrefno.IsNullable = true;
				colvarStockinrefno.IsPrimaryKey = false;
				colvarStockinrefno.IsForeignKey = false;
				colvarStockinrefno.IsReadOnly = false;
				colvarStockinrefno.DefaultSetting = @"";
				colvarStockinrefno.ForeignKeyTableName = "";
				schema.Columns.Add(colvarStockinrefno);
				
				TableSchema.TableColumn colvarIsdiscrepancy = new TableSchema.TableColumn(schema);
				colvarIsdiscrepancy.ColumnName = "isdiscrepancy";
				colvarIsdiscrepancy.DataType = DbType.Boolean;
				colvarIsdiscrepancy.MaxLength = 0;
				colvarIsdiscrepancy.AutoIncrement = false;
				colvarIsdiscrepancy.IsNullable = true;
				colvarIsdiscrepancy.IsPrimaryKey = false;
				colvarIsdiscrepancy.IsForeignKey = false;
				colvarIsdiscrepancy.IsReadOnly = false;
				colvarIsdiscrepancy.DefaultSetting = @"";
				colvarIsdiscrepancy.ForeignKeyTableName = "";
				schema.Columns.Add(colvarIsdiscrepancy);
				
				TableSchema.TableColumn colvarUserfloat6 = new TableSchema.TableColumn(schema);
				colvarUserfloat6.ColumnName = "userfloat6";
				colvarUserfloat6.DataType = DbType.Currency;
				colvarUserfloat6.MaxLength = 0;
				colvarUserfloat6.AutoIncrement = false;
				colvarUserfloat6.IsNullable = true;
				colvarUserfloat6.IsPrimaryKey = false;
				colvarUserfloat6.IsForeignKey = false;
				colvarUserfloat6.IsReadOnly = false;
				colvarUserfloat6.DefaultSetting = @"";
				colvarUserfloat6.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat6);
				
				TableSchema.TableColumn colvarUserfloat7 = new TableSchema.TableColumn(schema);
				colvarUserfloat7.ColumnName = "userfloat7";
				colvarUserfloat7.DataType = DbType.Currency;
				colvarUserfloat7.MaxLength = 0;
				colvarUserfloat7.AutoIncrement = false;
				colvarUserfloat7.IsNullable = true;
				colvarUserfloat7.IsPrimaryKey = false;
				colvarUserfloat7.IsForeignKey = false;
				colvarUserfloat7.IsReadOnly = false;
				colvarUserfloat7.DefaultSetting = @"";
				colvarUserfloat7.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat7);
				
				TableSchema.TableColumn colvarUserfloat8 = new TableSchema.TableColumn(schema);
				colvarUserfloat8.ColumnName = "userfloat8";
				colvarUserfloat8.DataType = DbType.Currency;
				colvarUserfloat8.MaxLength = 0;
				colvarUserfloat8.AutoIncrement = false;
				colvarUserfloat8.IsNullable = true;
				colvarUserfloat8.IsPrimaryKey = false;
				colvarUserfloat8.IsForeignKey = false;
				colvarUserfloat8.IsReadOnly = false;
				colvarUserfloat8.DefaultSetting = @"";
				colvarUserfloat8.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat8);
				
				TableSchema.TableColumn colvarUserfloat9 = new TableSchema.TableColumn(schema);
				colvarUserfloat9.ColumnName = "userfloat9";
				colvarUserfloat9.DataType = DbType.Currency;
				colvarUserfloat9.MaxLength = 0;
				colvarUserfloat9.AutoIncrement = false;
				colvarUserfloat9.IsNullable = true;
				colvarUserfloat9.IsPrimaryKey = false;
				colvarUserfloat9.IsForeignKey = false;
				colvarUserfloat9.IsReadOnly = false;
				colvarUserfloat9.DefaultSetting = @"";
				colvarUserfloat9.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat9);
				
				TableSchema.TableColumn colvarUserfloat10 = new TableSchema.TableColumn(schema);
				colvarUserfloat10.ColumnName = "userfloat10";
				colvarUserfloat10.DataType = DbType.Currency;
				colvarUserfloat10.MaxLength = 0;
				colvarUserfloat10.AutoIncrement = false;
				colvarUserfloat10.IsNullable = true;
				colvarUserfloat10.IsPrimaryKey = false;
				colvarUserfloat10.IsForeignKey = false;
				colvarUserfloat10.IsReadOnly = false;
				colvarUserfloat10.DefaultSetting = @"";
				colvarUserfloat10.ForeignKeyTableName = "";
				schema.Columns.Add(colvarUserfloat10);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["PowerPOS"].AddSchema("PurchaseOrderDet",schema);
			}
		}
		#endregion
		
		#region Props
		  
		[XmlAttribute("PurchaseOrderDetRefNo")]
		[Bindable(true)]
		public string PurchaseOrderDetRefNo 
		{
			get { return GetColumnValue<string>(Columns.PurchaseOrderDetRefNo); }
			set { SetColumnValue(Columns.PurchaseOrderDetRefNo, value); }
		}
		  
		[XmlAttribute("ItemNo")]
		[Bindable(true)]
		public string ItemNo 
		{
			get { return GetColumnValue<string>(Columns.ItemNo); }
			set { SetColumnValue(Columns.ItemNo, value); }
		}
		  
		[XmlAttribute("PurchaseOrderHdrRefNo")]
		[Bindable(true)]
		public string PurchaseOrderHdrRefNo 
		{
			get { return GetColumnValue<string>(Columns.PurchaseOrderHdrRefNo); }
			set { SetColumnValue(Columns.PurchaseOrderHdrRefNo, value); }
		}
		  
		[XmlAttribute("ExpiryDate")]
		[Bindable(true)]
		public DateTime? ExpiryDate 
		{
			get { return GetColumnValue<DateTime?>(Columns.ExpiryDate); }
			set { SetColumnValue(Columns.ExpiryDate, value); }
		}
		  
		[XmlAttribute("Quantity")]
		[Bindable(true)]
		public decimal? Quantity 
		{
			get { return GetColumnValue<decimal?>(Columns.Quantity); }
			set { SetColumnValue(Columns.Quantity, value); }
		}
		  
		[XmlAttribute("RemainingQty")]
		[Bindable(true)]
		public decimal? RemainingQty 
		{
			get { return GetColumnValue<decimal?>(Columns.RemainingQty); }
			set { SetColumnValue(Columns.RemainingQty, value); }
		}
		  
		[XmlAttribute("FactoryPrice")]
		[Bindable(true)]
		public decimal FactoryPrice 
		{
			get { return GetColumnValue<decimal>(Columns.FactoryPrice); }
			set { SetColumnValue(Columns.FactoryPrice, value); }
		}
		  
		[XmlAttribute("Gst")]
		[Bindable(true)]
		public double Gst 
		{
			get { return GetColumnValue<double>(Columns.Gst); }
			set { SetColumnValue(Columns.Gst, value); }
		}
		  
		[XmlAttribute("CostOfGoods")]
		[Bindable(true)]
		public decimal CostOfGoods 
		{
			get { return GetColumnValue<decimal>(Columns.CostOfGoods); }
			set { SetColumnValue(Columns.CostOfGoods, value); }
		}
		  
		[XmlAttribute("Remark")]
		[Bindable(true)]
		public string Remark 
		{
			get { return GetColumnValue<string>(Columns.Remark); }
			set { SetColumnValue(Columns.Remark, value); }
		}
		  
		[XmlAttribute("Discount")]
		[Bindable(true)]
		public decimal? Discount 
		{
			get { return GetColumnValue<decimal?>(Columns.Discount); }
			set { SetColumnValue(Columns.Discount, value); }
		}
		  
		[XmlAttribute("CreatedBy")]
		[Bindable(true)]
		public string CreatedBy 
		{
			get { return GetColumnValue<string>(Columns.CreatedBy); }
			set { SetColumnValue(Columns.CreatedBy, value); }
		}
		  
		[XmlAttribute("CreatedOn")]
		[Bindable(true)]
		public DateTime? CreatedOn 
		{
			get { return GetColumnValue<DateTime?>(Columns.CreatedOn); }
			set { SetColumnValue(Columns.CreatedOn, value); }
		}
		  
		[XmlAttribute("ModifiedBy")]
		[Bindable(true)]
		public string ModifiedBy 
		{
			get { return GetColumnValue<string>(Columns.ModifiedBy); }
			set { SetColumnValue(Columns.ModifiedBy, value); }
		}
		  
		[XmlAttribute("ModifiedOn")]
		[Bindable(true)]
		public DateTime? ModifiedOn 
		{
			get { return GetColumnValue<DateTime?>(Columns.ModifiedOn); }
			set { SetColumnValue(Columns.ModifiedOn, value); }
		}
		  
		[XmlAttribute("UniqueID")]
		[Bindable(true)]
		public Guid UniqueID 
		{
			get { return GetColumnValue<Guid>(Columns.UniqueID); }
			set { SetColumnValue(Columns.UniqueID, value); }
		}
		  
		[XmlAttribute("Userfld1")]
		[Bindable(true)]
		public string Userfld1 
		{
			get { return GetColumnValue<string>(Columns.Userfld1); }
			set { SetColumnValue(Columns.Userfld1, value); }
		}
		  
		[XmlAttribute("Userfld2")]
		[Bindable(true)]
		public string Userfld2 
		{
			get { return GetColumnValue<string>(Columns.Userfld2); }
			set { SetColumnValue(Columns.Userfld2, value); }
		}
		  
		[XmlAttribute("Userfld3")]
		[Bindable(true)]
		public string Userfld3 
		{
			get { return GetColumnValue<string>(Columns.Userfld3); }
			set { SetColumnValue(Columns.Userfld3, value); }
		}
		  
		[XmlAttribute("Userfld4")]
		[Bindable(true)]
		public string Userfld4 
		{
			get { return GetColumnValue<string>(Columns.Userfld4); }
			set { SetColumnValue(Columns.Userfld4, value); }
		}
		  
		[XmlAttribute("Userfld5")]
		[Bindable(true)]
		public string Userfld5 
		{
			get { return GetColumnValue<string>(Columns.Userfld5); }
			set { SetColumnValue(Columns.Userfld5, value); }
		}
		  
		[XmlAttribute("Userfld6")]
		[Bindable(true)]
		public string Userfld6 
		{
			get { return GetColumnValue<string>(Columns.Userfld6); }
			set { SetColumnValue(Columns.Userfld6, value); }
		}
		  
		[XmlAttribute("Userfld7")]
		[Bindable(true)]
		public string Userfld7 
		{
			get { return GetColumnValue<string>(Columns.Userfld7); }
			set { SetColumnValue(Columns.Userfld7, value); }
		}
		  
		[XmlAttribute("Userfld8")]
		[Bindable(true)]
		public string Userfld8 
		{
			get { return GetColumnValue<string>(Columns.Userfld8); }
			set { SetColumnValue(Columns.Userfld8, value); }
		}
		  
		[XmlAttribute("Userfld9")]
		[Bindable(true)]
		public string Userfld9 
		{
			get { return GetColumnValue<string>(Columns.Userfld9); }
			set { SetColumnValue(Columns.Userfld9, value); }
		}
		  
		[XmlAttribute("Userfld10")]
		[Bindable(true)]
		public string Userfld10 
		{
			get { return GetColumnValue<string>(Columns.Userfld10); }
			set { SetColumnValue(Columns.Userfld10, value); }
		}
		  
		[XmlAttribute("Userflag1")]
		[Bindable(true)]
		public bool? Userflag1 
		{
			get { return GetColumnValue<bool?>(Columns.Userflag1); }
			set { SetColumnValue(Columns.Userflag1, value); }
		}
		  
		[XmlAttribute("Userflag2")]
		[Bindable(true)]
		public bool? Userflag2 
		{
			get { return GetColumnValue<bool?>(Columns.Userflag2); }
			set { SetColumnValue(Columns.Userflag2, value); }
		}
		  
		[XmlAttribute("Userflag3")]
		[Bindable(true)]
		public bool? Userflag3 
		{
			get { return GetColumnValue<bool?>(Columns.Userflag3); }
			set { SetColumnValue(Columns.Userflag3, value); }
		}
		  
		[XmlAttribute("Userflag4")]
		[Bindable(true)]
		public bool? Userflag4 
		{
			get { return GetColumnValue<bool?>(Columns.Userflag4); }
			set { SetColumnValue(Columns.Userflag4, value); }
		}
		  
		[XmlAttribute("Userflag5")]
		[Bindable(true)]
		public bool? Userflag5 
		{
			get { return GetColumnValue<bool?>(Columns.Userflag5); }
			set { SetColumnValue(Columns.Userflag5, value); }
		}
		  
		[XmlAttribute("Userfloat1")]
		[Bindable(true)]
		public decimal? Userfloat1 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat1); }
			set { SetColumnValue(Columns.Userfloat1, value); }
		}
		  
		[XmlAttribute("Userfloat2")]
		[Bindable(true)]
		public decimal? Userfloat2 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat2); }
			set { SetColumnValue(Columns.Userfloat2, value); }
		}
		  
		[XmlAttribute("Userfloat3")]
		[Bindable(true)]
		public decimal? Userfloat3 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat3); }
			set { SetColumnValue(Columns.Userfloat3, value); }
		}
		  
		[XmlAttribute("Userfloat4")]
		[Bindable(true)]
		public decimal? Userfloat4 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat4); }
			set { SetColumnValue(Columns.Userfloat4, value); }
		}
		  
		[XmlAttribute("Userfloat5")]
		[Bindable(true)]
		public decimal? Userfloat5 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat5); }
			set { SetColumnValue(Columns.Userfloat5, value); }
		}
		  
		[XmlAttribute("Userint1")]
		[Bindable(true)]
		public int? Userint1 
		{
			get { return GetColumnValue<int?>(Columns.Userint1); }
			set { SetColumnValue(Columns.Userint1, value); }
		}
		  
		[XmlAttribute("Userint2")]
		[Bindable(true)]
		public int? Userint2 
		{
			get { return GetColumnValue<int?>(Columns.Userint2); }
			set { SetColumnValue(Columns.Userint2, value); }
		}
		  
		[XmlAttribute("Userint3")]
		[Bindable(true)]
		public int? Userint3 
		{
			get { return GetColumnValue<int?>(Columns.Userint3); }
			set { SetColumnValue(Columns.Userint3, value); }
		}
		  
		[XmlAttribute("Userint4")]
		[Bindable(true)]
		public int? Userint4 
		{
			get { return GetColumnValue<int?>(Columns.Userint4); }
			set { SetColumnValue(Columns.Userint4, value); }
		}
		  
		[XmlAttribute("Userint5")]
		[Bindable(true)]
		public int? Userint5 
		{
			get { return GetColumnValue<int?>(Columns.Userint5); }
			set { SetColumnValue(Columns.Userint5, value); }
		}
		  
		[XmlAttribute("BalanceBefore")]
		[Bindable(true)]
		public int BalanceBefore 
		{
			get { return GetColumnValue<int>(Columns.BalanceBefore); }
			set { SetColumnValue(Columns.BalanceBefore, value); }
		}
		  
		[XmlAttribute("BalanceAfter")]
		[Bindable(true)]
		public int BalanceAfter 
		{
			get { return GetColumnValue<int>(Columns.BalanceAfter); }
			set { SetColumnValue(Columns.BalanceAfter, value); }
		}
		  
		[XmlAttribute("Deleted")]
		[Bindable(true)]
		public bool? Deleted 
		{
			get { return GetColumnValue<bool?>(Columns.Deleted); }
			set { SetColumnValue(Columns.Deleted, value); }
		}
		  
		[XmlAttribute("Stockinrefno")]
		[Bindable(true)]
		public string Stockinrefno 
		{
			get { return GetColumnValue<string>(Columns.Stockinrefno); }
			set { SetColumnValue(Columns.Stockinrefno, value); }
		}
		  
		[XmlAttribute("Isdiscrepancy")]
		[Bindable(true)]
		public bool? Isdiscrepancy 
		{
			get { return GetColumnValue<bool?>(Columns.Isdiscrepancy); }
			set { SetColumnValue(Columns.Isdiscrepancy, value); }
		}
		  
		[XmlAttribute("Userfloat6")]
		[Bindable(true)]
		public decimal? Userfloat6 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat6); }
			set { SetColumnValue(Columns.Userfloat6, value); }
		}
		  
		[XmlAttribute("Userfloat7")]
		[Bindable(true)]
		public decimal? Userfloat7 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat7); }
			set { SetColumnValue(Columns.Userfloat7, value); }
		}
		  
		[XmlAttribute("Userfloat8")]
		[Bindable(true)]
		public decimal? Userfloat8 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat8); }
			set { SetColumnValue(Columns.Userfloat8, value); }
		}
		  
		[XmlAttribute("Userfloat9")]
		[Bindable(true)]
		public decimal? Userfloat9 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat9); }
			set { SetColumnValue(Columns.Userfloat9, value); }
		}
		  
		[XmlAttribute("Userfloat10")]
		[Bindable(true)]
		public decimal? Userfloat10 
		{
			get { return GetColumnValue<decimal?>(Columns.Userfloat10); }
			set { SetColumnValue(Columns.Userfloat10, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a Item ActiveRecord object related to this PurchaseOrderDet
		/// 
		/// </summary>
		public PowerPOS.Item Item
		{
			get { return PowerPOS.Item.FetchByID(this.ItemNo); }
			set { SetColumnValue("ItemNo", value.ItemNo); }
		}
		
		
		/// <summary>
		/// Returns a PurchaseOrderHdr ActiveRecord object related to this PurchaseOrderDet
		/// 
		/// </summary>
		public PowerPOS.PurchaseOrderHdr PurchaseOrderHdr
		{
			get { return PowerPOS.PurchaseOrderHdr.FetchByID(this.PurchaseOrderHdrRefNo); }
			set { SetColumnValue("PurchaseOrderHdrRefNo", value.PurchaseOrderHdrRefNo); }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(string varPurchaseOrderDetRefNo,string varItemNo,string varPurchaseOrderHdrRefNo,DateTime? varExpiryDate,decimal? varQuantity,decimal? varRemainingQty,decimal varFactoryPrice,double varGst,decimal varCostOfGoods,string varRemark,decimal? varDiscount,string varCreatedBy,DateTime? varCreatedOn,string varModifiedBy,DateTime? varModifiedOn,Guid varUniqueID,string varUserfld1,string varUserfld2,string varUserfld3,string varUserfld4,string varUserfld5,string varUserfld6,string varUserfld7,string varUserfld8,string varUserfld9,string varUserfld10,bool? varUserflag1,bool? varUserflag2,bool? varUserflag3,bool? varUserflag4,bool? varUserflag5,decimal? varUserfloat1,decimal? varUserfloat2,decimal? varUserfloat3,decimal? varUserfloat4,decimal? varUserfloat5,int? varUserint1,int? varUserint2,int? varUserint3,int? varUserint4,int? varUserint5,int varBalanceBefore,int varBalanceAfter,bool? varDeleted,string varStockinrefno,bool? varIsdiscrepancy,decimal? varUserfloat6,decimal? varUserfloat7,decimal? varUserfloat8,decimal? varUserfloat9,decimal? varUserfloat10)
		{
			PurchaseOrderDet item = new PurchaseOrderDet();
			
			item.PurchaseOrderDetRefNo = varPurchaseOrderDetRefNo;
			
			item.ItemNo = varItemNo;
			
			item.PurchaseOrderHdrRefNo = varPurchaseOrderHdrRefNo;
			
			item.ExpiryDate = varExpiryDate;
			
			item.Quantity = varQuantity;
			
			item.RemainingQty = varRemainingQty;
			
			item.FactoryPrice = varFactoryPrice;
			
			item.Gst = varGst;
			
			item.CostOfGoods = varCostOfGoods;
			
			item.Remark = varRemark;
			
			item.Discount = varDiscount;
			
			item.CreatedBy = varCreatedBy;
			
			item.CreatedOn = varCreatedOn;
			
			item.ModifiedBy = varModifiedBy;
			
			item.ModifiedOn = varModifiedOn;
			
			item.UniqueID = varUniqueID;
			
			item.Userfld1 = varUserfld1;
			
			item.Userfld2 = varUserfld2;
			
			item.Userfld3 = varUserfld3;
			
			item.Userfld4 = varUserfld4;
			
			item.Userfld5 = varUserfld5;
			
			item.Userfld6 = varUserfld6;
			
			item.Userfld7 = varUserfld7;
			
			item.Userfld8 = varUserfld8;
			
			item.Userfld9 = varUserfld9;
			
			item.Userfld10 = varUserfld10;
			
			item.Userflag1 = varUserflag1;
			
			item.Userflag2 = varUserflag2;
			
			item.Userflag3 = varUserflag3;
			
			item.Userflag4 = varUserflag4;
			
			item.Userflag5 = varUserflag5;
			
			item.Userfloat1 = varUserfloat1;
			
			item.Userfloat2 = varUserfloat2;
			
			item.Userfloat3 = varUserfloat3;
			
			item.Userfloat4 = varUserfloat4;
			
			item.Userfloat5 = varUserfloat5;
			
			item.Userint1 = varUserint1;
			
			item.Userint2 = varUserint2;
			
			item.Userint3 = varUserint3;
			
			item.Userint4 = varUserint4;
			
			item.Userint5 = varUserint5;
			
			item.BalanceBefore = varBalanceBefore;
			
			item.BalanceAfter = varBalanceAfter;
			
			item.Deleted = varDeleted;
			
			item.Stockinrefno = varStockinrefno;
			
			item.Isdiscrepancy = varIsdiscrepancy;
			
			item.Userfloat6 = varUserfloat6;
			
			item.Userfloat7 = varUserfloat7;
			
			item.Userfloat8 = varUserfloat8;
			
			item.Userfloat9 = varUserfloat9;
			
			item.Userfloat10 = varUserfloat10;
			
		
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		
		/// <summary>
		/// Updates a record, can be used with the Object Data Source
		/// </summary>
		public static void Update(string varPurchaseOrderDetRefNo,string varItemNo,string varPurchaseOrderHdrRefNo,DateTime? varExpiryDate,decimal? varQuantity,decimal? varRemainingQty,decimal varFactoryPrice,double varGst,decimal varCostOfGoods,string varRemark,decimal? varDiscount,string varCreatedBy,DateTime? varCreatedOn,string varModifiedBy,DateTime? varModifiedOn,Guid varUniqueID,string varUserfld1,string varUserfld2,string varUserfld3,string varUserfld4,string varUserfld5,string varUserfld6,string varUserfld7,string varUserfld8,string varUserfld9,string varUserfld10,bool? varUserflag1,bool? varUserflag2,bool? varUserflag3,bool? varUserflag4,bool? varUserflag5,decimal? varUserfloat1,decimal? varUserfloat2,decimal? varUserfloat3,decimal? varUserfloat4,decimal? varUserfloat5,int? varUserint1,int? varUserint2,int? varUserint3,int? varUserint4,int? varUserint5,int varBalanceBefore,int varBalanceAfter,bool? varDeleted,string varStockinrefno,bool? varIsdiscrepancy,decimal? varUserfloat6,decimal? varUserfloat7,decimal? varUserfloat8,decimal? varUserfloat9,decimal? varUserfloat10)
		{
			PurchaseOrderDet item = new PurchaseOrderDet();
			
				item.PurchaseOrderDetRefNo = varPurchaseOrderDetRefNo;
			
				item.ItemNo = varItemNo;
			
				item.PurchaseOrderHdrRefNo = varPurchaseOrderHdrRefNo;
			
				item.ExpiryDate = varExpiryDate;
			
				item.Quantity = varQuantity;
			
				item.RemainingQty = varRemainingQty;
			
				item.FactoryPrice = varFactoryPrice;
			
				item.Gst = varGst;
			
				item.CostOfGoods = varCostOfGoods;
			
				item.Remark = varRemark;
			
				item.Discount = varDiscount;
			
				item.CreatedBy = varCreatedBy;
			
				item.CreatedOn = varCreatedOn;
			
				item.ModifiedBy = varModifiedBy;
			
				item.ModifiedOn = varModifiedOn;
			
				item.UniqueID = varUniqueID;
			
				item.Userfld1 = varUserfld1;
			
				item.Userfld2 = varUserfld2;
			
				item.Userfld3 = varUserfld3;
			
				item.Userfld4 = varUserfld4;
			
				item.Userfld5 = varUserfld5;
			
				item.Userfld6 = varUserfld6;
			
				item.Userfld7 = varUserfld7;
			
				item.Userfld8 = varUserfld8;
			
				item.Userfld9 = varUserfld9;
			
				item.Userfld10 = varUserfld10;
			
				item.Userflag1 = varUserflag1;
			
				item.Userflag2 = varUserflag2;
			
				item.Userflag3 = varUserflag3;
			
				item.Userflag4 = varUserflag4;
			
				item.Userflag5 = varUserflag5;
			
				item.Userfloat1 = varUserfloat1;
			
				item.Userfloat2 = varUserfloat2;
			
				item.Userfloat3 = varUserfloat3;
			
				item.Userfloat4 = varUserfloat4;
			
				item.Userfloat5 = varUserfloat5;
			
				item.Userint1 = varUserint1;
			
				item.Userint2 = varUserint2;
			
				item.Userint3 = varUserint3;
			
				item.Userint4 = varUserint4;
			
				item.Userint5 = varUserint5;
			
				item.BalanceBefore = varBalanceBefore;
			
				item.BalanceAfter = varBalanceAfter;
			
				item.Deleted = varDeleted;
			
				item.Stockinrefno = varStockinrefno;
			
				item.Isdiscrepancy = varIsdiscrepancy;
			
				item.Userfloat6 = varUserfloat6;
			
				item.Userfloat7 = varUserfloat7;
			
				item.Userfloat8 = varUserfloat8;
			
				item.Userfloat9 = varUserfloat9;
			
				item.Userfloat10 = varUserfloat10;
			
			item.IsNew = false;
			if (System.Web.HttpContext.Current != null)
				item.Save(System.Web.HttpContext.Current.User.Identity.Name);
			else
				item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
		}
		#endregion
        
        
        
        #region Typed Columns
        
        
        public static TableSchema.TableColumn PurchaseOrderDetRefNoColumn
        {
            get { return Schema.Columns[0]; }
        }
        
        
        
        public static TableSchema.TableColumn ItemNoColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn PurchaseOrderHdrRefNoColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn ExpiryDateColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn QuantityColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn RemainingQtyColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn FactoryPriceColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn GstColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn CostOfGoodsColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn RemarkColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn DiscountColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[13]; }
        }
        
        
        
        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[14]; }
        }
        
        
        
        public static TableSchema.TableColumn UniqueIDColumn
        {
            get { return Schema.Columns[15]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld1Column
        {
            get { return Schema.Columns[16]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld2Column
        {
            get { return Schema.Columns[17]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld3Column
        {
            get { return Schema.Columns[18]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld4Column
        {
            get { return Schema.Columns[19]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld5Column
        {
            get { return Schema.Columns[20]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld6Column
        {
            get { return Schema.Columns[21]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld7Column
        {
            get { return Schema.Columns[22]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld8Column
        {
            get { return Schema.Columns[23]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld9Column
        {
            get { return Schema.Columns[24]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfld10Column
        {
            get { return Schema.Columns[25]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag1Column
        {
            get { return Schema.Columns[26]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag2Column
        {
            get { return Schema.Columns[27]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag3Column
        {
            get { return Schema.Columns[28]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag4Column
        {
            get { return Schema.Columns[29]; }
        }
        
        
        
        public static TableSchema.TableColumn Userflag5Column
        {
            get { return Schema.Columns[30]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat1Column
        {
            get { return Schema.Columns[31]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat2Column
        {
            get { return Schema.Columns[32]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat3Column
        {
            get { return Schema.Columns[33]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat4Column
        {
            get { return Schema.Columns[34]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat5Column
        {
            get { return Schema.Columns[35]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint1Column
        {
            get { return Schema.Columns[36]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint2Column
        {
            get { return Schema.Columns[37]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint3Column
        {
            get { return Schema.Columns[38]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint4Column
        {
            get { return Schema.Columns[39]; }
        }
        
        
        
        public static TableSchema.TableColumn Userint5Column
        {
            get { return Schema.Columns[40]; }
        }
        
        
        
        public static TableSchema.TableColumn BalanceBeforeColumn
        {
            get { return Schema.Columns[41]; }
        }
        
        
        
        public static TableSchema.TableColumn BalanceAfterColumn
        {
            get { return Schema.Columns[42]; }
        }
        
        
        
        public static TableSchema.TableColumn DeletedColumn
        {
            get { return Schema.Columns[43]; }
        }
        
        
        
        public static TableSchema.TableColumn StockinrefnoColumn
        {
            get { return Schema.Columns[44]; }
        }
        
        
        
        public static TableSchema.TableColumn IsdiscrepancyColumn
        {
            get { return Schema.Columns[45]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat6Column
        {
            get { return Schema.Columns[46]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat7Column
        {
            get { return Schema.Columns[47]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat8Column
        {
            get { return Schema.Columns[48]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat9Column
        {
            get { return Schema.Columns[49]; }
        }
        
        
        
        public static TableSchema.TableColumn Userfloat10Column
        {
            get { return Schema.Columns[50]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string PurchaseOrderDetRefNo = @"PurchaseOrderDetRefNo";
			 public static string ItemNo = @"ItemNo";
			 public static string PurchaseOrderHdrRefNo = @"PurchaseOrderHdrRefNo";
			 public static string ExpiryDate = @"ExpiryDate";
			 public static string Quantity = @"Quantity";
			 public static string RemainingQty = @"RemainingQty";
			 public static string FactoryPrice = @"FactoryPrice";
			 public static string Gst = @"GST";
			 public static string CostOfGoods = @"CostOfGoods";
			 public static string Remark = @"Remark";
			 public static string Discount = @"Discount";
			 public static string CreatedBy = @"CreatedBy";
			 public static string CreatedOn = @"CreatedOn";
			 public static string ModifiedBy = @"ModifiedBy";
			 public static string ModifiedOn = @"ModifiedOn";
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
			 public static string BalanceBefore = @"BalanceBefore";
			 public static string BalanceAfter = @"BalanceAfter";
			 public static string Deleted = @"Deleted";
			 public static string Stockinrefno = @"stockinrefno";
			 public static string Isdiscrepancy = @"isdiscrepancy";
			 public static string Userfloat6 = @"userfloat6";
			 public static string Userfloat7 = @"userfloat7";
			 public static string Userfloat8 = @"userfloat8";
			 public static string Userfloat9 = @"userfloat9";
			 public static string Userfloat10 = @"userfloat10";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
